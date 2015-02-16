using System;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptActivator
    {
        object CreateInstance(Type type, params object[] @params);

        TObject CreateInstance<TObject>(Type type, params object[] @params); 
    }
}