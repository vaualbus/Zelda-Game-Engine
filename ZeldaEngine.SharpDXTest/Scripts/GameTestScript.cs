using System;
using SharpDX;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.ScriptEngine.Attributes;
using Vector2 = ZeldaEngine.Base.ValueObjects.Vector2;

namespace ZeldaEngine.SharpDXTest.Scripts
{
    [GlobalScript]
    public class GameTestScript : GameScript
    {
        private BallGameObject _ball;
        private MovableGameObject _playerBar;
        private AiBar _aiBar;

        const float BallRadious = 10.0f;
        const int BarSize = 10;
        const float BallSize = BallRadious / 2;

        public override void OnInit()
        {
            Logger.LogInfo("Initializing the game.....");
            SetUpGameObjects();
        }

        public void Run()
        {
            PerformCollision();
        }

        private void SetUpGameObjects()
        {
            _playerBar = GameObjectFactory.Create<PlayerBar>("playerBar", g =>
            {
                g.MoveDirection = MovableDirection.Up | MovableDirection.Down;
                g.MoveVelocity = 50.0f;
                g.Color = Color.White;
                g.Position = new Vector2(100, 250);
            });


            _aiBar = GameObjectFactory.Create<AiBar>("aiBar", g =>
            {
                g.Position = new Vector2(500, 250);
                g.Color = Color.Pink;
            });

            _ball = GameObjectFactory.Create<BallGameObject>("ball", g =>
            {
                g.MoveVelocity = 6;
                g.BallColor = Color.Yellow;
                g.Position = new Vector2(300, 200);
            });
        }

        private void PerformCollision()
        {
            //First we need to update the game objects
            _playerBar.Update(0.0f);
            _aiBar.Update(0.0f);
            _ball.Update(0.0f);
        }

        public override void OnDraw()
        {
            _playerBar.Draw(RenderEngine);
            _aiBar.Draw(RenderEngine);
            _ball.Draw(RenderEngine);
        }

        private class BallGameObject : DrawableGameObject
        {
            private int _direction;
            private float _incrementalVelocity;

            public Color BallColor { get; set; }
            public int MoveVelocity { get; set; }

            public BallGameObject(IGameEngine gameEngine)
                : base(gameEngine)
            {
                MoveVelocity = 1;
                _direction = 1;
            }

            protected override void OnDraw(IRenderEngine renderEngine)
            {
                renderEngine.DrawFillCircle(Position, 10, BallColor);
            }

            protected override void OnUpdate(float dt)
            {
                //we find the distance between the ball and the ai bar/player bar
                var barGo = GameEngine.GameObjectFactory.Find<AiBar>("aiBar");
                var playerGo = GameEngine.GameObjectFactory.Find<PlayerBar>("playerBar");

                var distanceFromBarGo = Vector2.Distance(Position, barGo.Position);
                var distanceFromPlayerGo = Vector2.Distance(Position, playerGo.Position);
                var distance = Math.Min(distanceFromBarGo, distanceFromPlayerGo);

                var xPos = 0.0f;
                if (distance - distanceFromBarGo == 0.0f)
                    xPos = playerGo.Position.X;
                else if (distance - distanceFromPlayerGo == 0.0f)
                    xPos = barGo.Position.X;

                if (distance <= xPos + (BarSize - BallSize))
                {
                    _direction = -_direction;
                    //_incrementalVelocity += 0.8f;
                }
            
                //If the ball go outside the screen, reset the position to the middle
                if (Position.X >= GameEngine.Configuration.GameConfig.ScreenWidth ||
                    Position.X <= 0)
                    Position = new Vector2(300, 200);


                Position.X += MoveVelocity * _direction + _incrementalVelocity;
            }
        }

        private class AiBar : DrawableGameObject
        {
            private bool _isMoved;

            public AiBar(IGameEngine gameEngine)
                : base(gameEngine)
            {
            }

            protected override void OnUpdate(float dt)
            {
                var player = GameEngine.GameObjectFactory.Find("playerBar");

                //Do the update
                Position = _isMoved ? new Vector2(Position.X, player.Position.Y) : Position; 
                _isMoved = true;
            }

            protected override void OnDraw(IRenderEngine renderEngine)
            {
                renderEngine.DrawLine(Position, 100, 90.0f, Color, 10);
            }
        }

        private class PlayerBar : MovableGameObject
        {
            private int _direction;

            public PlayerBar(IGameEngine gameEngine)
                : base(gameEngine)
            {
                _direction = 1;
            }

            protected override void OnInit()
            {
                AddComponent<MovableGameComponent>("movableComponent");
                base.OnInit();
            }

            protected override void OnDraw(IRenderEngine renderEngine)
            {
                renderEngine.DrawLine(Position, 100, 90.0f, Color, 10);
            }
        }
    }
}