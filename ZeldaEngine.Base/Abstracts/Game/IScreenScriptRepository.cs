using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IScreenScriptRepository
    {
        Dictionary<IScriptManager, GameScript> ScriptManagerForScript { get; }

        void AddScript(IScriptManager scriptManager, string scriptName);

        void Remove(string name);
    }
}