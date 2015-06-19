using Autofac;
using System.Linq;
using System.Reflection;
using ZeldaEngine.Base;
using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.Extensions;
using ZeldaEngine.Base.ValueObjects.Game.Attributes;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

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

            builder.RegisterType<ProjectManager>()
                   .As<IProjectManager>()
                   .InstancePerLifetimeScope();
        }

        #region Proxy Methods

        public override void Update(float dt)
        {
            foreach (var scriptGo in ScriptRepository.Scripts.Values.Where(t => t.ScriptManager.CurrentMenagedScript.GetType().GetCustomAttribute<ScriptDataClassAttribute>() == null))
            {
                var paramsForScript = ParamsProvider.GetParamatersForScript(scriptGo.ScriptManager.RuntimeScript.Name);
                var runMethod = scriptGo.ScriptManager.CurrentMenagedScript.GetType()
                    .GetMethods()
                    .FirstOrDefault(t => (t.Name == "Run" || t.Name == "RunScript") &&
                                         t.GetParameters().Length == paramsForScript.Length &&
                                         t.MatchTypes(paramsForScript));

                scriptGo.ScriptManager.CurrentMenagedScript.CurrentTime = dt;

                if (runMethod != null)
                {
                    try 
                    {
                        runMethod.Invoke(scriptGo.ScriptManager.CurrentMenagedScript, paramsForScript);
                    }
                    catch
                    {
                        return;
                    }
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