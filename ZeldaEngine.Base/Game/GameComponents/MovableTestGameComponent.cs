using System;
using System.Data;
using System.Drawing;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class MovableTestGameComponent : GameComponent
    {
        public override bool Action(IGameObject gameObject)
        {
            if (gameObject is PlayerGameObject)
            {
                var drawableGO = (PlayerGameObject) gameObject;
                //Basic test collision
                const float wallCollisionLeft = 1f;
                if (GameEngine.InputManager.IsKeyDown(GameKeys.Right) && !GameEngine.InputManager.IsKeyUp(GameKeys.Right)) // -->
                {
                    var distanceToWindowEnd = GameEngine.GameConfig.ScreenWidth -
                                              (float) (drawableGO.Center.X + drawableGO.Tile.Width);
                    if (distanceToWindowEnd > wallCollisionLeft)
                        drawableGO.Position = new Vector2(drawableGO.Position.X + (int) drawableGO.MoveVelocity,
                            drawableGO.Position.Y);
                }

                if (GameEngine.InputManager.IsKeyDown(GameKeys.Left) && !GameEngine.InputManager.IsKeyUp(GameKeys.Left)) // <--
                {
                    //var distanceToWindowEnd = GameEngine.Configuration.GameConfig.ScreenWidth - (drawableGO.Position.X + drawableGO.Tile.Width / 2);
                    //if (distanceToWindowEnd <= 0)
                    //    drawableGO.Position.X = 0;
                    //else
                    drawableGO.Position.X -= (int) drawableGO.MoveVelocity;
                }

                if (GameEngine.InputManager.IsKeyDown(GameKeys.Up) && !GameEngine.InputManager.IsKeyUp(GameKeys.Up))
                {
                    drawableGO.Position.Y -= (int) drawableGO.MoveVelocity;
                    if (drawableGO.Position.Y < 0)
                        drawableGO.Position.Y = 0;
                }

                if (GameEngine.InputManager.IsKeyDown(GameKeys.Down) && !GameEngine.InputManager.IsKeyUp(GameKeys.Down))
                {
                    drawableGO.Position.Y += (int) drawableGO.MoveVelocity;
                    if (drawableGO.Position.Y > drawableGO.GameEngine.GameConfig.ScreenHeight)
                        drawableGO.Position.Y = drawableGO.GameEngine.GameConfig.ScreenHeight;
                }
            }
            return true;
        }
    }
}