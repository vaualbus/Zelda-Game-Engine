using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptCompiler
    {
        StringBuilder AdditionalCodeToCompile { get; }

        List<Assembly> AdditionalAssemblies { get; set; }

        CompiledScript Compile(string fileName);
    }
}
