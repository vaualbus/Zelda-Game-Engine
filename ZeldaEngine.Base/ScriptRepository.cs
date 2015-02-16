using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    internal class ScriptRepository : IScriptRepository
    {
        internal class GameViewInfo
        {
            public IGameView GameView { get; private set; }

            public string ScriptName { get; private set; }

            public GameViewInfo(IGameView gameView, string scriptName)
            {
                GameView = gameView;
                ScriptName = scriptName;
            }
        }

        private readonly IScriptEngine _engine;
        private readonly IScriptCompiler _compiler;
        private readonly IDependencyResolver _resolver;
        private readonly IScriptActivator _scriptActivator;
        private readonly ILogger _logger;

        public Dictionary<GameViewInfo, IScriptManager> Scripts { get; private set; }

        public ScriptRepository(IScriptEngine engine,
                                IScriptCompiler compiler,
                                IDependencyResolver resolver, 
                                IScriptActivator scriptActivator,
                                ILogger logger)
        {
            Scripts = new Dictionary<GameViewInfo, IScriptManager>();

            _engine = engine;
            _compiler = compiler;
            _resolver = resolver;
            _scriptActivator = scriptActivator;
            _logger = logger;
        }

        public GameScript AddScript(IGameView screen, string scriptName, CompiledScript scriptMetadata)
        {
            IScriptManager outScriptManager;
            if (Scripts.TryGetValue(new GameViewInfo(screen, scriptName), out outScriptManager))
            {
                _logger.LogError("A Script with the same name {0} has arleady addd in screen {1}");
                return null;
            }

            var scriptManager = new ScriptManager(_engine, _compiler, this, _resolver,  _scriptActivator, _logger);
            var result = scriptManager.AddScript(screen, scriptMetadata, scriptName);


            if (screen != null)
            {
                //if (screen.CurrentScriptStates.ContainsKey(result))
                //{
                //    _logger.LogError("The script {0} is added more than once!");
                //    return null;
                //}

               // screen.CurrentScriptStates.Add(scriptManager.CurrentMenagedScript, ScriptState.NotSet);

                lock (Scripts)
                {
                    Scripts.Add(new GameViewInfo(screen, scriptName), scriptManager);
                }

                return result;
            }

            lock (Scripts)
            {
                Scripts.Add(new GameViewInfo(screen, scriptName), scriptManager);
            }

            return result;
        }

        public GameScript Compile(RuntimeScript runtimeScript)
        {
            IScriptManager outScriptManager;
            if (Scripts.TryGetValue(new GameViewInfo(runtimeScript.GameView, runtimeScript.Name), out outScriptManager))
            {
                _logger.LogError("A Script with the same name {0} has arleady addd in screen {1}");
                return null;
            }

            var scriptManager = new ScriptManager(_engine, _compiler, this, _resolver, _scriptActivator, _logger);
            scriptManager.CreateScript(runtimeScript);

            var screen = runtimeScript.GameView;
            if (screen != null)
            {
                //if (screen.CurrentScriptStates.ContainsKey(scriptManager.CurrentMenagedScript))
                //{
                //    _logger.LogError("The script {0} is added more than once!");
                //    return null;
                //}

                //screen.CurrentScriptStates.Add(scriptManager.CurrentMenagedScript, ScriptState.NotSet);

                lock (Scripts)
                {
                    Scripts.Add(new GameViewInfo(screen, runtimeScript.Name), scriptManager);
                }

                return scriptManager.CurrentMenagedScript;
            }

            lock (Scripts)
            {
                Scripts.Add(new GameViewInfo(runtimeScript.GameView, runtimeScript.Name), scriptManager);
            }

            return scriptManager.CurrentMenagedScript;
        }

        public void Remove(IGameView screen, string scriptName)
        {
            if (screen == null)
            {
                _logger.LogError("The give screen is invalid");
                return;
            }

            Scripts.Remove(new GameViewInfo(screen, scriptName));
        }

        public GameScript GetScript(IGameView screen, string scriptName)
        {
            return Scripts.FirstOrDefault(t => t.Key.GameView.Equals(screen) && t.Key.ScriptName == scriptName).Value.CurrentMenagedScript;
        }

        public IScriptManager GetScriptManager(IGameView screen, string scriptName)
        {
            if (screen == null)
            {
                _logger.LogError("The give screen is invalid");
                return null;
            }

            return Scripts.Where(t => t.Key.GameView.Equals(screen) && t.Key.ScriptName == scriptName).Select(t => t.Value).FirstOrDefault();
        }

        public Dictionary<GameScript, IScriptManager> GetScripts(IGameView gameScreen)
        {
            if (gameScreen == null)
            {
                _logger.LogError("The give screen is invalid");
                return null;
            }

            var screen = Scripts.Where(t => t.Key.GameView == gameScreen);
            return screen.ToDictionary(t => t.Value.CurrentMenagedScript, t => t.Value);
        }
    }
}