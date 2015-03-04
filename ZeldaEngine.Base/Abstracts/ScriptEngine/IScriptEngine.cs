using System;
using System.Collections.Generic;
using Autofac;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptEngine : IDisposable
    {
#region Proxy Fields
        GameConfig GameConfig { get; }

        string CurrentScriptName { get; set; }

        GameScript[] CurrentLoadedScripts { get;  }

        void InitializeEngine();

#endregion

        IGameEngine GameEngine { get; }

        IScriptRepository ScriptRepository { get; }

        IScriptParamaterProvider ParamsProvider { get; }
            
        IScriptCompiler ScriptCompiler { get; }

        ILifetimeScope ScriptSystemLifetimeScope { get; }

        ContainerBuilder ContainerBuilder { get;  }

        ILogger Logger { get; }

        bool GenerateProject();

        void UpdateProject();

        void DeleteProject();

        void RegisterComponents(ContainerBuilder builder);

#region Proxy Methods

        void Update(IGameView view, float dt);

        IEnumerable<GameScript> GetScripts();
        
        IScriptManager GetScript(IGameView gameScreen, string name);

        Type GetScriptType(IGameView gameScreen, string scriptName);

        object GetScriptValue(IGameView gameScreen, string scriptName, string scriptFieldName);

        void AddScriptParams(IGameView gameScreen, string name, object[] @params);

        RuntimeScript AddScript(IGameView screen, string scriptName, string fileName);

        GameScript AddScript(IGameView screen, string scriptName, CompiledScript compiledScript);

        void SetScriptInitialLocation(IGameView gameScreen, string scriptName, Vector2 pos);

        #endregion
    }
}
