using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class PlayerGameObject : DrawableGameObject
    {
        public float MoveVelocity { get; set; }

        public PlayerGameObject(IGameEngine gameEngine)
            : base(gameEngine)
        {
            AddComponent<MovableTestGameComponent>("testMovableComponent");
        }

        public override ObjectType ObjectType =>  ObjectType.PlayableObject;
    }
}