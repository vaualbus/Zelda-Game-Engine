using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class MovableGameObject : DrawableGameObject
    {
        public MovableDirection MoveDirection { get; set; }

        public float MoveVelocity { get; set; }

        public MovableGameObject(IGameEngine gameEngine) 
            : base(gameEngine)
        {
            AddComponent<MovableGameComponent>("movableComponent");
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);

            //Automation Movement
            ///TODO: Remeber later to change the width and the height to map width/heght and not like now as the screen width/height
            if (Position.X > GameEngine.Configuration.GameConfig.ScreenWidth)
                Position.X = GameEngine.Configuration.GameConfig.ScreenWidth;
            if (Position.X < 0)
                Position.X = 0;

            if (Position.Y > GameEngine.Configuration.GameConfig.ScreenHeight)
                Position.Y = GameEngine.Configuration.GameConfig.ScreenHeight;
            if (Position.Y < 0)
                Position.Y = 0;

            //Add the possibility to decide the move direction
        }
    }
}