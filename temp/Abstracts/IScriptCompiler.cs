using System.Runtime.InteropServices;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.ScriptEngine.Abstracts
{
    [ComVisible(false)]
    public interface IScriptCompiler
    {
        CompiledScript CompiledScript { get; }

        CompiledScript Compile(string fileName, bool isDebug = false);

        CompiledScript Compile();
    }
}
