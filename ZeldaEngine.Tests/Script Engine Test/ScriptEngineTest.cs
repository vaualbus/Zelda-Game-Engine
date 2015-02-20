using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Tests
{
    [TestFixture]
    public class ScriptEngineTest
    {
        private readonly Config _testConfig = new Config(new  GameConfig("Test", baseDirectory: AppDomain.CurrentDomain.BaseDirectory));

        public class TestScreen : BaseGameView
        {
            public TestScreen(string name) 
                : base(name, new Vector2(0,0))
            {
            }
        }

        private static IGameView _testScreen = new TestScreen("Test");

        [Test]
        public void InstantiateEngineNotThrownAnException()
        {
            var engine = new ScriptEngine.GameScriptEngine(_testConfig);
            engine.InitializeEngine();

            engine.AddScript(_testScreen, "Test", engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"));

            const int expectedValue = Int32.MaxValue;
            engine.GetScript(_testScreen, "Test").ExcuteFunction("TestFunc1", new object[]{ expectedValue}).Should().Be(expectedValue);
        }

        [Test]
        public void GivenMultipleScriptExcutedRunFunctionWithGivenParams()
        {
            var engine = new ScriptEngine.GameScriptEngine(_testConfig);
            engine.InitializeEngine();

            var gameView = new TestScreen("Test");
            engine.AddScript(gameView, "testScript", @"Scripts\TestScript.cs").Compile();
            engine.AddScript(gameView, "engineScript", @"Scripts\TestEngineScript.cs").Compile();

            engine.ParamsProvider.AddParamater(gameView, "testScript", new object[] {10,  20 });
            engine.ParamsProvider.AddParamater(gameView, "engineScript", new object[] { "Abba" });

            var watch = new Stopwatch();
            watch.Start();
            engine.Update(gameView, 0.0f);
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
        }

        [Test]
        public void MultipleRunMethodPerformanceTest()
        {
            var engine = new ScriptEngine.GameScriptEngine(_testConfig);
            engine.InitializeEngine();

            var gameView = new TestScreen("Test");
            for (var i = 0; i < 10; i++)
            {
                engine.AddScript(gameView, string.Format("testScript_{0}", i), @"Scripts\TestScript.cs").Compile();
                engine.ParamsProvider.AddParamater(gameView, string.Format("testScript_{0}", i), new object[] {10, 20});
            }

            var watch = new Stopwatch();
            watch.Start();
            engine.Update(gameView, 0.0f);
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
        }
    }
}