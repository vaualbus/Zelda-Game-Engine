using System.Drawing;
using ZeldaEngine.Base;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Attributes;

namespace TestScript
{
    [GlobalScript]
    public class TestScript : GameScript
    {
        public void Run()
        {
            RenderEngine?.DrawCircle(new Vector2(50, 50), 10, Color.Gold);
            WaitFrame();
        }
    }
}
