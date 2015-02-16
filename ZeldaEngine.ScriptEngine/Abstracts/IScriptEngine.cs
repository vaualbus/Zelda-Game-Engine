using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.ValueObjects;


namespace ZeldaEngine.ScriptEngine.Abstracts
{
    public interface IScriptEngine : IDisposable
    {
#region Proxy Fields
        string EnginePath { get; set; }

        string ProjectName { get; set; }

        string ProjectFolder { get; set; }

        string CurrentScriptName { get; set; }

        string[] ExtraAssemblies { get; set; }

        bool NeedToGenerateProject { get; }

        ZeldaScript[] CurrentLoadedScripts { get;  }
#endregion

        IScriptParamaterProvider ParamsProvider { get; }

        IScriptManager ScriptManager { get; }

        IScriptCompiler ScriptCompiler { get; } 

        void Report<TType>(string message, ReportReason reason);

        void Report(Type scriptType, string message, ReportReason reason);

#region Proxy Methods
        void Report(string message, ReportReason reason);

        void InitializeEngine();

        void Update(float dt);

        ZeldaScript GetScript(string name);

        object GetScriptValue(string scriptName, string scriptFieldName);

        void AddScriptParams(string name, object[] @params);

        void SetScriptInitialLocation(string scriptName, Vector2 pos);

        void Dispose();

        #endregion
    }
}
