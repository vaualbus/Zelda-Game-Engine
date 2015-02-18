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
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Extensions;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.ScriptEngine
{
    public class GameScriptEngine : BaseScriptEngine
    {
        #region Proxy Fields

        public GameScriptEngine(Config config, IGameEngine gameEngine = null)
            : base(config, gameEngine)
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

            builder.Register(context => new GameLogger(Config))
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

            foreach (var valuePair in ScriptRepository.GetScripts(screen).Where(t => t.Key != null && t.Value != null))
            {
                var script = valuePair.Value;

                var paramsForScript = ParamsProvider.GetParamatersForScript(screen, script.RuntimeScript.Name);
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
    }
}