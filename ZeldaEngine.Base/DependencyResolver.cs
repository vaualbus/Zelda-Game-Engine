using System;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Services;

namespace ZeldaEngine.Base
{
    public class DependencyResolver
    {
        private IDependencyResolver _currentResolver;

        private static bool _isInit;
        private static DependencyResolver _instance;

        public static IDependencyResolver Current
        {
            get { return _instance._currentResolver; }
            private set { _instance._currentResolver = value; }
        }

        public static IScriptActivator CurrentActivator { get; private set; }

        static DependencyResolver()
        {
            _instance = new DependencyResolver();
            _isInit = true;

            if (CurrentActivator == null)
                CurrentActivator = new InternalScriptActivator();

            if (Current == null)
                Current = new InternalDependecyResolver();
        }

        public static void Set(IDependencyResolver resolver)
        {
            CurrentActivator = new InternalScriptActivator();

            if (!_isInit)
                throw new Exception("Runtime broke down!");

            if (resolver == null)
                throw new InvalidOperationException("Cannot set a not instetiate resolver");

            _instance._currentResolver = resolver;
        }

        public static object Resolve(Type type)
        {
            return _instance._currentResolver.GetService(type); //Return to each value of each obj in the cypor
        }

        private static void Init()
        {
            _instance = new DependencyResolver();
            _instance._currentResolver = new InternalDependecyResolver();
            _isInit = true;
        }
    }
}