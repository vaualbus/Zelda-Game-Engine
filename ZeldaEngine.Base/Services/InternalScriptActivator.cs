using System;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.Services
{
    public class InternalScriptActivator : IScriptActivator
    {
        public object CreateInstance(Type type, params object[] @params)
        {
            return Activator.CreateInstance(type, @params);
        }

        public TObject CreateInstance<TObject>(Type type, params object[] @params)
        {
            return (TObject)CreateInstance(type, @params);
        }
    }
}