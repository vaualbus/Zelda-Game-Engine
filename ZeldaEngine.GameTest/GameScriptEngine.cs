using System;
using System.Collections.Generic;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.ScriptEngine.Abstracts;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest
{
    public class GameScriptEngine : IScriptEngine
    {
#region Proxy Method 
        public string EnginePath { get; set; }
        public string ProjectName { get; set; }
        public string ProjectFolder { get; set; }
        public string CurrentScriptName { get; set; }
        public string[] ExtraAssemblies { get; set; }
        public ZeldaScript[] CurrentLoadedScripts { get; private set; }
        #endregion

        public IScriptParamaterProvider ParamsProvider { get; private set; }

        public IScriptManager ScriptManager { get; private set; }

        public IScriptCompiler ScriptCompiler { get; private set; }

        public GameScriptEngine()
        {
            InitializeEngine();
        }

        public void Report<TType>(string message, ReportReason reason)
        {
            Report(typeof(TType), message, reason);
        }

        private string GetMessageType(ReportReason reason)
        {
            switch(reason)
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


        public void Report(Type scriptType, string message, ReportReason reason)
        {
            Console.WriteLine("{0}: {1} in {2}", GetMessageType(reason), message, scriptType);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Report(string message, ReportReason reason)
        {
            Console.WriteLine("{0}: {1}", GetMessageType(reason), message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void InitializeEngine()
        {
            ParamsProvider = new TestScriptParmaterProvider(this);
            ScriptCompiler = new CSharpScriptCompiler(this, additionalyAssenblies: new[] { "ZeldaEngine.GameTest.exe" });
            ScriptManager = new ScriptManager(this, ParamsProvider);
        }

#region Proxy Method 
        public void Update(float dt)
        {
            throw new NotImplementedException();
        }

        public ZeldaScript GetScript(string name)
        {
            throw new NotImplementedException();
        }

        public object GetScriptValue(string scriptName, string scriptFieldName)
        {
            throw new NotImplementedException();
        }

        public void AddScriptParams(string name, object[] @params)
        {
            throw new NotImplementedException();
        }

        public void SetScriptInitialLocation(string scriptName, Vector2 pos)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
