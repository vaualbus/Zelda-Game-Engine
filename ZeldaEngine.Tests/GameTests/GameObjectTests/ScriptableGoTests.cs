using System.Reflection;
using NUnit.Framework;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.Tests.TestRig;
using GameScriptEngine = ZeldaEngine.ScriptEngine.GameScriptEngine;

namespace ZeldaEngine.Tests.GameTests.GameObjectTests
{
    [TestFixture]
    public class ScriptableGoTests
    {
        

        private readonly IScriptParamaterProvider _scriptParamsProvider;

        private readonly Config _testConfig = new Config(new GameScriptConfig(@"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
                                                                              @"C:\Users\alberto\Documents\Visual Studio 2015\Projects\ZeldaEngine\ZeldaEngine.Tests",
                                                                              "TestProject"), null);

        private readonly GameScriptEngine _engine;

        public ScriptableGoTests()
        {
            _engine = new ScriptEngine.GameScriptEngine(_testConfig);
            _engine.InitializeEngine();

            _engine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));

            new GameObjectFactory(new TestGameEngine());
        }

        [Test]
        public void AttachScriptToAGameObjectNotThrowException()
        {
            var script = _engine.ScriptCompiler.Compile(@"Scripts\GameObjectScriptTest.cs");
            var go = GameObjectFactory.Create<ScriptableGameObject>("Test", scriptGameObject =>
            {
                scriptGameObject.ScriptParamProvider = _engine.ParamsProvider;
                scriptGameObject.ObjectType = ObjectType.Enemy;

                //scriptGameObject.Scripts.Add(_engine.GetScript());
            });
        }
    }
}