using ZeldaEngine.Base;
using ZeldaEngine.ScriptEngine.Attributes;

namespace ZeldaEngine.Tests.Scripts
{
    [GlobalScript]
    public class Script2 : GameScript
    {
        public int Data { get; private set; }

        public Script2(int data)
        {
            Data = data;
        }

        public int ScriptFunc1()
        {
            return Data;
        }
    }
}
