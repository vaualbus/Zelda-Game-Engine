using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Tests.TestRig;

namespace ZeldaEngine.Tests.Script_Engine_Test
{
    [TestFixture]
    public class ScriptEngineTest
    {
        private readonly GameConfig _testConfig = new  GameConfig("Test", baseDirectory: AppDomain.CurrentDomain.BaseDirectory, scriptDirectory: "Scripts", isInTest: true);

        private readonly ScriptableGameObject _scripatbleGameObject;

        public class TestScreen : BaseGameView
        {
            public TestScreen(string name) 
                : base(name, new Vector2(0,0))
            {
            }
        }

        private static IGameView _testScreen = new TestScreen("Test");

        public ScriptEngineTest()
        {
            var engine = new ScriptEngine.GameScriptEngine(new TestGameEngine(_testConfig, new TestLogger()));
            _scripatbleGameObject = engine.GameEngine.GameObjectFactory.CreateEmpty<ScriptableGameObject>("emptyGo");
        }

        [Test]
        public void InstantiateEngineNotThrownAnException()
        {
            var engine = new ScriptEngine.GameScriptEngine(new TestGameEngine(_testConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.AddScript(_scripatbleGameObject, "Test", engine.ScriptCompiler.Compile(@"TestScript.cs"));

            const int expectedValue = Int32.MaxValue;
            engine.GetScript("Test").ExcuteFunction("TestFunc1", new object[]{ expectedValue}).Should().Be(expectedValue);
        }

        [Test]
        public void GivenMultipleScriptExcutedRunFunctionWithGivenParams()
        {
            var engine = new ScriptEngine.GameScriptEngine(new TestGameEngine(_testConfig, new TestLogger()));
            engine.InitializeEngine();

            var gameView = new TestScreen("Test");
            engine.AddScript(_scripatbleGameObject, "testScript", @"TestScript.cs").Compile();
            engine.AddScript(_scripatbleGameObject, "engineScript", @"TestEngineScript.cs").Compile();

            engine.ParamsProvider.AddParamater("testScript", new object[] {10,  20 });
            engine.ParamsProvider.AddParamater("engineScript", new object[] { "Abba" });

            var watch = new Stopwatch();
            watch.Start();
            engine.Update(gameView, 0.0f);
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
        }

        [Test]
        public void MultipleRunMethodPerformanceTest()
        {
            var engine = new ScriptEngine.GameScriptEngine(new TestGameEngine(_testConfig, new TestLogger()));
            engine.InitializeEngine();

            var gameView = new TestScreen("Test");
            for (var i = 0; i < 10; i++)
            {
                engine.AddScript(_scripatbleGameObject, string.Format("testScript_{0}", i), @"TestScript.cs").Compile();
                engine.ParamsProvider.AddParamater(string.Format("testScript_{0}", i), new object[] {10, 20});
            }

            var watch = new Stopwatch();
            watch.Start();
            engine.Update(gameView, 0.0f);
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
        }
    }
}