using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Tests.TestRig
{
    public class InMemoryRenderEngine : IRenderEngine
    {
        private readonly IGameEngine _gameEngine;

        public InMemoryRenderEngine(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public string CurrentFontName { get { return "Return Font"; } }
        public object GraphicsDevice => null;

        public void Render(IGameObject gameObject)
        {
            _gameEngine.Logger.LogInfo("Rendering {0} game object", gameObject.Name);
        }

        public void ApplyTexture(DrawableGameObject gameObject, IResourceData texture)
        {
            _gameEngine.Logger.LogInfo("Appying {0} game object", gameObject.Name);
        }

        public void UpdateRenderGameTime(int milliseconds)
        {
            _gameEngine.Logger.LogInfo("Updating the render engine");
        }

        public void DrawCircle(Vector2 position, int radious, object lineColor)
        {
            _gameEngine.Logger.LogInfo("Drawing circle in {0}, radius: {1}", position, radious);
        }

        public void DrawFillCircle(Vector2 position, int radious, object fillColor)
        {
            _gameEngine.Logger.LogInfo("Drawing fill circle in {0}, radius: {1}", position, radious);
        }

        public void DrawLine(Vector2 start, Vector2 end, object lineColor, int thickness = 1)
        {
            _gameEngine.Logger.LogInfo("Drawing line {0} - {1}", start, end);
        }

        public void DrawLine(int x0, int y0, int x1, int y1, object lineColor, int thickness = 1)
        {
            _gameEngine.Logger.LogInfo("Drawing line {0} - {1}", new Vector2(x0, y0), new Vector2(x1, y1));
        }

        public void DrawLine(Vector2 start, int lenght, float rotation, object lineColor, int thickness = 1)
        {
            _gameEngine.Logger.LogInfo("Drawing line {0} lenght {1}, rotation {2}, thickness: {3}", start, lenght, rotation, thickness);
        }

        public void DrawLine(int x0, int y0, int lenght, float rotation, object lineColor, int thickness = 1)
        {
            _gameEngine.Logger.LogInfo("Drawing line {0} lenght {1}, rotation {2}, thickness: {3}", new Vector2(x0, y0), lenght, rotation, thickness);
        }

        public void DrawTexture(Vector2 position, IResourceData texture, float rotation, object color, int layer = 0)
        {
            _gameEngine.Logger.LogInfo("Drawing texture {0} rotation {1}", position, rotation);
        }

        public void DrawTriangle(Vector2 position, Vertex[] verticies, object lineColor, int thickness = 1)
        {
        }

        public void DrawFillTriangle(Vector2 position, Vertex[] verticies)
        {
        }

        public void DrawBox(Vector2 position, int width, object color)
        {
            _gameEngine.Logger.LogInfo("Drawing box {0}, Width: {1}", position, width);
        }

        public void DrawRect(Vector2 position, int width, int height, object color)
        {
            _gameEngine.Logger.LogInfo("Drawing rect {0},  Width: {1} Height: {2}", position, width, height);
        }

        public void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects)
        {
        }

        public void DrawString(Vector2 position, string text, float size, object color)
        {
            _gameEngine.Logger.LogInfo("Drawing string {0} - {1} - {2}", position, text, size);
        }

        public void DrawString(Vector2 position, string text, string fontName, float size, object color)
        {
            _gameEngine.Logger.LogInfo("Drawing string {0} - {1} - {2} with font", position, text, size, fontName);
        }

        public void Dispose()
        {
            _gameEngine.Logger.LogInfo("Disposing the render engine");
        }
    }
}