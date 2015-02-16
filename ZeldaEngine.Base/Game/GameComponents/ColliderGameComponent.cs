using System.Linq.Expressions;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game.GameComponents
{
    public class ColliderGameComponent : GameComponent
    {
        public override bool Action(IGameObject gameObject)
        {
            if (gameObject is DrawableGameObject)
            {
                var dGo = (DrawableGameObject) gameObject;
                if (dGo is PlayerGameObject)
                {
                    var player = (PlayerGameObject) dGo;
                    var nearObjects = GameObjectFactory.FindNearGameObject(new Vector2(1, 1));
                    foreach (var nearGo in nearObjects)
                    {
                        GameEngine.Logger.LogInfo("Near object: ({0}-{1}", nearGo.Name, nearGo.Position);
                    }
                }
            }

            return true;
        }
    }
}