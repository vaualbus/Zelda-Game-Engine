using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Extensions;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.ScriptEngine
{
    public class GameScriptEngine : BaseScriptEngine
    {
        #region Proxy Fields

        public GameScriptEngine(IGameEngine gameEngine)
            : base(gameEngine)
        {
        }


        #endregion

        protected override void OnInitializeEngine()
        {
            Logger.LogInfo("Init the engine Component");
        }



        public override void RegisterComponents(ContainerBuilder builder)
        {
            builder.Register(context => this)
                .As<IScriptEngine>()
                .InstancePerLifetimeScope();

            builder.Register(context => new GameLogger(GameEngine.GameConfig))
               .As<ILogger>()
               .InstancePerLifetimeScope();

            builder.RegisterType<GameScriptParmaterProvider>()
                .As<IScriptParamaterProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoslynCSharpScriptCompiler>()
                   .As<IScriptCompiler>()
                   .SingleInstance();

        }

        #region Proxy Methods

        public override void Update(IGameView screen, float dt)
        {
            if(screen == null)
                return;

            foreach (var script in screen.Scripts.Select(t => t.ScriptManager))
            {
                var paramsForScript = ParamsProvider.GetParamatersForScript(script.RuntimeScript.Name);
                var runMethod = script.CurrentMenagedScript.GetType().GetMethods()
                    .FirstOrDefault(t => (t.Name == "Run" || t.Name == "RunScript")  && t.GetParameters().Length == paramsForScript.Length && t.MatchTypes(paramsForScript));

                if (runMethod != null)
                {
//#if DEBUG
//                    Logger.LogWarning("------------------------------------");
//                    Logger.LogWarning("Calling method {0} with @params: {1}", runMethod.Name, string.Join(", ", paramsForScript));
                    
//#endif
                    runMethod.Invoke(script.CurrentMenagedScript, paramsForScript);

//#if DEBUG
//                    Logger.LogWarning("------------------------------------");
//#endif

                }
                else
                    Logger.LogWarning("Try calling Method Run with mismatched arguments.");
            }
        }

        #endregion

        public override ScriptableGameObject AddScript(GameObject parentGo, Dictionary<string, string> scriptFiles, QuestLoaderScriptType scriptType)
        {
            ScriptableGameObject scriptableGo = null;

            GameEngine.GameObjectFactory.TryGet(scriptType.Script.Name, out scriptableGo);

            if (scriptableGo == null)
            {
                scriptableGo = GameEngine.GameObjectFactory.Create<ScriptableGameObject>(scriptType.Script.Name, t =>
                {
                    t.ObjectType = Base.ValueObjects.Game.ObjectType.Script;
                    t.ScriptType = scriptType.ScriptType;
                });
            }

            scriptableGo.ScriptType = scriptType.ScriptType;
            scriptableGo.GameObject = parentGo;

            var compiledScript = ScriptCompiler.Compile(scriptFiles[scriptType.Script.Name]);
            ScriptRepository.AddScript(scriptableGo, scriptType.Script.Name, compiledScript);

            return scriptableGo;
        }
    }
}