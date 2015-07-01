using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;

namespace ZeldaEngine.Game.UI.GameObjects
{
    public class UIGameObject : DrawableGameObject
    {
        public UIGameObject(IGameEngine gameEngine) : base(gameEngine)
        {
        }
    }
}