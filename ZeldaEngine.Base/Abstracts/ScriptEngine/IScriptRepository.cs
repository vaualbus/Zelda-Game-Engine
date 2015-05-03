using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptRepository
    {
        GameScript AddScript(ScriptableGameObject scriptableGo, string name, CompiledScript scriptMetadata);

        GameScript Compile(RuntimeScript runtimeScript);

        void Remove(string scriptName);

        GameScript GetScript(string scriptName);

        IScriptManager GetScriptManager(string scriptName);
    }
}