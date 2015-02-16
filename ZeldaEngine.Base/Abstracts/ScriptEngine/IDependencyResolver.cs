using System;
using System.Collections.Generic;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType); 
    }
}