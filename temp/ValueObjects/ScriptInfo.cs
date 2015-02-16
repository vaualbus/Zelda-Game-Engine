using System;
using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.ValueObjects
{
    [ComVisible(false)]
    public class ScriptInfo
    {
        public string ScriptName { get; private set; }

        public string ScriptPath { get; private set; }

        public DateTime CompiledAt { get; private set; }

        public ScriptInfo(string scriptName, string scriptPath = "")
        {
            ScriptName = scriptName;
            ScriptPath = scriptPath;
            CompiledAt = DateTime.Now;
        }
    }
}