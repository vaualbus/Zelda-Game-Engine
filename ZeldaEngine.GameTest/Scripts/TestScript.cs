using System;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.ScriptEngine.Attributes;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Scripts
{
    [GlobalScript]
    public class TestScript : ZeldaScript
    {
        public int X { get; private set; }

        public float TestProp { get; private set; }

        public TestScript()
        {
            TestProp = 24f;
        }

        public override void ApplicationInit()
        {
        }

        public void Run(int enemyId, string b)
        {
            Console.WriteLine("Working 2: {0} - {1}", enemyId, b);
            Log("Test Warning", ReportReason.Warning);
        }

        public void Run(int x)
        {
            Console.WriteLine("Working 1: {0}", x);
            Log("Test Warning", ReportReason.Warning);
        }

        public void Run()
        {
            Console.WriteLine("Working!");
            Log("Test Warning", ReportReason.Warning);
        }

        public void TestFunc(float f, char a, float c)
        {
            Console.WriteLine("You passed me: {0} - {1} - {2}", f, a, c);
            Log("Test Warning", ReportReason.Warning);
        }
    }
}
