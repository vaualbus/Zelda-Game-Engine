using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Abstracts;


namespace ZeldaEngine.ScriptEngine
{
    public class ScriptParmaterProvider : IScriptParamaterProvider
    {
        private readonly Dictionary<string, IEnumerable<object>> _paramaters;
        private readonly IScriptEngine _engine;

        public ScriptParmaterProvider(IScriptEngine engine)
        {
            engine.Report("Init script Paramater", ReportReason.Log);

            _paramaters = new Dictionary<string, IEnumerable<object>>();
            _engine = engine;
        }

        public bool AddParamater<TType>(IEnumerable<object> @params)
        {
            return AddParamater(typeof(TType), @params);
        }

        public object[] GetParamatersForScript(string scriptName)
        {
            IEnumerable<object> @params;
            if (!_paramaters.TryGetValue(scriptName, out @params))
                throw new InvalidOperationException(string.Format("Cannot get the params for script: {0}", scriptName));

            return _paramaters[scriptName].ToArray();
        }

        public void ChangeParamaters<TType>(IEnumerable<object> newParams)
        {
            ChangeParamaters(typeof(TType), newParams);
        }

        public void RemoveParamaters<TType>()
        {
            RemoveParamaters(typeof(TType));
        }

        public void RemoveParamaters(Type type)
        {
            //Tell the engine we change the script
            _engine.Report(type, string.Format("Remove params {0} from {1}", string.Join(", ", _paramaters[type.Name].ToArray()), type.Name),
                                  ReportReason.Log);

            _paramaters.Remove(type.Name);
        }


        public void ChangeParamaters(Type script, IEnumerable<object> newParams)
        {
            lock (_paramaters)
            {
                if (_paramaters.ContainsKey(script.Name))
                {
                    //Tell the engine we change the script
                    _engine.Report(script, string.Format("Changing params from {0} to {1}", string.Join(", ", _paramaters[script.Name].ToArray()), string.Join(", ", newParams.ToArray())),
                                          ReportReason.Log);

                    RemoveParamaters(script);

                }

                _paramaters[script.Name] = newParams;
            }
        }

        public bool AddParamater(Type type, IEnumerable<object> @params)
        {
            if (_paramaters.ContainsKey(type.Name))
                return false;

            if (@params == null)
            {
                _paramaters.Add(type.Name, new object[] { });
                //Tell the engine we change the script
                _engine.Report(type, string.Format("Adding null value to {0}", type.Name),
                                      ReportReason.Log);
                return false;
            }

            _paramaters.Add(type.Name, @params);

            //Tell the engine we change the script
            _engine.Report(type, string.Format("Adding params {0} to {1}",
                                                string.Join(", ", _paramaters[type.Name].ToArray()),
                                                type.Name),
                                  ReportReason.Log);

            return true;
        }
    }
}