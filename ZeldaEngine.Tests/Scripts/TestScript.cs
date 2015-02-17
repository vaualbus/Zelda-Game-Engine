using ZeldaEngine.Base;
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
            Logger.LogWarning("Test {0}", a);
            return a;
        }

        public void TestLog()
        {
            Logger.LogWarning("Test: 10");
        }

       // [GameEntryPoint]
        public void Run(int x, int y)
        {
            Logger.LogInfo("X: {0}, Y: {1}", x, y);
        }
    }
}
