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
                ///TODO(albus95): Not completed need to add diagonal moves
                var mGo = gameObject as MovableGameObject;
                if (mGo.MoveDirection == (MovableDirection.Up | MovableDirection.Down))
                {
                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int) mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.Up)
                {
                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.Down)
                {
                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.Left)
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.Right)
                {
                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == (MovableDirection.Left | MovableDirection.Right))
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == (MovableDirection.Left | MovableDirection.Right | MovableDirection.Up | MovableDirection.Down))
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.UpDown)
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == (MovableDirection.Left | MovableDirection.Right | MovableDirection.UpDown))
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.LeftRight)
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == (MovableDirection.LeftRight | MovableDirection.UpDown))
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
                if (mGo.MoveDirection == MovableDirection.UDLR)
                {
                    if (InputManager.IsKeyDown("Left"))
                        mGo.Position.X -= (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Right"))
                        mGo.Position.X += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Down"))
                        mGo.Position.Y += (int)mGo.MoveVelocity;

                    if (InputManager.IsKeyDown("Up"))
                        mGo.Position.Y -= (int)mGo.MoveVelocity;
                }
            }

            return true;
        }
    }
}