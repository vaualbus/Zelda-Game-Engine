using System;
using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptParamaterProvider
    {
        bool AddParamater(IGameView screen, string scriptName, IEnumerable<object> @params);

        void ChangeParamaters(IGameView screen, string scriptName, IEnumerable<object> newParams);

        void RemoveParamaters(IGameView screen, string scriptName);

        object[] GetParamatersForScript(IGameView screen, string scriptName);
    }  
}
