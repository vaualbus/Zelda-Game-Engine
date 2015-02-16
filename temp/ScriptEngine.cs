using System;
using System.Runtime.InteropServices;
using ZeldaEngine.ScriptEngine.Abstracts;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.ScriptEngine
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("44F9F03C-7B29-4F13-95AD-593D065CA902")]
    public class ScriptEngine : IScriptEngine
    {
        public IScriptParamaterProvider ParamsProvider { get; private set; }

        public IScriptManager ScriptManager { get; private set; }

        public IScriptCompiler ScriptCompiler { get; private set; }

        [ComVisible(true)]
        public void InitializeEngine()
        {
        }

        public void Report(Type scriptType, string message, ReportReason reason)
        {
        }

        public void Report(string message, ReportReason reason)
        {
        }

        public void Report<TType>(string message, ReportReason reason)
        {
        }
    }
}