using System.Runtime.InteropServices;
using ZeldaEngine.Base.ValueObjects;


namespace ZeldaEngine.ScriptEngine.Abstracts
{
    public interface IScriptCompiler
    {
        CompiledScript CompiledScript { get; }

        CompiledScript Compile(string fileName, bool isDebug = false);

        CompiledScript Compile();
    }
}
