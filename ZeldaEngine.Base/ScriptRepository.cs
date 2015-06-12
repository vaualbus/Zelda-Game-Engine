using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    internal class ScriptRepository : IScriptRepository
    {

        private readonly IScriptEngine _engine;
        private readonly IScriptCompiler _compiler;
        private readonly IDependencyResolver _resolver;
        private readonly IScriptActivator _scriptActivator;
        private readonly ILogger _logger;

        public Dictionary<string, ScriptableGameObject> Scripts { get; private set; }

        public ScriptRepository(IScriptEngine engine,
                                IScriptCompiler compiler,
                                IDependencyResolver resolver, 
                                IScriptActivator scriptActivator,
                                ILogger logger)
        {
            Scripts = new Dictionary<string, ScriptableGameObject>();

            _engine = engine;
            _compiler = compiler;
            _resolver = resolver;
            _scriptActivator = scriptActivator;
            _logger = logger;
        }

        public GameScript AddScript(ScriptableGameObject scriptableGo, string name, CompiledScript scriptMetadata)
        {
            var scriptManager = new ScriptManager(_engine, this, _resolver, _scriptActivator, _logger);
            var compiledScript = scriptManager.AddScript(scriptableGo, scriptMetadata, name);
            scriptableGo.ScriptManager = scriptManager;

            if (Scripts.ContainsKey(name))
            {
                
            }
            else
                Scripts.Add(name, scriptableGo);

            return (GameScript) scriptableGo.ScriptManager.CurrentMenagedScript;
        }

        public GameScript Compile(RuntimeScript runtimeScript)
        {
            ScriptableGameObject outScriptManager;
            if (Scripts.TryGetValue(runtimeScript.ScriptableGo.Name, out outScriptManager))
            {
                _logger.LogError("A Script with the same name {0} has arleady addd in screen {1}");
                return null;
            }

            var scriptManager = new ScriptManager(_engine, this, _resolver, _scriptActivator, _logger);
            scriptManager.CreateScript(runtimeScript);

            var scriptableGo = runtimeScript.ScriptableGo;
            scriptableGo.ScriptManager = scriptManager;
            if (scriptableGo != null)
            {
                //if (screen.CurrentScriptStates.ContainsKey(scriptManager.CurrentMenagedScript))
                //{
                //    _logger.LogError("The script {0} is added more than once!");
                //    return null;
                //}

                //screen.CurrentScriptStates.Add(scriptManager.CurrentMenagedScript, ScriptState.NotSet);

                lock (Scripts)
                {
                    Scripts.Add(runtimeScript.Name, scriptableGo);
                }

                return scriptManager.CurrentMenagedScript;
            }

            lock (Scripts)
            {
                Scripts.Add(runtimeScript.Name, scriptableGo);
            }

            return scriptManager.CurrentMenagedScript;
        }

        public void Remove(string scriptName)
        {
            Scripts.Remove(scriptName);
        }

        public GameScript GetScript(string scriptName)
        {
            return Scripts[scriptName].ScriptManager.CurrentMenagedScript;
        }

        public IScriptManager GetScriptManager(string scriptName)
        {
            return Scripts[scriptName].ScriptManager;
        }

        public ScriptableGameObject TryGetScriptGameObject(string scriptName)
        {
            ScriptableGameObject sGo = null;
            if (!Scripts.TryGetValue(scriptName, out sGo))
                return null;

            return sGo;
        }
    }
}