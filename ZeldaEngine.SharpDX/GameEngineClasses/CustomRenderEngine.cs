using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.SharpDx.GameCode.ValueObjects;
using BlendState = SharpDX.Toolkit.Graphics.BlendState;
using Color = System.Drawing.Color;
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

        public void RenderCircle(Vector2 position, float radious, Color fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void RenderTriangle(Vector2 position, float radious, Color fillColor)
        {
            throw new System.NotImplementedException();
        }

        public void RenderUI(UIContext uiContext)
        {
            throw new System.NotImplementedException();
        }

        public void RenderCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
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

        public void RenderTexture(string test, Vector2 position, global::SharpDX.Color color, int layer = 0)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_gameEngine.ResourceLoader.Load<Texture2D>(test), position, color);
            SpriteBatch.End();
        }
    }
}