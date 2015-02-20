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


        void DrawCircle(Vector2 position, int radious, object fillColor);

        //void DrawTriangle(Vector2 position, int @base, Color fillColor);

        void DrawBox(Vector2 position, int width, object color);

        //public void RenderUI(UIContext uiContext);

        void DrawCollisionLines(GameObject go, IEnumerable<GameObject> nearestObjects);
    }
}