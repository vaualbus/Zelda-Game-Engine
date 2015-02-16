using System.Runtime.InteropServices;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using ZeldaEngine.Tests.TestRig;

namespace ZeldaEngine.Tests
{


    public class Vector3f
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Attenuation : Vector3f
    {
        public float Constant { get { return X; } }

        public float Linear { get { return Y; } }

        public float Exponent { get { return Z; } }

        public Attenuation(float constant, float linear, float exponent)
            : base(constant, linear, exponent)
        {

        }
    }

    [TestFixture]
    public class ScriptTest
    {
        [Test]
        public void CanCompileAnGetMethodNotThrowAnException()
        {
            var engine = new GameScriptEngine();
            engine.InitializeEngine();

            engine.ScriptManager.AddScript(engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"), "Test");
            engine.ScriptManager.AddScript(engine.ScriptCompiler.Compile(@"Scripts\Script2.cs"), "Script");

            engine.ScriptManager.ExcuteFunction("Test", "Test", new object[] {3}).Should().Be(3);
            engine.ScriptManager.ExcuteFunction("Script", "Test", new object[] { 3 }).Should().Be(3);

            var testPropValue = engine.ScriptManager.GetScriptValue<float>("Script", "TestProp");
            testPropValue.Should().Be(24.0f);
        }

        [Test]
        public void SystemCanMengaeMultithreadEnviroment()
        {
            var engine = new GameScriptEngine();
            engine.InitializeEngine();

            engine.ScriptManager.AddScript(engine.ScriptCompiler.Compile(@"Scripts\TestScript.cs"), "Test");
            engine.ScriptManager.AddScript(engine.ScriptCompiler.Compile(@"Scripts\Script2.cs"), "Script");


            var th1 = new Thread(() => engine.ScriptManager.ExcuteFunction("Test", "Test", new object[] { 3 }).Should().Be(3));
            var th2 = new Thread(() => engine.ScriptManager.ExcuteFunction("Test", "Test", new object[] { 3 }).Should().Be(3));
            var th3 = new Thread(() => engine.ScriptManager.ExcuteFunction("Test", "Test", new object[] { 25 }).Should().Be(25));

            th1.Start();
            th2.Start();
            th3.Start();
        }

        [Test]
        public void CanVector3FBeAssignableFromDerivedClass()
        {
            var atten = new Attenuation(0, 0, 0);
            typeof (Vector3f).IsAssignableFrom(typeof (Attenuation)).Should().Be(true);
        }

        [Test]
        public void CanGenerateProjectFolderAndProjectFile()
        {
            var engine = new GameScriptEngine(true)
            {
                EnginePath = @"C:\Users\Gianpaolo-Pc\Desktop\ZeldaEngine\ZeldaEngine.Tests",
                ProjectName = "TestProj",
                ProjectFolder = "TestProj"
            };

            engine.InitializeEngine();
        }
    }
}