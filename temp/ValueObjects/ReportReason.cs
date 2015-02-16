using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.ValueObjects
{
    [ComVisible(true)]
    public enum ReportReason
    {
        Error,
        Warning,
        Log
    }
}
