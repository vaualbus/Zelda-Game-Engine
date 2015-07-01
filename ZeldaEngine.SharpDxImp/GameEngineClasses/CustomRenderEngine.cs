using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.SharpDxImp.GameCode.ValueObjects;
using RectangleF = SharpDX.RectangleF;
using Texture2D = SharpDX.Toolkit.Graphics.Texture2D;

namespace ZeldaEngine.SharpDxImp.GameEngineClasses
{
    public class CustomRenderEngine : IRenderEngine
    {
        private readonly IGameEngine _gameEngine;

        private readonly Texture2D _emptyTexture;

        public GraphicsDevice GraphicsDevice { get; private set; }

        public SpriteBatch SpriteBatch { get; }

        public CustomRenderEngine(IGameEngine gameEngine, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _gameEngine = gameEngine;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;

            _emptyTexture = Texture2D.New(GraphicsDevice, 1, 1, 0, PixelFormat.R8G8B8A8.UNormSRgb);
            var pixels = new[] {0xFFFFFF};
            _emptyTexture.SetData(pixels);
        }

        public void Render(IGameObject gameObject)
        {
            var drawableGo = gameObject as DrawableGameObject;
            if (drawableGo != null)
            {
                var texture = drawableGo.Tile.Texture.Value as Texture2D;
                var position = new Vector2(drawableGo.Position.X, drawableGo.Position.Y);

                if (texture == null)
                    return;

                ///TODO(albus95): Implement the layer stuff here....
                /// Layer = -1 mean transparen
                SpriteBatch.Begin();
                SpriteBatch.Draw(texture, new RectangleF(position.X, position.Y, drawableGo.Tile.Width, drawableGo.Tile.Height),
                                (Color) (drawableGo.Color ?? Color.White));

                SpriteBatch.End();
            }
        }

        public void RenderTexture(string test, Vector2 position, SharpDX.Color color, int layer = 0)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_gameEngine.ResourceLoader.Load<Texture2D>(test), position, color);
            SpriteBatch.End();
        }

        public void DrawBox(Base.ValueObjects.Vector2 position, int width, object color)
        {
            throw new System.NotImplementedException();
        }

        public void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
        {
            throw new System.NotImplementedException();
        }

        public void DrawString(Base.ValueObjects.Vector2 position, string text, float size, object color)
        {
            if (text == null)
                return;

            DrawString(position, text, _gameEngine.GameConfig.DefaultFont, size, color);
        }

        public void DrawString(Base.ValueObjects.Vector2 position, string text, string fontName, float size, object color)
        {
            if (text == null)
                return;

            var font = _gameEngine.ResourceLoader.Load<SpriteFont>(fontName);
            
            SpriteBatch.Begin();
            SpriteBatch.DrawString(font, text, new Vector2(position.X, position.Y), (Color)color);
            SpriteBatch.End();
        }

        public void DrawCircle(Base.ValueObjects.Vector2 position, int radius, object lineColor)
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
            SpriteBatch.Draw(texture, new Vector2(position.X, position.Y), (Color) lineColor);
            SpriteBatch.End();
        }

        public void DrawFillCircle(Base.ValueObjects.Vector2 position, int radius, object fillColor)
        {
            int outerRadius = radius * 2 + 2;
            var texture = Texture2D.New(GraphicsDevice, outerRadius, outerRadius, true, PixelFormat.R8G8B8A8.UNormSRgb);

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

                data[y * outerRadius + x + 1] = Color.Black;
            }

            for (int i = 0; i < outerRadius; i++)
            {
                int yStart = -1;
                int yEnd = -1;


                //loop through height to find start and end to fill
                for (int j = 0; j < outerRadius; j++)
                {

                    if (yStart == -1)
                    {
                        if (j == outerRadius - 1)
                        {
                            //last row so there is no row below to compare to
                            break;
                        }

                        //start is indicated by Color followed by Transparent
                        if (data[i + (j * outerRadius)] == Color.Black && data[i + ((j + 1) * outerRadius)] == Color.Transparent)
                            yStart = j + 1;
                    }
                    else if (data[i + (j * outerRadius)] == Color.Black)
                        yEnd = j;
                }

                //if we found a valid start and end position
                if (yStart != -1 && yEnd != -1)
                {
                    //height
                    for (int j = yStart; j < yEnd; j++)
                    {
                        data[i + (j * outerRadius)] = (Color) fillColor;
                    }
                }
            }

            texture.SetData(data);

            SpriteBatch.Begin();
            SpriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
            SpriteBatch.End();
        }

        public void DrawLine(Base.ValueObjects.Vector2 start, Base.ValueObjects.Vector2 end, object lineColor, int thickness = 1)
        {
            //First we sould want to calculate the angle of the rect.
            var lenght = (int) Base.ValueObjects.Vector2.Distance(start, end);
            var rotation = CalculateRotation(start, end);
            var rect = new Rectangle((int)start.X, (int)start.Y, lenght, thickness);

            SpriteBatch.Begin();
            SpriteBatch.Draw(_emptyTexture, rect, null, (SharpDX.Color) lineColor, rotation, new Vector2(0, 0), SpriteEffects.None,  0.0f);
            SpriteBatch.End();
        }

        public void DrawLine(int x0, int y0, int x1, int y1, object lineColor, int thickness = 1)
        {
            DrawLine(new Base.ValueObjects.Vector2(x0, y0), new Base.ValueObjects.Vector2(x1, y1), lineColor, (int) thickness);
        }

        public void DrawLine(Base.ValueObjects.Vector2 start, int lenght, float rotation, object lineColor, int thickness)
        {
            var rect = new Rectangle((int)start.X, (int)start.Y, lenght, thickness);

            SpriteBatch.Begin();
            SpriteBatch.Draw(_emptyTexture, rect, null, (Color)lineColor, -MathUtil.DegreesToRadians(rotation), new Vector2(0, 0), SpriteEffects.None, 0.0f);
            SpriteBatch.End();
        }
        
        public void DrawLine(int x0, int y0, int lenght, float rotation, object lineColor, int thickness)
        {
            DrawLine(new Base.ValueObjects.Vector2(x0, y0), lenght, rotation, lineColor, thickness);
        }

        public void DrawTexture(Base.ValueObjects.Vector2 position, IResourceData texture, float rotation, object color, int layer = 0)
        {
            var texture2D = (Texture2D) texture.Value;
            var rect = new Rectangle((int)position.X, (int)position.Y, texture2D.Width, texture.Height);

            SpriteBatch.Begin();
            SpriteBatch.Draw((Texture2D) texture.Value, rect, null, (Color) color, rotation, new Vector2(0,0), SpriteEffects.None, layer);
            SpriteBatch.End();
        }

        public void DrawTriangle(Base.ValueObjects.Vector2 position, Vertex[] verticies, object lineColor, int thickness = 1)
        {
            throw new NotImplementedException();
        }

        public void DrawFillTriangle(Base.ValueObjects.Vector2 position, Vertex[] verticies)
        {
            throw new NotImplementedException();
        }

        public void UpdateRenderGameTime(int milliseconds)
        {
        }

        public void ApplyTexture(DrawableGameObject gameObject, IResourceData texture)
        {
            // gameObject.Texture = texture;
        }

        public void RenderUI(UIContext uiContext)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _emptyTexture.Dispose();
        }

        private float CalculateRotation(Base.ValueObjects.Vector2 start, Base.ValueObjects.Vector2 end)
        {
            var adj = start.X - end.X;
            var opp = start.Y - end.Y;
            var tan = opp / adj;
            var res = MathUtil.RadiansToDegrees((float) Math.Atan2(opp, adj));
            res = (res - 180)%360;
            if (res < 0)
                res += 360;

            res = MathUtil.DegreesToRadians(res);
            return res;
        }
    }
}