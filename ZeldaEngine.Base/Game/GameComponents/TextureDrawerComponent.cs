using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class TextureDrawerComponent : GameComponent
    {
        public override bool Action(IGameObject gameObject)
        {
            if (gameObject is DrawableGameObject)
            {
                var drawableGO = (DrawableGameObject) gameObject;

                GameEngine.RenderEngine.ApplyTexture(drawableGO, drawableGO.Tile.Texture);
                GameEngine.RenderEngine.Render(gameObject);
            }

            return true;
        }
    }
}