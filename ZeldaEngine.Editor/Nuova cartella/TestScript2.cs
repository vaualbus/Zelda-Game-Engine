using SharpDX;
using ZeldaEngine.Base;
using ZeldaEngine.ScriptEngine.Attributes;

namespace TestScript
{
    [GlobalScript]
    public class TestScript : GameScript
    {
        private int _moveVelocity = 5;
        private float _rotation;

        public void Run()
        {
            //WaitFrame();

            if (Position.X <= Config.GameConfig.ScreenWidth)
                Position.X += _moveVelocity;
            else
            {
                Position.X = 0;

                if (Position.Y >= Config.GameConfig.ScreenHeight)
                {
                    Position.Y = 0;
                    _moveVelocity = 5;
                }

                Position.Y += _moveVelocity;
            }

            //if(InputManager.IsKeyDown("R"))
            _rotation += 1.0f;
        }

        public override void OnDraw()
        {
            RenderEngine.DrawFillCircle(Position, 100, Color.Green);

            ///RenderEngine.DrawLine(100, 100, 300, _rotation, Color.Red, 50);
            //RenderEngine.DrawLine(500, 500, 100, _rotation, Color.Blue, 2);
        }
    }
}
