using System;
using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.Services
{
    public class InternalDependecyResolver : IDependencyResolver
    {
        public InternalDependecyResolver()
        {
        }

        public object GetService(Type serviceType)
        {
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return null;
        }
    }
}