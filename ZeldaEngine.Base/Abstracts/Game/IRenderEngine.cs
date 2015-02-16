using ZeldaEngine.Base.Game.GameObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IRenderEngine
    {
        void Render(IGameObject gameObject);

        void ApplyTexture(DrawableGameObject gameObject, IResourceData texture);
        void UpdateRenderGameTime(int milliseconds);
    }
}