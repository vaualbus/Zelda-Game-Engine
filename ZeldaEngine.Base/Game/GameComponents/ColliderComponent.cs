using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class ColliderComponent : GameComponent
    {
        public override bool Action(IGameObject gameObject)
        {
            return true;
        }
    }
}