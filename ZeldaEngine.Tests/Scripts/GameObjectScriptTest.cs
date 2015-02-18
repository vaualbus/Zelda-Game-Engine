using ZeldaEngine.Base;
using ZeldaEngine.Base.ValueObjects;
using System.Drawing;

namespace ZeldaEngine.Tests.Scripts
{
    public class GameObjectScriptTest : GameScript
    {
        public void Run()
        {
            RenderEngine?.DrawBox(new Vector2(0,0), 100, Color.Red);
        }

        public void Run(int color)
        {
            
        }

        public void Run(string name)
        {
            
        }
    }
}