using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ZeldaEngine.ScriptEngine.Abstracts;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.ScriptEngine
{
    [ComVisible(false)]
    public class ScriptManager : IScriptManager
    {
        private readonly Dictionary<string, CompiledScript> _scripts;
        private readonly IScriptEngine _engine;
        private readonly IScriptParamaterProvider _paramsProvider;

        private volatile Dictionary<Type, ZeldaScript> _scriptCachedInstances;

        public ScriptManager(IScriptEngine engine, IScriptParamaterProvider provider)
        {
            _scripts = new Dictionary<string, CompiledScript>();
            _paramsProvider = provider;
            _engine = engine;
            _scriptCachedInstances = new Dictionary<Type, ZeldaScript>();
        }

        public bool AddScript(CompiledScript script, string scriptName)
        {
            try
            {
                CompiledScript tempsScript;
                if (_scripts.TryGetValue(scriptName, out tempsScript))
                {
                    _engine.Report<ScriptManager>("Cannot the same script twice", ReportReason.Error);
                    return false;
                }

                _scripts.Add(scriptName, script);

                if (_scriptCachedInstances.ContainsKey(script.ScriptType))
                    return false;

                var objValue = Activator.CreateInstance(script.ScriptType) as ZeldaScript;
                if (objValue == null)
                    return false;

                _scriptCachedInstances.Add(script.ScriptType, objValue);

                var scriptDesc = new ScriptInfo(scriptName, script.CompiledPath);

                objValue.GetType().GetField("_engine", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(objValue, _engine);

                objValue.GetType().GetField("_scriptInfo", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(objValue, scriptDesc);

                //Set script engine to the current engine
                SetScriptFields(scriptName, "_engine", _engine);
                SetScriptFields(scriptName, "_scriptInfo", scriptDesc);

                return true;
            }
            catch (Exception ex)
            {
                _engine.Report(string.Format("Error during the adding of the script: {0}. Info: ({1}, {2}) ", scriptName, ex.Message, ex.StackTrace), ReportReason.Error);
                return false;
            }
        }

        public void RemoveScript(string name)
        {
            if (FindAndCheck(name) == null)
                return;

            _scripts.Remove(name);
        }

        public CompiledScript Find(string name)
        {
            return _scripts.ContainsKey(name) ? _scripts[name] : null;
        }

        public object ExcuteFunction(string scriptName, string funcName, object[] @params)
        {
            var script = FindAndCheck(scriptName);
            if (script == null)
                return null;

            return FindMethod(scriptName, funcName, @params).Invoke(_scriptCachedInstances[script.ScriptType], @params);
        }

        public MethodInfo GetMethod(string scriptName, string funcName, object[] @params)
        {
            return FindMethod(scriptName, funcName, @params);
        }

        public object GetScriptValue(string scriptName, string propName)
        {
            var script = FindAndCheck(scriptName);
            if (script == null)
                return null;

            try
            {
                var prop = script.Properties.FirstOrDefault(t => t.Name == propName);

                if (prop == null)
                {
                    _engine.Report<ScriptManager>("Cannot find a properties with the given Name.", ReportReason.Error);
                    return null;
                }

                return prop.GetValue(_scriptCachedInstances[script.ScriptType]);
            }
            catch (Exception)
            {
                _engine.Report<ScriptManager>("Cannot create and instance of the script. Script cannot have a constructor that have paramaters", ReportReason.Error);
                return null;
            }
        }

        public void SetScriptValue(string scriptName, string propName, object value)
        {
            var script = FindAndCheck(scriptName);
            if (script == null)
                return;

            try
            {
                var prop = script.Properties.FirstOrDefault(t => t.Name == propName);

                if (prop == null)
                {
                    _engine.Report<ScriptManager>("Cannot find a properties with the given Name.", ReportReason.Error);
                    return;
                }

                prop.SetValue(_scriptCachedInstances[script.ScriptType], value);
            }
            catch (Exception)
            {
                _engine.Report<ScriptManager>("Cannot create and instance of the script. Script cannot have a constructor that have paramaters", ReportReason.Error);
            }
        }

        public void SetScriptFields(string scriptName, string fieldName, object scriptEngine)
        {
            var script = FindAndCheck(scriptName);
            if (script == null)
                return;

            try
            {
                var prop = script.Fields.FirstOrDefault(t => t.Name == fieldName);

                if (prop == null)
                {
                    _engine.Report<ScriptManager>("Cannot find a properties with the given Name.", ReportReason.Error);
                    return;
                }

                prop.SetValue(_scriptCachedInstances[script.ScriptType], fieldName);
            }
            catch (Exception ex)
            {
                _engine.Report<ScriptManager>(string.Format("Cannot create and instance of the script. Script cannot have a constructor that have paramaters. Exception: {0}", ex.Message), ReportReason.Error);
            }
        }


        public TReturn ExcuteFunction<TReturn>(string scriptName, string funcName, object[] @params)
        {
            return (TReturn) ExcuteFunction(scriptName, funcName, @params);
        }

        public TValue GetScriptValue<TValue>(string scriptName, string propName)
        {
            return (TValue) GetScriptValue(scriptName, propName);
        }

        private CompiledScript FindAndCheck(string name)
        {
            lock (_scripts)
            {
                var script = Find(name);
                if (script == null)
                {
                    _engine.Report<ScriptManager>("Cannot delete the script, script not found", ReportReason.Error);
                    return null;
                }

                return script;
            }
        }

        private MethodInfo FindMethod(string scriptName, string funcName, object[] @params)
        {
            var script = FindAndCheck(scriptName);
            if (script == null)
                return null;

            try
            {
                var method = script.Methods.FirstOrDefault(t => t.Name == funcName && t.GetParameters().Length == @params.Length);

                if (method == null)
                {
                    _engine.Report<ScriptManager>("Cannot find a method with the given Name and the given paramaters", ReportReason.Error);
                    return null;
                }

                var methodParamsTypes = method.GetParameters().Select(t => t.ParameterType);
                var valuesTypes = @params.Select(t => t.GetType());
                var differences = methodParamsTypes.Except(valuesTypes).Count();

                if (differences > 0)
                {
                    _engine.Report<ScriptManager>("The given paramaters don't natch the method signature", ReportReason.Error);
                    return null;
                }

                return method;
            }
            catch (Exception ex)
            {
                _engine.Report<ScriptManager>("Cannot create and instance of the script. Script cannot have a constructor that have paramaters", ReportReason.Error);
                _engine.Report<ScriptManager>(ex.Message, ReportReason.Error);
                return null;
            }
        }
    }
}
