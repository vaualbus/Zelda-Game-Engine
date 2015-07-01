using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.SharpDxImp;
using ZeldaEngine.Tests.TestRig;
using GameScriptEngine = ZeldaEngine.ScriptEngine.GameScriptEngine;


namespace ZeldaEngine.Tests.GameTests.GameObjectTests
{


    [TestFixture]
    public class ScriptableGoTests
    {

        private readonly GameScriptEngine _engine;

        public ScriptableGoTests()
        {
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
                gameConfig = new GameConfig("Zelda Engine", 300, 200, AppDomain.CurrentDomain.BaseDirectory, scriptDirectory: "Scripts", isInTest: true);
                ConfigurationManager.CreateConfiguration(gameConfig);
            }


            var gameEngine = new SharpDxCoreEngine(new TestGame(), gameConfig, new GameLogger(gameConfig));
            _engine = new GameScriptEngine(gameEngine);
                
            _engine.InitializeEngine();

            _engine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));

            _engine.GameEngine.GameObjectFactory = new GameObjectFactory(_engine.GameEngine);

            gameEngine.ScriptEngine = _engine;
        }

        [Test]
        public void AttachScriptToAGameObjectNotThrowException()
        {
            var go = _engine.AddScript("scriptGo", "GameObjectScriptTest.cs", new object[] {10});
            go.Update(0);
        }

        private ScriptManager CreateScript(string scriptName, string scriptFilename)
        {
            //var runtimeScript = new RuntimeScript(null, _engine.ScriptCompiler, _engine.ScriptRepository, "Test",
            //   @"Scripts\GameObjectScriptTest.cs");

            //runtimeScript.Compile();
            var script = _engine.ScriptCompiler.Compile(scriptFilename);

            var scriptManager = new ScriptManager(_engine, _engine.ScriptRepository, null,
                new InternalScriptActivator(), _engine.Logger);

            scriptManager.AddScript(null, script, scriptName);

            return scriptManager;
        }
    }
}