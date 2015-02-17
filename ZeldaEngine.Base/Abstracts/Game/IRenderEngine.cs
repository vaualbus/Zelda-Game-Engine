using System.Collections.Generic;
using System.Drawing;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IRenderEngine
    {
        void Render(IGameObject gameObject);

        void ApplyTexture(DrawableGameObject gameObject, IResourceData texture);

        void UpdateRenderGameTime(int milliseconds);


        void DrawCircle(Vector2 position, float radious, Color fillColor);

        void DrawTriangle(Vector2 position, float radious, Color fillColor);

        void DrawBox(Vector2 position, int width, Color color);

        //public void RenderUI(UIContext uiContext);

        void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects);
    }
}