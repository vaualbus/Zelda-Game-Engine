using System;
using SharpDX;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.Game.Attributes;
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
        private Walls _walls;
        private DrawableGameObject _testGo;

        const float BallRadious = 10.0f;
        const int BarSize = 10;
        const float BallSize = BallRadious / 2;

        private  IResourceData[] _testTextures;
        private int _currentTextureIndex = 0;

        [DataFrom("dataScript.Text")]
        private string _testField;

        public override void OnInit()
        {
            _testTextures = new IResourceData[5];

            Logger.LogInfo("Initializing the game.....");
            SetUpGameObjects();
        }

        public void Run()
        {
            if (InputManager.IsKeyDown("Right"))
            {
                _currentTextureIndex++;
                if (_currentTextureIndex > _testTextures.Length - 1)
                    _currentTextureIndex = 0;

                
            }
            if (InputManager.IsKeyDown("Left"))
            {
                _currentTextureIndex--;
                if (_currentTextureIndex < 0)
                    _currentTextureIndex = _testTextures.Length - 1;
            }

            //_currentTextureIndex--;
            //if (_currentTextureIndex < 0)
            //    _currentTextureIndex = _testTextures.Length - 1;

            _testGo.Tile.Texture = _testTextures[_currentTextureIndex];

            PerformCollision();

        }



        private void SetUpGameObjects()
        {
            //_playerBar = GameObjectFactory.Create<PlayerBar>("playerBar", g =>
            //{
            //    g.MoveDirection = MovableDirection.Up | MovableDirection.Down;
            //    g.MoveVelocity = 50.0f;
            //    g.Color = Color.White;
            //    g.Position = new Vector2(100, 250);
            //});


            //_aiBar = GameObjectFactory.Create<AiBar>("aiBar", g =>
            //{
            //    g.Position = new Vector2(400, 250);
            //    g.Color = Color.Pink;
            //});
            //_testTexture = LoadTexture2D("Default");
            // Populate the textures array
            _testTextures = new[]
            {
                LoadTexture2D("Default"),
                LoadTexture2D("Grass"),
                LoadTexture2D("2"),
            };

            _ball = GameObjectFactory.Create<BallGameObject>("ball", g =>
            {
                g.MoveVelocity = 10;
                g.BallColor = Color.Yellow;
                g.Position = new Vector2(100, 230);
            });

            _walls = GameObjectFactory.Create<Walls>("walls", go =>
            {
                go.WallSize = 15;
                go.WallColor = Color.Gray;
            });

            _testGo = GameObjectFactory.Create<DrawableGameObject>("test", g =>
            {
                g.Position = new Vector2(150, 150);
                g.Tile.Width = 500;
                g.Tile.Height = 450;
            });

        }

        private void PerformCollision()
        {
            //First we need to update the game objects
            //_playerBar.Update(0.0f);
            //_aiBar.Update(0.0f);
            _ball.Update(0.0f);
            _walls.Update(0.0f);


            _testGo.Update(0.0f);
        }

        public override void OnDraw()
        {
            
            RenderEngine.DrawString(new Vector2(100, 100), _testField, 30, Color.Blue);

           // _playerBar.Draw(RenderEngine);
           // _aiBar.Draw(RenderEngine);
            _ball.Draw(RenderEngine);
            _walls.Draw(RenderEngine);
            _testGo.Draw(RenderEngine);

            // _testGo.Tile.Texture = LoadTexture2D("Default");
        }

        private class AiBar : DrawableGameObject
        {
            private bool _isMoved;

            public AiBar(IGameEngine gameEngine)
                : base(gameEngine)
            {
                Tile.Height = 100;
                Tile.Width = 10;
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

        private class BallGameObject : DrawableGameObject
        {
            private int _directionX;
            private float _directionY;
            private float _incrementalVelocity;

            public Color BallColor { get; set; }

            public int MoveVelocity { get; set; }

            public BallGameObject(IGameEngine gameEngine)
                : base(gameEngine)
            {
                MoveVelocity = 1;
                _directionX = 1;
                _directionY = 0;

                Tile.Height = 20;
                Tile.Width = 20;
            }

            protected override void OnUpdate(float dt)
            {
                ////we find the distance between the ball and the ai bar/player bar
                //var barGo = GameEngine.GameObjectFactory.Find<AiBar>("aiBar");
                //var playerGo = GameEngine.GameObjectFactory.Find<PlayerBar>("playerBar");

                ////var realPosition = new Vector2(Position.X + BallRadious, Position.Y + BallRadious);
                ////var realBarPosition = new Vector2(barGo.Position.X - BarSize, barGo.Position.Y);
                ////var realPlayerPosition = new Vector2(playerGo.Position.X + BarSize, playerGo.Position.Y);

                ////var distanceFromBarGo = Vector2.Distance(realPosition, realBarPosition);
                ////var distanceFromPlayerGo = Vector2.Distance(realPosition, realPlayerPosition);
                ////var distance = _directionX > 0 ? distanceFromBarGo : distanceFromPlayerGo;

                ////var xPos = 0.0f;
                ////if (distance - distanceFromBarGo < 0.0f && _directionX < 0)
                ////    xPos = realPlayerPosition.X;
                ////else if (distance - distanceFromPlayerGo < 0.0f && _directionX > 0)
                ////    xPos = realBarPosition.X;

                ////if (distance - xPos < 0)
                ////    _directionX = -_directionX;


                //var realBallPosition = new Vector2(Position.X + 2 * BallRadious, Position.Y /*+ BallSize*/);
                //var realBarPosition = new Vector2(barGo.Position.X - BarSize, barGo.Position.Y);
                //var realPlayerPosition = new Vector2(playerGo.Position.X + BarSize, playerGo.Position.Y);
                //if (_directionX > 0)
                //{
                //    //We are traveling to the ai bar
                //    if (realBallPosition.X > realBarPosition.X)
                //    {
                //        if (realBallPosition.Y <= realPlayerPosition.Y + BarSize)
                //            _directionX = -1;
                //    }
                //}
                //else
                //{
                //    //We are traveling to the ai bar
                //    if (realBallPosition.X < realPlayerPosition.X + 2 * BallRadious)
                //    {                                                    
                //        if (realBallPosition.Y <= realBarPosition.Y + BarSize)
                //            _directionX = 1;
                //    }
                //}

                ////If the ball go outside the screen, reset the position to the middle
                //if (Position.X >= GameEngine.GameConfig.ScreenWidth || Position.X <= 0)
                //    Position = new Vector2(300, 200);

                Position.X += MoveVelocity *_directionX + _incrementalVelocity;
                Position.Y += _directionY;

            }

            protected override void OnDraw(IRenderEngine renderEngine)
            {
                GameEngine.RenderEngine.DrawString(new Vector2(100, 400), 
                    string.Format("Colliding with: {0}", _directionX > 0 ? "player bar" : "ai bar"),
                    10,
                    SharpDX.Color.Red);

                renderEngine.DrawFillCircle(Position, 10, BallColor);
            }


            public void CollisionX()
            {
                _directionX = -_directionX;
               // _incrementalVelocity++;
            }

            public void CollisionY()
            {
                _directionY = -_directionY;
                // _incrementalVelocity++;
            }
        }


        private class PlayerBar : MovableGameObject
        {
            public PlayerBar(IGameEngine gameEngine)
                : base(gameEngine)
            {
                Tile.Height = 100;
                Tile.Width = 10;
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

        private class Walls : DrawableGameObject 
        {
            public int WallSize { get; set; }

            public Color WallColor
            {
                set
                {
                    _wallColor = value;
                    _upWallColor = _downWallColor = _leftWallColor = _rightWallColor = value;
                }
            }

            private Color _wallColor;

            private Color _upWallColor;
            private Color _downWallColor;
            private Color _leftWallColor;
            private Color _rightWallColor;

            public Walls(IGameEngine gameEngine)
                : base(gameEngine)
            {
                WallSize = 20;
                WallColor = SharpDX.Color.White;
            }

            protected override void OnUpdate(float dt)
            {
                var ball = GameEngine.GameObjectFactory.Find<BallGameObject>("ball");

                var upWallRect = new Rectangle(0, 0, GameEngine.GameConfig.ScreenWidth, WallSize);
                var downWallRect = new Rectangle(0, GameEngine.GameConfig.ScreenHeight - WallSize, GameEngine.GameConfig.ScreenWidth, WallSize);
                var leftWallRect = new Rectangle(0, WallSize, WallSize, GameEngine.GameConfig.ScreenHeight - WallSize);
                var rightWallRect = new Rectangle(GameEngine.GameConfig.ScreenWidth - WallSize, WallSize, GameEngine.GameConfig.ScreenWidth, WallSize);
                

                //Menage the wall ball collision
                if (ball.Position.X  > GameEngine.GameConfig.ScreenWidth - (WallSize + 2 * BallRadious))
                    ball.CollisionX();
                //Menage the wall ball collision
                if (ball.Position.X - 2 * BallSize < WallSize)
                    ball.CollisionX();
                //Menage the wall ball collision
                if (ball.Position.Y - 2 * BallSize > GameEngine.GameConfig.ScreenHeight - WallSize)
                    ball.CollisionY();
                //Menage the wall ball collision
                if (ball.Position.Y - 2 * BallSize < WallSize)
                    ball.CollisionY();
            }

            protected override void OnDraw(IRenderEngine renderEngine)
            {
               //Draw the four wall
               GameEngine.RenderEngine.DrawLine(0, 0, GameEngine.GameConfig.ScreenWidth, 0, _upWallColor, WallSize);
               GameEngine.RenderEngine.DrawLine(0, GameEngine.GameConfig.ScreenHeight - WallSize, GameEngine.GameConfig.ScreenWidth, GameEngine.GameConfig.ScreenHeight - WallSize, _downWallColor, WallSize);

               GameEngine.RenderEngine.DrawLine(WallSize, WallSize, WallSize, GameEngine.GameConfig.ScreenHeight - WallSize, _leftWallColor, WallSize);
               GameEngine.RenderEngine.DrawLine(GameEngine.GameConfig.ScreenWidth, WallSize, GameEngine.GameConfig.ScreenWidth, GameEngine.GameConfig.ScreenHeight - WallSize, _rightWallColor, WallSize);
            }
        }
    }
}