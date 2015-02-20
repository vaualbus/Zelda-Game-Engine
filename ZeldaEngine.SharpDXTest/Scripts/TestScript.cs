using SharpDX;
using ZeldaEngine.Base;
using ZeldaEngine.ScriptEngine.Attributes;
using Vector2 = ZeldaEngine.Base.ValueObjects.Vector2;

namespace TestScript
{
    [GlobalScript]
    public class TestScript : GameScript
    {
        private Vector2 _position;

        private int _moveVelocity = 5;

        public TestScript()
        {
            _position = new Vector2(10, 10);
        }

        public void Run()
        {
            //WaitFrame();
            if(InputManager.IsKeyDown("R"))
                _position = new Vector2(0, 0);

            if (_position.X <= Config.GameConfig.ScreenWidth)
                _position.X += _moveVelocity;
            else
            {
                _position.X = 0;

                if (_position.Y >= Config.GameConfig.ScreenHeight)
                {
                    _position.Y = 0;
                    _moveVelocity = 5;
                }

                _position.Y += _moveVelocity;
            }
        }

        public override void OnDraw()
        {
            RenderEngine.DrawCircle(_position, 50, Color.Yellow);
        }
    }
}
