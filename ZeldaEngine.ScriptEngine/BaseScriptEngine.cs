using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using System.Xml;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Abstracts;


namespace ZeldaEngine.ScriptEngine
{
    public abstract class BaseScriptEngine : IScriptEngine
    {
        private readonly Dictionary<string, ZeldaScript> _currentDirectoryScripts;
        private readonly Dictionary<string, object> _chachedScript;
        private StreamWriter _logFileWriter;

        protected BaseScriptEngine(bool needToGenerateProject = false)
        {
            NeedToGenerateProject = needToGenerateProject;
            _currentDirectoryScripts = new Dictionary<string, ZeldaScript>();
            _chachedScript = new Dictionary<string, object>();

            _logFileWriter = new StreamWriter(string.Format("{0}\\{1}{2}.log", EnginePath ?? AppDomain.CurrentDomain.BaseDirectory, "Log", _currentLogCount));

            _currentLogCount++;
        }

        private static int _currentLogCount;

        #region Proxy Fields
        public string EnginePath { get; set; }

        public string ProjectName { get; set; }

        public string ProjectFolder { get; set; }

        public string CurrentScriptName { get; set; }

        public string[] ExtraAssemblies { get; set; }

        public bool NeedToGenerateProject { get; private set; }

        public ZeldaScript[] CurrentLoadedScripts { get { return _currentDirectoryScripts.Values.ToArray(); } }
#endregion

        public IScriptParamaterProvider ParamsProvider { get; protected set; }

        public IScriptManager ScriptManager { get; protected set; }

        public IScriptCompiler ScriptCompiler { get; protected set; }

        public abstract void InitializeEngine();
        //{
        //    _logFileWriter = new StreamWriter(string.Format("{0}\\{1}.log", EnginePath, "Log"));
        //    Report("Init the engine Component", ReportReason.Log);

        //    ParamsProvider = new ScriptParmaterProvider(this);
        //    ScriptManager = new ScriptManager(this, ParamsProvider);
        //    ScriptCompiler = new CSharpScriptCompiler(this, ProjectFolder, ProjectName, additionalyAssenblies: ExtraAssemblies.ToArray());
        //}

        public virtual void Report(Type scriptType, string message, ReportReason reason)
        {
            _logFileWriter.WriteLine("{0}: {1} in {2}", GetMessageType(reason), message, scriptType);
        }

        public virtual void Report<TType>(string message, ReportReason reason)
        {
            Report(typeof(TType), message, reason);
        }

        #region Proxy Methods
       
        public virtual void Report(string message, ReportReason reason)
        {
            _logFileWriter.WriteLine("{0}: {1}", GetMessageType(reason), message);
        }

        public virtual void Dispose()
        {
            _logFileWriter.Dispose();
        }


        public virtual void Update(float dt)
        {
            var paramsForScript = ParamsProvider.GetParamatersForScript(CurrentScriptName);
            var runMethod = _currentDirectoryScripts[CurrentScriptName]
                .GetType()
                .GetMethods().FirstOrDefault(t => t.Name == "Run" && t.GetParameters().Length == paramsForScript.Length);

            runMethod.Invoke(_chachedScript[CurrentScriptName], paramsForScript);

            Thread.Sleep(10);

        }

        public virtual IEnumerable<ZeldaScript> GetScripts()
        {
            return null;
        }

        public virtual ZeldaScript GetScript(string name)
        {
            return null;
        }

        public virtual object GetScriptValue(string scriptName, string scriptFieldName)
        {
            return ScriptManager.GetScriptValue(scriptName, scriptFieldName);
        }

        public virtual void AddScriptParams(string scriptName, object[] @params)
        {
            var scriptType = GetScriptType(scriptName);
            ParamsProvider.AddParamater(scriptType, @params);
        }

        public virtual void SetScriptInitialLocation(string scriptName, Vector2 pos)
        {
        }

        #endregion

        protected void GenerateProject()
        {
            if (!NeedToGenerateProject)
                return;

            if (ProjectFolder == null && ProjectName == null)
                throw new InvalidOperationException("Cannot create the project. Please provide a project path and project name");

            var projectPath = string.Format("{0}/{1}", EnginePath, ProjectFolder);
            var projectFileName = string.Format("{0}/{1}.csproj", projectPath, ProjectName);
            
            if(!Directory.Exists(string.Format("{0}/{1}", EnginePath, ProjectFolder)))
            {
                try
                {
                    //Create the prjoct directory
                    Directory.CreateDirectory(projectPath);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format("Cannot create the project directory: {0}, Acess Denied", projectPath));
                }
            }

            //The project directory is not empty and contains arleady a project file no need to generate it.
            if(Directory.GetFiles(projectPath).Length > 0 && File.Exists(projectFileName))
                return;


            using (var sw = new StreamWriter(projectFileName))
            {
                //First we write the xml version
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                {
                    sw.WriteLine("<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
                    {
                        sw.WriteLine(@"<Import Project='$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props' Condition='Exists($(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props)\'/>");

                        sw.WriteLine("<PropertyGroup>");
                        {
                            sw.WriteLine("<Configuration Condition=\"'$(Configuration)' == '' \">Debug</Configuration>");
                            sw.WriteLine("<Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>");
                            sw.WriteLine("<ProjectGuid>{0}</ProjectGuid>", Guid.NewGuid());
                            sw.WriteLine("<OutputType>Library</OutputType>");
                            sw.WriteLine("<AppDesignerFolder>Properties</AppDesignerFolder>");
                            sw.WriteLine("<RootNamespace>{0}.dll</RootNamespace>", ProjectName);
                            sw.WriteLine("<AssemblyName>{0}.dll</AssemblyName>", ProjectName);

                            //TODO: Rember to add the check to get most high net framework version on the pc
                            sw.WriteLine("<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>");
                            sw.WriteLine("<FileAlignment>512</FileAlignment>");
                            sw.WriteLine("<TargetFrameworkProfile />");
                        }
                        sw.WriteLine("</PropertyGroup>");
                    }
                    sw.WriteLine("</Project>");
                }
                sw.WriteLine("");
            }
        }

        protected virtual string GetMessageType(ReportReason reason)
        {
            switch (reason)
            {
                case ReportReason.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return "Error";

                case ReportReason.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return "Warning";

                case ReportReason.Log:
                    Console.ForegroundColor = ConsoleColor.Green;
                    return "Log";

                default:
                    return "Unknow message";
            }
        }

        protected Type GetScriptType(object scriptNameName)
        {
            return null;
        }
    }
}