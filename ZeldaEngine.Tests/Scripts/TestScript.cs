using System;
using ZeldaEngine.Base;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Attributes;

namespace ZeldaEngine.Tests.Scripts
{
    [GlobalScript]
    public class TestScript : GameScript
    {
        public int X { get; private set; }

        public float TestProp { get; private set; }

        public int TestFunc1(int a)
        {
            _logger.LogWarning("Test {0}", a);
            return a;
        }

        public void TestLog()
        {
            _logger.LogWarning("Test: 10");
        }

       // [GameEntryPoint]
        public void Run(int x, int y)
        {
            _logger.LogInfo("X: {0}, Y: {1}", x, y);
        }
    }
}
