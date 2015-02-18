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
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public abstract class BaseScriptEngine : IScriptEngine, IDisposable
    {
        private IContainer _containerBuilder;

        public IScriptRepository ScriptRepository { get; private set; }

        public Config Config { get; private set; }

        public IGameEngine GameEngine { get; private set; }

        public string CurrentScriptName { get; set; }

        public virtual GameScript[] CurrentLoadedScripts { get; protected set; }
       
        public  ILogger Logger { get; private set; }

        public  IScriptParamaterProvider ParamsProvider { get; private set; }

        public  IScriptCompiler ScriptCompiler { get; private set; }

        public  ILifetimeScope ScriptSystemLifetimeScope { get { return  _containerBuilder.BeginLifetimeScope(); } }
       
        public ContainerBuilder ContainerBuilder { get; private set; }

        protected BaseScriptEngine(Config config, IGameEngine gameEngine = null)
        {
            Config = config;
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

            //If the project path is set, than copy the game engine dll in it
            ///TODO(alberto): Better do when we create the project?
            //if (Config.GameScriptConfig.ProjectFolder != string.Empty && !File.Exists(".test"))
            //{
            //    var applicationFiles = new List<string>()
            //    {
            //        "ZeldaEngine.Base.dll",
            //        "ZeldaEngine.ScriptEngine.dll",
            //        "ZeldaEngine.Base.pdb",
            //        "ZeldaEngine.ScriptEngine.pdb"
            //    };

            //    var directoryFiles = Directory.GetFiles(Config.GameConfig.BaseDirectory).Select(t => new FileInfo(t).Name);
            //    var engineFiles = directoryFiles.Intersect(applicationFiles);

            //    foreach (var projectFile in engineFiles.Where(t => t != null))
            //    {
            //        if (File.Exists(Path.Combine(Config.GameScriptConfig.ProjectFolder, projectFile)) &&
            //            new FileInfo(Path.Combine(Config.GameScriptConfig.ProjectFolder, projectFile)).Extension == ".dll")
            //        {
            //            //We have a dll file, so now we will load and check if the new file is newer.
            //            //If so we will copy it.
            //            var currentFile = projectFile;
            //            var baseFile = directoryFiles.FirstOrDefault(t => t == currentFile);

            //            var engineFileAssembly = Assembly.LoadFrom(Path.Combine(Config.GameConfig.BaseDirectory, baseFile));
            //            var projectFileAssembly = Assembly.LoadFrom(Path.Combine(Config.GameScriptConfig.ProjectFolder, currentFile));

            //            if (FileVersionInfo.GetVersionInfo(engineFileAssembly.Location).FileVersion ==
            //                FileVersionInfo.GetVersionInfo(projectFileAssembly.Location).FileVersion)
            //                continue;
            //        }

            //        if (File.Exists(Path.Combine(Config.GameScriptConfig.ProjectFolder, projectFile)))
            //            continue;

            //        if (!Directory.Exists(Config.GameScriptConfig.ProjectFolder))
            //            Directory.CreateDirectory(Config.GameScriptConfig.ProjectFolder);

            //        File.Copy(projectFile, Path.Combine(Config.GameScriptConfig.ProjectFolder, projectFile));
            //    }
            //}

            ScanAssemblies();

            OnInitializeEngine();

            GenerateProject();
        }

        public void UpdateEnviromentInfo(GameEviromentCollection gameEnviromentCollection)
        {
            Config = gameEnviromentCollection.Config;
        }

        public bool GenerateProject()
        {
            return ProjectUtil.CreateProject(this);
        }

        public void UpdateProject()
        {
            ProjectUtil.UpdateProject(this);
        }

        public void DeleteProject()
        {
            ProjectUtil.DeleteProject(Config.GameScriptConfig.ProjectName);
        }

        public virtual void Update(IGameView view, float dt)
        {
        }

        public virtual IEnumerable<GameScript> GetScripts()
        {
            return null;
        }

        public virtual IScriptManager GetScript(IGameView gameScreen, string name)
        {
            return ScriptRepository.GetScriptManager(gameScreen, name);
        }

        public virtual Type GetScriptType(IGameView gameScreen, string scriptName)
        {
            return null;
        }

        public virtual object GetScriptValue(IGameView gameScreen, string scriptName, string scriptFieldName)
        {
            return null;
        }

        public virtual void AddScriptParams(IGameView gameScreen, string name, object[] @params)
        {
        }

        public virtual RuntimeScript AddScript(IGameView screen, string scriptName, string fileName)
        {
            return new RuntimeScript(screen, ScriptCompiler, ScriptRepository, scriptName, fileName);
        }

        public virtual GameScript AddScript(IGameView screen, string scriptName, CompiledScript compiledScript)
        {
            return ScriptRepository.AddScript(screen, scriptName, compiledScript);
        }

        public virtual void SetScriptInitialLocation(IGameView gameScreen, string scriptName, Vector2 pos)
        {
        }

        protected virtual void OnInitializeEngine()
        {
        }

        public abstract void RegisterComponents(ContainerBuilder builder);

        private void ScanAssemblies()
        {
            if (Config.GameScriptConfig.ProjectFolder == string.Empty)
                return;

            if (ProjectUtil.GetProject(Config.GameScriptConfig.ProjectName))
            {
                ScriptCompiler.AdditionalAssemblies.AddRange(ProjectUtil.GetProjectAssemblies(Config.GameScriptConfig.ProjectName));
                //AddAssembliesFromDirectoryName("Assemblies");
                return;
            }

            //AddAssembliesFromDirectoryName("Assemblies");

            string dllDirectory;
            try
            {
                var binDirectory = Directory.GetDirectories(Directory.GetDirectories(Config.GameScriptConfig.ProjectFolder).First(t => t.EndsWith("bin")));
#if DEBUG
                dllDirectory = binDirectory.First(t => t.EndsWith("Debug"));
#else
                dllDirectory = binDirectory.First(t => t.EndsWith("Release"));
#endif
            }
            catch
            { 
                dllDirectory = Config.GameScriptConfig.ProjectFolder;
            }

            if(!Directory.Exists(dllDirectory))
                return;

            foreach (var dll in Directory.GetFiles(dllDirectory).Where(t => (new FileInfo(t)).Extension == ".dll"))
            {
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

                //Add System.Drwaing
                ScriptCompiler.AdditionalAssemblies.Add(loadedAssembly);
            }
        }

        public virtual void OnDispose()
        {
            
        }

        public void Dispose()
        {
            OnDispose();

            ScriptSystemLifetimeScope.Dispose();
        }
    }
}