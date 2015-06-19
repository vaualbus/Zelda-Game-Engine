using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game.Attributes;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public abstract class BaseScriptEngine : IScriptEngine, IDisposable
    {
        private IContainer _containerBuilder;

        public IScriptRepository ScriptRepository { get; private set; }

        public IGameEngine GameEngine { get; private set; }

        public virtual GameScript[] CurrentLoadedScripts { get; protected set; }
       
        public  ILogger Logger { get; private set; }

        public IProjectManager ProjectManager { get; protected set; }

        public  IScriptParamaterProvider ParamsProvider { get; private set; }

        public  IScriptCompiler ScriptCompiler { get; private set; }

        public  ILifetimeScope ScriptSystemLifetimeScope { get { return  _containerBuilder.BeginLifetimeScope(); } }
       
        public ContainerBuilder ContainerBuilder { get; private set; }

        protected BaseScriptEngine(IGameEngine gameEngine)
        {
            GameEngine = gameEngine;
        }

        public void InitializeEngine()
        {
            var builder = new ContainerBuilder();
            builder.Register(
                context => new AutofacControllerFactory(() => ScriptSystemLifetimeScope, DependencyResolver.Current))
                .As<IDependencyResolver>()
                .SingleInstance();

            RegisterComponents(builder);

            builder.RegisterType<InternalScriptActivator>()
                .As<IScriptActivator>()
                .InstancePerLifetimeScope();

            _containerBuilder = builder.Build();

            ContainerBuilder = builder;


            DependencyResolver.Set(_containerBuilder.Resolve<IDependencyResolver>());

            Logger = _containerBuilder.Resolve<ILogger>();

            object scriptCompiler;
            if (!_containerBuilder.TryResolve(typeof (IScriptCompiler), out scriptCompiler))
                ScriptCompiler = new CSharpScriptCompiler(this, Logger);
            else
                ScriptCompiler = _containerBuilder.Resolve<IScriptCompiler>();

            ParamsProvider = _containerBuilder.Resolve<IScriptParamaterProvider>();

            ScriptRepository = new ScriptRepository(this, ScriptCompiler,
                _containerBuilder.Resolve<IDependencyResolver>(),
                _containerBuilder.Resolve<IScriptActivator>(),
                _containerBuilder.Resolve<ILogger>());

            ProjectManager = _containerBuilder.Resolve<IProjectManager>();

            //ProjectManager.CopyEngineFileIfNecessary();

            ScanAssemblies();

            OnInitializeEngine();

        }


        public abstract void Update(float dt);

        public virtual IEnumerable<GameScript> GetScripts()
        {
            return ScriptRepository.Scripts.Select(t => t.Value.ScriptManager.CurrentMenagedScript);
        }

        public virtual IScriptManager GetScript(string name)
        {
            return ScriptRepository.GetScriptManager(name);
        }

        public virtual RuntimeScript AddScript(ScriptableGameObject go, string scriptName, string fileName)
        {
            return new RuntimeScript(go, ScriptCompiler, ScriptRepository, scriptName, fileName);
        }

        public virtual GameScript AddScript(ScriptableGameObject go, string scriptName, CompiledScript compiledScript)
        {
            return ScriptRepository.AddScript(go, scriptName, compiledScript);
        }

        public ScriptableGameObject AddScript(string goName, string fileName, object[] @params = null)
        {
            var go = GameEngine.GameObjectFactory.CreateEmpty<ScriptableGameObject>(goName);
            AddScript(go, goName, ScriptCompiler.Compile(fileName));

            ParamsProvider.AddParamater(goName, @params);

            return go;
        }

        public void PerformScriptBinding()
        {
            foreach (var scriptGo in ScriptRepository.Scripts)
            {
                var scriptDataFormAttributes = scriptGo.Value.GetAttibutes<DataFromAttribute>();
                foreach (var scriptDataFormAttribute in scriptDataFormAttributes)
                {
                    var script2 = ScriptRepository.Scripts.ContainsKey(scriptDataFormAttribute.Value.ScriptName) ? ScriptRepository.Scripts[scriptDataFormAttribute.Value.ScriptName] : null; //TryGetScriptGameObject(scriptDataFormAttribute.Value.ScriptName);
                    if (script2 != null && scriptGo.Key != script2.Name)
                    {
                        //Set the current script value to the correct script value
                        scriptGo.Value.ScriptManager.SetScriptValue(scriptDataFormAttribute.Key, script2.ScriptManager.GetScriptValue(scriptDataFormAttribute.Value.FieldName));
                    }
                }
            }
        }

        protected virtual void OnInitializeEngine()
        {
        }

        private void ScanAssemblies()
        {

            if(!Directory.Exists(GameEngine.GameConfig.BaseDirectory))
                return;

            foreach (var dll in Directory.GetFiles(GameEngine.GameConfig.BaseDirectory)
                                         .Where(t => (new FileInfo(t))
                                                     .Extension == ".dll" &&
                                                     IsDotNetAssembly32(t) && 
                                                     IsDotNetAssembly64(t)))
            {
                //Logger.LogInfo($"Loading file: {dll}");

                Assembly loadedAssembly = null;
                try
                {
                    loadedAssembly = Assembly.LoadFrom(dll);
                    if (loadedAssembly == null)
                        continue;
                }
                catch (Exception ex)
                {
                    Logger.LogInfo("Cannot load the assemblies: {0}, the assembly is not a valid .net assembly",
                        new FileInfo(dll).Name);

                    Logger.LogInfo("Error detail {0}", ex.Message);
                    continue;
                }
                if (ScriptCompiler.AdditionalAssemblies.Contains(loadedAssembly))
                {
                    var assemblyDirAssemblyInfo = Assembly.LoadFrom(new FileInfo(dll).Name);

                    if (assemblyDirAssemblyInfo.GetName().Version == loadedAssembly.GetName().Version)
                        continue;

                    ScriptCompiler.AdditionalAssemblies.Remove(loadedAssembly);
                    ScriptCompiler.AdditionalAssemblies.Add(assemblyDirAssemblyInfo);
                }

                //Add System.Drawing
                ScriptCompiler.AdditionalAssemblies.Add(loadedAssembly);
            }
        }

        public virtual ScriptableGameObject AddScript(GameObject parentGo, Dictionary<string, string> scriptFiles, QuestLoaderScriptType scriptType)
        {
            return null;
        }

        public abstract void RegisterComponents(ContainerBuilder builder);

        public virtual void OnDispose()
        {
        }

        public void Dispose()
        {
            OnDispose();

            ScriptSystemLifetimeScope.Dispose();
        }

        private static bool IsDotNetAssembly32(string peFile)
        {
            var dataDictionaryRva = new uint[16];
            var dataDictionarySize = new uint[16];

            var fs = new FileStream(peFile, FileMode.Open, FileAccess.Read);
            var reader = new BinaryReader(fs);

            //PE Header starts @ 0x3C (60). Its a 4 byte header.
            fs.Position = 0x3C;

            var peHeader = reader.ReadUInt32();

            //Moving to PE Header start location...
            fs.Position = peHeader;

            //Read the signature
            reader.ReadUInt32();

            //We can also show all these value, but we will be       
            //limiting to the CLI header test.

            //Read the machine
            reader.ReadUInt16();

            //read the section
            reader.ReadUInt16();

            //read the timestamp
            reader.ReadUInt32();

            //Read the pSymbolTable
            reader.ReadUInt32();

            //Read the noOfSybol Table
            reader.ReadUInt32();

            //Read  the optional Header Size
            reader.ReadUInt16();

            //Read the charateristics
            reader.ReadUInt16();

            /*
                Now we are at the end of the PE Header and from here, the
                            PE Optional Headers starts...
                    To go directly to the datadictionary, we'll increase the      
                    stream’s current position to with 96 (0x60). 96 because,
                            28 for Standard fields
                            68 for NT-specific fields
                From here DataDictionary starts...and its of total 128 bytes. DataDictionay has 16 directories in total,
                doing simple maths 128/16 = 8.
                So each directory is of 8 bytes.
                            In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.

                btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
         */

            var dataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x60);
            fs.Position = dataDictionaryStart;
            for (int i = 0; i < 15; i++)
            {
                dataDictionaryRva[i] = reader.ReadUInt32();
                dataDictionarySize[i] = reader.ReadUInt32();
            }
            if (dataDictionaryRva[14] == 0)
                return false;

            fs.Close();
            return true;
        }

        private static bool IsDotNetAssembly64(string peFile)
        {
            var dataDictionaryRva = new uint[16];
            var dataDictionarySize = new uint[16];

            var fs = new FileStream(peFile, FileMode.Open, FileAccess.Read);
            var reader = new BinaryReader(fs);

            //PE Header starts @ 0x3C (60). Its a 4 byte header.
            fs.Position = 0x3C;

            var peHeader = reader.ReadUInt32();

            //Moving to PE Header start location...
            fs.Position = peHeader;

            //Read the signature
            reader.ReadUInt32();

            //We can also show all these value, but we will be       
            //limiting to the CLI header test.

            //Read the machine
            reader.ReadUInt16();

            //read the section
            reader.ReadUInt16();

            //read the timestamp
            reader.ReadUInt32();

            //Read the pSymbolTable
            reader.ReadUInt32();

            //Read the noOfSybol Table
            reader.ReadUInt32();

            //Read  the optional Header Size
            reader.ReadUInt16();

            //Read the charateristics
            reader.ReadUInt16();

            /*
                Now we are at the end of the PE Header and from here, the
                            PE Optional Headers starts...
                    To go directly to the datadictionary, we'll increase the      
                    stream’s current position to with 96 (0x60). 96 because,
                            28 for Standard fields
                            68 for NT-specific fields
                From here DataDictionary starts...and its of total 128 bytes. DataDictionay has 16 directories in total,
                doing simple maths 128/16 = 8.
                So each directory is of 8 bytes.
                            In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.

                btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
         */

            var dataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x70);
            fs.Position = dataDictionaryStart;
            for (int i = 0; i < 15; i++)
            {
                dataDictionaryRva[i] = reader.ReadUInt32();
                dataDictionarySize[i] = reader.ReadUInt32();
            }
            if (dataDictionaryRva[14] == 0)
                return false;

            fs.Close();
            return true;
        }
    }
}