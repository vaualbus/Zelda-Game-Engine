using System;
using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptParamaterProvider
    {
        bool AddParamater(string scriptName, IEnumerable<object> @params);

        void ChangeParamaters(string scriptName, IEnumerable<object> newParams);

        void RemoveParamaters(string scriptName);

        object[] GetParamatersForScript(string scriptName);

        object[] GetParamatersForScript(GameScript gameScript);
    }  
}
