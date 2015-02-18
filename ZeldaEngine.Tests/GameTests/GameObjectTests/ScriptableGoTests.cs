using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.SharpDx;
using ZeldaEngine.Tests.TestRig;
using GameScriptEngine = ZeldaEngine.ScriptEngine.GameScriptEngine;

namespace ZeldaEngine.Tests.GameTests.GameObjectTests
{
    [TestFixture]
    public class ScriptableGoTests
    {
        

        private readonly IScriptParamaterProvider _scriptParamsProvider;

        private readonly GameScriptEngine _engine;

        public ScriptableGoTests()
        {
            //ConfigurationManager.CreateConfiguration(new GameConfig("Zelda Engine", 300, 200, AppDomain.CurrentDomain.BaseDirectory));
            //_testConfig = new Config(new GameScriptConfig(@"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
            //                                              @"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
            //                                              "TestProject"), 
            //                                              ConfigurationManager.GetConfiguration());

            GameConfig gameConfig;
            if (File.Exists(ConfigurationManager.ConfigurationFileName))
            {
                if (!ConfigurationManager.CheckEngineDirectory())
                    ConfigurationManager.UpdateEngineDirectory();

                gameConfig = ConfigurationManager.GetConfiguration();
            }
            else
            {
                //We need to create the config file than save the configuration.
                gameConfig = new GameConfig("Zelda Engine", 300, 200, AppDomain.CurrentDomain.BaseDirectory);
                ConfigurationManager.CreateConfiguration(gameConfig);
            }

            var scriptConfig = new GameScriptConfig(@"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
                                                          @"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
                                                          "TestProject");

            var config = new Config(scriptConfig, gameConfig);


            var gameEngine = new SharpDxCoreEngine(new TestGame(), config, new GameLogger(config));
            _engine = new ScriptEngine.GameScriptEngine(config, gameEngine);
                
            _engine.InitializeEngine();

            _engine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));

            new GameObjectFactory(new TestGameEngine());
        }

        [Test]
        public void AttachScriptToAGameObjectNotThrowException()
        {
            var go = GameObjectFactory.Create<ScriptableGameObject>("Test", scriptGameObject =>
            {
                scriptGameObject.ScriptParamProvider = _engine.ParamsProvider;
                scriptGameObject.ObjectType = ObjectType.Enemy;

                var script1 = CreateScript("Script1", @"Scripts\GameObjectScriptTest.cs");
                var script2 = CreateScript("Script2", @"Scripts\GameObjectScriptTest.cs");
                var script3 = CreateScript("Script3", @"Scripts\GameObjectScriptTest.cs");

                scriptGameObject.Scripts.Add(script1);
                scriptGameObject.Scripts.Add(script2);
                scriptGameObject.Scripts.Add(script3);

                ((GameScriptParmaterProvider) scriptGameObject.ScriptParamProvider).AddParamater(script1.CurrentMenagedScript, new object[] { 10 });
                ((GameScriptParmaterProvider) scriptGameObject.ScriptParamProvider).AddParamater(script2.CurrentMenagedScript, new object[] { "Test" });
                ((GameScriptParmaterProvider) scriptGameObject.ScriptParamProvider).AddParamater(script1.CurrentMenagedScript, null);
            });

            go.Update(0);
        }

        private ScriptManager CreateScript(string scriptName, string scriptFilename)
        {
            //var runtimeScript = new RuntimeScript(null, _engine.ScriptCompiler, _engine.ScriptRepository, "Test",
            //   @"Scripts\GameObjectScriptTest.cs");

            //runtimeScript.Compile();
            var script = _engine.ScriptCompiler.Compile(scriptFilename);

            var scriptManager = new ScriptManager(_engine, _engine.ScriptCompiler, _engine.ScriptRepository, null,
                new InternalScriptActivator(), _engine.Logger);

            scriptManager.AddScript(null, script, scriptName);

            return scriptManager;
        }
    }
}