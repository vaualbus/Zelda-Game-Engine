﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public class ScriptManager : IScriptManager
    {
        private ILogger _logger;
        private IScriptEngine _engine;
        private IDependencyResolver _resolver;
        private readonly IScriptActivator _scriptActivator;
        private IScriptCompiler _compiler;
        private readonly IScriptRepository _scriptRepository;

        private CompiledScript _currentCompiledScript;
        private volatile GameScript _currentCachedScriptInstance;
        private RuntimeScript _runtimeScript;

        public GameScript CurrentMenagedScript
        {
            get { return _currentCachedScriptInstance; }
        }

        public RuntimeScript RuntimeScript
        {
            get { return _runtimeScript; }
        }

        public ScriptManager(IScriptEngine engine, IScriptCompiler compiler, IScriptRepository scriptRepository,
            IDependencyResolver resolver, IScriptActivator scriptActivator, ILogger logger)
        {
            _logger = logger;
            _engine = engine;
            _compiler = compiler;
            _scriptRepository = scriptRepository;
            _resolver = resolver;
            _scriptActivator = scriptActivator;

            _currentCachedScriptInstance = null;
            _currentCompiledScript = null;

            _logger.LogInfo("Init script Manager");
        }

        public GameScript AddScript(IGameView gameView, CompiledScript script, string scriptName)
        {
            _currentCompiledScript = script;
            _runtimeScript = new RuntimeScript(_scriptRepository, gameView, script, scriptName);

            var objValue = CreateRuntimeScript(_runtimeScript);
            if (objValue == null)
            {
                _logger.LogError("Cannot create the runtime script");
                return null;
            }

            var scriptDesc = new ScriptInfo(scriptName, script.CompiledPath);

            _currentCachedScriptInstance = objValue;

            //Set the script fields to the current engine
            SetScriptFields("Engine", _engine);
            SetScriptFields("ScriptInfo", scriptDesc);
            SetScriptFields("Logger", _logger);
            SetScriptFields("RenderEngine", _engine.GameEngine?.RenderEngine);
            SetScriptFields("InputManager", _engine.GameEngine?.InputManager);
            SetScriptFields("AudioEngine", _engine.GameEngine?.AudioEngine);
            SetScriptFields("ResourceLoader", _engine.GameEngine?.ResourceLoader);

            return _currentCachedScriptInstance;
        }

        public bool CreateScript(RuntimeScript runtimeScript)
        {
            _currentCompiledScript = runtimeScript.CompiledScript;
            _runtimeScript = runtimeScript;

            var scriptName = runtimeScript.Name;
            var script = runtimeScript.CompiledScript;

            var objValue = CreateRuntimeScript(runtimeScript);
            if (objValue == null)
                return false;

            var scriptDesc = new ScriptInfo(scriptName, script.CompiledPath);

            _currentCachedScriptInstance = objValue;

            //Set script engine to the current engine
            SetScriptFields("Engine", _engine);
            SetScriptFields("ScriptInfo", scriptDesc);
            SetScriptFields("Logger", _logger);
            SetScriptFields("RenderEngine", null);

            return true;
        }

        public object ExcuteFunction(string funcName, object[] @params)
        {
            var method = FindMethod(funcName, @params ?? new object[0]);
            if (method == null)
            {
                _logger.LogError("Cannot found the method {0} in {1}", funcName,
                    _currentCompiledScript.ScriptType.ToString());
                return null;
            }

            return method.Invoke(_currentCachedScriptInstance, @params ?? new object[0]);
        }

        public MethodInfo GetMethod(string funcName, object[] @params)
        {
            return FindMethod(funcName, @params ?? new object[0]);
        }

        public object GetScriptValue(string propName)
        {
            try
            {
                var prop = _currentCompiledScript.Properties.FirstOrDefault(t => t.Name == propName);
                if (prop == null)
                {
                    _logger.LogError("Cannot find a property with name {0} on type {1}", propName,
                        _currentCompiledScript.ScriptType.ToString());
                    return null;
                }

                return prop.GetValue(_currentCachedScriptInstance, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception {0}", ex.Message);
                return null;
            }
        }

        public void SetScriptValue(string propName, object value)
        {
            try
            {
                var prop = _currentCompiledScript.Properties.FirstOrDefault(t => t.Name == propName);
                if (prop == null)
                {
                    _logger.LogError("Cannot find a property with name {0} on type {1}", propName,
                        _currentCompiledScript.ScriptType.ToString());
                    return;
                }
                prop.SetValue(_currentCachedScriptInstance, value);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception {0}", ex.Message);
            }
        }

        public void SetScriptFields(string fieldName, object value)
        {
            try
            {
                var prop = _currentCompiledScript.Fields.FirstOrDefault(t => t.Name == fieldName);

                if (prop == null)
                {
                    _logger.LogError("Cannot find a properties with the given Name.");
                    return;
                }

                prop.SetValue(_currentCachedScriptInstance, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Cannot create and instance of the script. Script cannot have a constructor that have paramaters");
                _logger.LogError("Debug Informotion {0}", ex.Message);
            }
        }

        public TReturn ExcuteFunction<TReturn>(string funcName, object[] @params)
        {
            return (TReturn) ExcuteFunction(funcName, @params);
        }

        public TValue GetScriptValue<TValue>(string propName)
        {
            return (TValue) GetScriptValue(propName);
        }

        private GameScript CreateRuntimeScript(RuntimeScript runtimeScript)
        {
            var script = runtimeScript.CompiledScript;
            var scriptCtors = script.ScriptType.GetConstructors().Select(t => t.GetParameters()).ToList()[0].ToList();


            GameScript objValue;
            if (scriptCtors.Count == 0)
                objValue = _scriptActivator.CreateInstance(script.ScriptType) as GameScript;
            else if (scriptCtors.Count == 1)
            {
                var values =
                    scriptCtors.Select(
                        controllerCtor =>
                            _resolver.GetService(controllerCtor.ParameterType) ??
                            (runtimeScript.CtorParams.ContainsKey(controllerCtor.ParameterType)
                                ? runtimeScript.CtorParams[controllerCtor.ParameterType].FirstOrDefault()
                                : null)).ToList();
                objValue = _scriptActivator.CreateInstance(script.ScriptType, values.ToArray()) as GameScript;
            }
            else
            {
                var values =
                    scriptCtors.Select(
                        controllerCtor =>
                            _resolver.GetServices(controllerCtor.ParameterType) ??
                            (runtimeScript.CtorParams.ContainsKey(controllerCtor.ParameterType)
                                ? runtimeScript.CtorParams[controllerCtor.ParameterType].ToArray()
                                : null)).ToList();
                var tempObj = new object[values.Count];

                for (var i = 0; i < values.Count; i++)
                    tempObj[i] = values[i];

                objValue = _scriptActivator.CreateInstance(script.ScriptType, values) as GameScript;
            }

            return objValue;
        }

        private MethodInfo FindMethod(string funcName, object[] @params)
        {
            try
            {
                var method = _currentCompiledScript.Methods.FirstOrDefault(t => t.Name == funcName && t.GetParameters().Length == @params.Length && MatchTypes(t, @params));
                if (method == null)
                {
                    _logger.LogError("Cannot find a method with the given Name {0} and the given paramaters {1}",
                        funcName, string.Join(", ", @params));
                    return null;
                }

                var methodParamsTypes = method.GetParameters().Select(t => t.ParameterType);
                var valuesTypes = @params?.Select(t => t.GetType()) ?? new Type[0];
                var differences = methodParamsTypes.Except(valuesTypes).Count();

                if (differences > 0)
                {
                    _logger.LogError("The given paramaters don't match the method signature");
                    return null;
                }

                return method;
            }
            catch (Exception ex)
            {
                _logger.LogError("Debug Informotion {0}", ex.Message);
                return null;
            }
        }

        private bool MatchTypes(MethodInfo methodInfo, object[] @params)
        {
            var paramsTypes = @params.Select(t => t.GetType()).ToArray();
            var providedMethodTypes = methodInfo.GetParameters().Select(t => t.ParameterType).ToArray();

            Debug.Assert(paramsTypes.Length == providedMethodTypes.Length);

            var results = new List<bool>();
            for (var i = 0; i < @params.Length; i++)
            {
                //Json.Net deserilize int value into int64 so we need to consider that a int32 instead.
                //if (paramsTypes[i] == typeof(Int64) && providedMethodTypes[i] == typeof(int))
                //    results.Add(true);

                if (paramsTypes[i] == providedMethodTypes[i])
                    results.Add(true);
            }

            return results.Count == paramsTypes.Length;
        }
    }
}
