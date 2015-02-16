using System;
using System.Runtime.InteropServices;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.ScriptEngine.Abstracts
{
    [Guid("A3E45B14-A8E7-4A1C-88E3-02F1DFDC867E")]
    public interface IScriptEngine
    {
        IScriptParamaterProvider ParamsProvider { get; }

        [ComVisible(false)]
        IScriptManager ScriptManager { get; }

        [ComVisible(false)]
        IScriptCompiler ScriptCompiler { get; } 

        [ComVisible(false)]
        void Report<TType>(string message, ReportReason reason);

        [ComVisible(false)]
        void Report(Type scriptType, string message, ReportReason reason);

        void Report(string message, ReportReason reason);

        void InitializeEngine();
    }
}
