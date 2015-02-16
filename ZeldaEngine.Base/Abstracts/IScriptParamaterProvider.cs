using System;
using System.Collections.Generic;

namespace ZeldaEngine.Base.Abstracts
{
    public interface IScriptParamaterProvider
    {
        bool AddParamater<TType>(IEnumerable<object> @params);

        void ChangeParamaters<TType>(IEnumerable<object> newParams);

        void RemoveParamaters<TType>();

        bool AddParamater(Type type, IEnumerable<object> @params);

        void ChangeParamaters(Type script, IEnumerable<object> newParams);

        void RemoveParamaters(Type scriptType);

        object[] GetParamatersForScript(string scriptName);
    }  
}
