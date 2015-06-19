using System;
using System.Collections.Generic;
using Autofac;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Abstracts.ScriptEngine
{
    public interface IScriptEngine : IDisposable
    {
        #region Proxy Fields

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

        IProjectManager ProjectManager { get; }

        void RegisterComponents(ContainerBuilder builder);

#region Proxy Methods

        void Update(float dt);

        IEnumerable<GameScript> GetScripts();
        
        IScriptManager GetScript(string name);

        RuntimeScript AddScript(ScriptableGameObject go, string scriptName, string fileName);

        GameScript AddScript(ScriptableGameObject go, string scriptName, CompiledScript compiledScript);

        ScriptableGameObject AddScript(GameObject parentGo, Dictionary<string, string> scriptFiles, QuestLoaderScriptType scriptType);

        ScriptableGameObject AddScript(string goName, string fileName, object[] @params = null);

        #endregion

        void PerformScriptBinding();
    }
}
