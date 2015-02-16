using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class DefaultGameObject : GameObject
    {
        public DefaultGameObject(IGameEngine gameEngine)
            : base(gameEngine)
        {
            Name = "Default Game Object";
            Position = Rotation = Scaling = new Vector2(0, 0);
        }

        public override ObjectType ObjectType
        {
            get { return ObjectType.Null; }
        }

        protected override void OnUpdate(float dt)
        {
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            renderEngine.Render(this);
        }
    }
}