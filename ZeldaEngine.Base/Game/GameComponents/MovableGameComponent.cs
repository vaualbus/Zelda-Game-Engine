using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class MovableGameComponent : GameComponent
    {
        public override bool Action(IGameObject gameObject)
        {
            if (gameObject is MovableGameObject)
            {
                var mGo = gameObject as MovableGameObject;
                if (mGo.MoveDirection == (MovableDirection.Up | MovableDirection.Down))
                {
                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int) mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
            }

            return true;
        }
    }
}