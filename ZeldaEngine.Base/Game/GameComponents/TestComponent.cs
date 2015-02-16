using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class TestComponent : GameComponent
    {
        public override bool Action(IGameObject parent)
        {
            GameEngine.Logger.LogInfo("Calling game component on {0}", parent.Name);
            return true;
        }
    }
}