using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.Abstracts
{
    [Guid("6D3790D0-7797-4FD3-81DD-B7EEFCDD73F5")]
    public interface IScriptParamaterProvider
    {
        [ComVisible(false)]
        bool AddParamater<TType>(IEnumerable<object> @params);

        [ComVisible(false)]
        void ChangeParamaters<TType>(IEnumerable<object> newParams);

        [ComVisible(false)]
        void RemoveParamaters<TType>();

        [ComVisible(false)]
        bool AddParamater(Type type, IEnumerable<object> @params);

        [ComVisible(false)]
        void ChangeParamaters(Type script, IEnumerable<object> newParams);

        [ComVisible(false)]
        void RemoveParamaters(Type scriptType);

        [ComVisible(true)]
        object[] GetParamatersForScript(string scriptName);
    }  
}
