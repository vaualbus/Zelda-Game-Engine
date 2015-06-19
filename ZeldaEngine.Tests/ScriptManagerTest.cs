using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Tests.Abstracts;
using ZeldaEngine.Tests.TestRig;

namespace ZeldaEngine.Tests
{
    [TestFixture]
    public class ScriptManagerTest
    {
        private static readonly GameConfig TestConfig = new GameConfig("Test", baseDirectory: AppDomain.CurrentDomain.BaseDirectory, scriptDirectory: "Scripts", isInTest: true);

        private readonly ScriptableGameObject _scriptableGameObject;

        public ScriptManagerTest()
        {
            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            _scriptableGameObject = engine.GameEngine.GameObjectFactory.CreateEmpty<ScriptableGameObject>("scriptGo");
        }

        [Test]
        public void CompileAnGetMethodNotThrowAnException()
        {
           // const int expctedValue = 12;

            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.AddScript(_scriptableGameObject, "Test", engine.ScriptCompiler.Compile(@"TestScript.cs"));

            const float expectedValue = 24.0f;
            engine.GetScript("Test").SetScriptValue("TestProp", expectedValue);
            engine.GetScript("Test").GetScriptValue<float>("TestProp").Should().Be(expectedValue);
        }

        [Test]
        public void ProvideIntToACtorReturnCorrectValueFromFuncAndField()
        {
            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            const int expctedValue = 10;

            engine.AddScript(_scriptableGameObject, "Script2", @"Script2.cs")
                  .AddCtorParam(expctedValue)
                  .Compile();

            engine.GetScript("Script2").GetScriptValue<int>("Data").Should().Be(expctedValue);
            engine.GetScript("Script2").ExcuteFunction<int>("ScriptFunc1", null).Should().Be(expctedValue);
        }

        [Test]
        public void TestSystemInMultiThreadEnv()
        {
            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.AddScript(_scriptableGameObject, "Test", engine.ScriptCompiler.Compile(@"TestScript.cs"));

            const int excptedValue = (int.MaxValue/2);

            var th1 = new Thread(() => engine.GetScript("Test").ExcuteFunction("TestFunc1", new object[] { excptedValue }).Should().Be(excptedValue));
            var th2 = new Thread(() => engine.GetScript("Test").ExcuteFunction("TestFunc1", new object[] { excptedValue }).Should().Be(excptedValue));

            th1.Start();
            th2.Start();
        }

        [Test]
        public void CanGenerateProjectFile()
        {
            var config = new GameConfig("Test", baseDirectory: @"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests", scriptDirectory: "Scripts");
            var engine = new ProjectGameScriptEngine(new TestGameEngine(config, new TestLogger()));

            engine.InitializeEngine();
        }

        [Test]
        public void InvokeMethodWithLogNotThrowException()
        {
            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.AddScript(_scriptableGameObject, "STest", engine.ScriptCompiler.Compile(@"TestScript.cs"));

            engine.GetScript("STest").ExcuteFunction("TestLog", null);
        }

        [Test]
        public void NewScriptManagerApi()
        {
            var engine = new GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.AddScript(_scriptableGameObject, "Test", @"Script2.cs")
                   .AddCtorParam<IDataProvider, DataProvider>(10)
                   .AddCtorParam("Abba")
                   .Compile();
        }

        [Test]
        public void AttachPropertyToGameObject()
        {
            var engine = new  ScriptEngine.GameScriptEngine(new TestGameEngine(TestConfig, new TestLogger()));
            engine.InitializeEngine();

            engine.ScriptCompiler.AdditionalAssemblies.Add(typeof(System.Linq.Expressions.MemberExpression).Assembly);

            var testScriptGo = engine.AddScript("TestGo", "AttachPropScript");

            const int testPosX = 25;
            const int testPosY = 25;

            testScriptGo.ScriptManager.SetScriptValue("XPos", testPosX);
            testScriptGo.ScriptManager.SetScriptValue("YPos", testPosY);

            engine.Update(0.0f);

            var go = testScriptGo.ScriptManager.GetScriptValue<DrawableGameObject>("TestGo");
            go.Tile.Width.Should().Be(testPosX);
            go.Tile.Height.Should().Be(testPosY);
        }
    }
}