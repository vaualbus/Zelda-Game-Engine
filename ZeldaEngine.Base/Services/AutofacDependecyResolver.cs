using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.Services
{
    public class AutofacControllerFactory : IDependencyResolver
    {
        private readonly Func<ILifetimeScope> _scope;
        private readonly IDependencyResolver _parent;

        public AutofacControllerFactory(Func<ILifetimeScope> scope, IDependencyResolver parent)
        {
            _scope = scope;
            _parent = parent;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                return null;

            object result;
            if (_scope().TryResolve(serviceType, out result))
                return result;

            var reg = _scope().ComponentRegistry.Registrations.ToList();

            return _parent.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                return null;

            object results;
            if (_scope().TryResolve(typeof(IEnumerable<>).MakeGenericType(serviceType), out results))
                return results as IEnumerable<object>;

            var reg = _scope().ComponentRegistry.Registrations.ToList();

            return _parent.GetServices(serviceType);
        }
    }
}