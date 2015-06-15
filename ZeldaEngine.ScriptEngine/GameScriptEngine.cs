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
using ZeldaEngine.Base.Abstracts.ScriptEngine.Project;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Extensions;
using ZeldaEngine.Base.ValueObjects.Game.Attributes;
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

            builder.RegisterType<ProjectManager>()
                   .As<IProjectManager>()
                   .InstancePerLifetimeScope();
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

        public override void Update(float dt)
        {
            foreach (var scriptGo in ScriptRepository.Scripts.Values)
            {
                var paramsForScript = ParamsProvider.GetParamatersForScript(scriptGo.ScriptManager.RuntimeScript.Name);
                var runMethod = scriptGo.ScriptManager.CurrentMenagedScript.GetType()
                                        .GetMethods()
                                        .FirstOrDefault(t => (t.Name == "Run" || t.Name == "RunScript") && 
                                                             t.GetParameters().Length == paramsForScript.Length && t.MatchTypes(paramsForScript));

                var scriptDataFormAttributes = GetAttibutes<DataFromAttribute>(scriptGo.ScriptManager.CurrentMenagedScript.GetType());
                foreach (var scriptDataFormAttribute in scriptDataFormAttributes)
                {
                    var script2 = ScriptRepository.Scripts[scriptDataFormAttribute.Value.ScriptName]; //TryGetScriptGameObject(scriptDataFormAttribute.Value.ScriptName);
                    if (script2 != null && scriptGo.Name != script2.Name)
                    {
                        //Set the current script value to the correct script value
                        scriptGo.ScriptManager.SetScriptValue(scriptDataFormAttribute.Key, script2.ScriptManager.GetScriptValue(scriptDataFormAttribute.Value.FieldName));
                    }

                }

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

        private Dictionary<string, TAttribute> GetAttibutes<TAttribute>(Type classType) where TAttribute : Attribute
        {
            var returnAttributes = new Dictionary<string, TAttribute>();

            var resultPublicFields = from t in classType.GetFields()
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            var resultNonPublicFields = from t in classType.GetFields()
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            var resultPublicProps = from t in classType.GetProperties()
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            var resultNonPublicProps = from t in classType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            var resultPublicMembers = from t in classType.GetMembers()
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            var resultNonPublicMembers = from t in classType.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance)
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute) Attribute.GetCustomAttribute(t, typeof (TAttribute))
                };

            foreach (var el in resultPublicFields.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);

            foreach (var el in resultNonPublicFields.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);


            foreach (var el in resultPublicProps.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);


            foreach (var el in resultNonPublicProps.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);


            foreach (var el in resultPublicMembers.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);

            foreach (var el in resultNonPublicMembers.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);

            return returnAttributes;
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