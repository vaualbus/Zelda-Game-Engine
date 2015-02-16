using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptRepository
    {
        GameScript AddScript(IGameView screen, string scriptName, CompiledScript scriptMetadata);

        GameScript Compile(RuntimeScript runtimeScript);

        void Remove(IGameView screen, string scriptName);

        GameScript GetScript(IGameView screen, string scriptName);

        IScriptManager GetScriptManager(IGameView screen, string scriptName);

        Dictionary<GameScript, IScriptManager> GetScripts(IGameView gameScreen);
    }
}