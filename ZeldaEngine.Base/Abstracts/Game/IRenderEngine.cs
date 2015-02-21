using System.Collections.Generic;
using System.Drawing;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IRenderEngine
    {
        void Render(IGameObject gameObject);

        void ApplyTexture(DrawableGameObject gameObject, IResourceData texture);

        void UpdateRenderGameTime(int milliseconds);

        void DrawCircle(Vector2 position, int radious, object lineColor);

        void DrawFillCircle(Vector2 position, int radious, object fillColor);

        void DrawLine(Vector2 start, Vector2 end, object lineColor, int thickness = 1);

        void DrawLine(int x0, int y0, int x1, int y1, object lineColor, int thickness = 1);

        void DrawLine(Vector2 start, int lenght, float rotation, object lineColor, int thickness = 1);

        void DrawLine(int x0, int y0, int lenght, float rotation, object lineColor, int thickness = 1);

        void DrawTriangle(Vector2 position, Vertex[] verticies, object lineColor, int thickness = 1);

        void DrawFillTriangle(Vector2 position, Vertex[] verticies);

        void DrawBox(Vector2 position, int width, object color);

        void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects);
    }
}