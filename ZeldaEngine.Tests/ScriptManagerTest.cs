using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Tests.Abstracts;
using ZeldaEngine.Tests.TestRig;

namespace ZeldaEngine.Tests
{
    [TestFixture]
    public class ScriptManagerTest
    {
        private static readonly GameConfig TestConfig = new GameConfig("Test", baseDirectory: AppDomain.CurrentDomain.BaseDirectory);

        private static readonly TestGameView TestView = new TestGameView("Test View"); 

        internal class TestGameView : BaseGameView
        {
            public TestGameView(string screenName)
                : base(screenName, new Vector2(0,0))
            {
            }
        }

        [Test]
        public void CompileAnGetMethodNotThrowAnException()
        {
           // const int expctedValue = 12;

            var engine = new GameScriptEngine(TestConfig);
            engine.InitializeEngine();

            engine.AddScript(TestView, "Test", engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"));

            const float expectedValue = 24.0f;
            engine.GetScript(TestView, "Test").SetScriptValue("TestProp", expectedValue);
            engine.GetScript(TestView, "Test").GetScriptValue<float>("TestProp").Should().Be(expectedValue);
        }

        [Test]
        public void ProvideIntToACtorReturnCorrectValueFromFuncAndField()
        {
            var engine = new GameScriptEngine(TestConfig);
            engine.InitializeEngine();

            const int expctedValue = 10;

            engine.AddScript(TestView, "Script2", @"Scripts\Script2.cs")
                  .AddCtorParam(expctedValue)
                  .Compile();

            engine.GetScript(TestView, "Script2").GetScriptValue<int>("Data").Should().Be(expctedValue);
            engine.GetScript(TestView, "Script2").ExcuteFunction<int>("ScriptFunc1", null).Should().Be(expctedValue);
        }

        [Test]
        public void TestSystemInMultiThreadEnv()
        {
            var engine = new GameScriptEngine(TestConfig);
            engine.InitializeEngine();

            engine.AddScript(TestView, "Test", engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"));

            const int excptedValue = (int.MaxValue/2);

            var th1 = new Thread(() => engine.GetScript(TestView, "Test").ExcuteFunction("TestFunc1", new object[] { excptedValue }).Should().Be(excptedValue));
            var th2 = new Thread(() => engine.GetScript(TestView, "Test").ExcuteFunction("TestFunc1", new object[] { excptedValue }).Should().Be(excptedValue));

            th1.Start();
            th2.Start();
        }

        [Test]
        public void CanGenerateProjectFile()
        {
            var config = new GameConfig("Test", baseDirectory: @"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests");

            var engine = new ProjectGameScriptEngine(config);

            engine.InitializeEngine();
        }

        [Test]
        public void InvokeMethodWithLogNotThrowException()
        {
            var engine = new GameScriptEngine(TestConfig);
            engine.InitializeEngine();

            engine.AddScript(TestView, "STest", engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"));

            engine.GetScript(TestView, "STest").ExcuteFunction("TestLog", null);
        }

        [Test]
        public void NewScriptManagerApi()
        {
            var engine = new GameScriptEngine(TestConfig);
            engine.InitializeEngine();

            engine.AddScript(TestView, "Test", @"Scripts\Script2.cs")
                   .AddCtorParam<IDataProvider, DataProvider>(10)
                   .AddCtorParam("Abba")
                   .Compile();
        }
    }
}