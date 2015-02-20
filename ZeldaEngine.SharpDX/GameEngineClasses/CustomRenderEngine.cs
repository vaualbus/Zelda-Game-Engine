using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.SharpDx.GameCode.ValueObjects;
using RectangleF = SharpDX.RectangleF;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomRenderEngine : IRenderEngine
    {
        private readonly IGameEngine _gameEngine;
        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch SpriteBatch { get; }

        public CustomRenderEngine(IGameEngine gameEngine, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _gameEngine = gameEngine;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
        }

        public void Render(IGameObject gameObject)
        {
            var drawableGo = gameObject as DrawableGameObject;
            if (drawableGo != null)
            {
                var texture = drawableGo.Tile.Texture.Value as Texture2D;
                var position = new Vector2(drawableGo.Position.X, drawableGo.Position.Y);

                ///TODO: Implement the layer stuff here....

                SpriteBatch.Begin();
                SpriteBatch.Draw(texture, new RectangleF(position.X, position.Y, drawableGo.Tile.Width,
                                 drawableGo.Tile.Height), SharpDX.Color.Wheat);
                SpriteBatch.End();
            }
        }

        public void RenderUI(UIContext uiContext)
        {
            throw new System.NotImplementedException();
        }

        public void DrawBox(Base.ValueObjects.Vector2 position, int width, object color)
        {
            throw new System.NotImplementedException();
        }

        public void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
        {
            throw new System.NotImplementedException();
        }

        public void ApplyTexture(DrawableGameObject gameObject, IResourceData texture)
        {
           // gameObject.Texture = texture;
        }

        public void UpdateRenderGameTime(int milliseconds)
        {
        }

        public void DrawCircle(Base.ValueObjects.Vector2 position, int radius, object fillColor)
        {
            int outerRadius = radius*2 + 2;
            var texture = Texture2D.New(GraphicsDevice, outerRadius, outerRadius, 0, PixelFormat.B8G8R8A8.UNorm);

            var data = new Color[outerRadius * outerRadius];
            for (var i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = Color.White;
            }

    
            texture.SetData(data);

            SpriteBatch.Begin();
            SpriteBatch.Draw(texture, new Vector2(position.X, position.Y), (Color) fillColor);
            SpriteBatch.End();
        }

        public void DrawTriangle(Base.ValueObjects.Vector2 position, float radious, Color fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void RenderTexture(string test, Vector2 position, SharpDX.Color color, int layer = 0)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_gameEngine.ResourceLoader.Load<Texture2D>(test), position, color);
            SpriteBatch.End();
        }
    }
}