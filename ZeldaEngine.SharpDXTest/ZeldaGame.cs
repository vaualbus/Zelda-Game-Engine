using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SharpDX;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.MapLoaders;
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.ScriptEngine;

namespace ZeldaEngine.SharpDXTest
{
    public class ZeldaGame : IGame
    {
        private GameScriptEngine _scriptEngine;

        private QuestManager _questManager;
        private List<GameObject> _gOs;
        private PlayerGameObject _playerGo;
        private GameView _testView;
        private ScriptableGameObject _scriptGo;
        private ScriptableGameObject _scriptGo2;

        public IGameEngine GameEngine { get; set; }


        public void HandleInput(float delta)
        {
        }

        public bool Init()
        {
            _gOs = new List<GameObject>();

            _scriptEngine = new GameScriptEngine(GameEngine);
            _scriptEngine.InitializeEngine();

            GameEngine.ScriptEngine = _scriptEngine;

            _questManager = new QuestManager(GameEngine, new JsonMapLoader(GameEngine));
            _questManager.LoadQuest("TestQuest");

            //_playerGo = GameObjectFactory.Create<PlayerGameObject>("Player", g =>
            // {
            //     g.Tile = new Tile(GameEngine, 50, 50)
            //     {
            //         Texture = GameEngine.TextureData("Default")
            //     };

            //     g.Position = new Base.ValueObjects.Vector2(10, 10);
            //     g.MoveVelocity = 50f;
            // });

            //const int sizeX = 1024;
            //const int sizeY = 768;
            //const int tileWidth = 50;
            //const int tileHeight = 50;
            //const int spacing = 100;
            //const int spacingLeft = 200;
            //const int spacingTop = 200;

            //for (var x = 0; x < sizeX; x += tileWidth + spacing)
            //{
            //    for (var y = 0; y < sizeY; y += tileHeight + spacing)
            //    {
            //        var _x = x;
            //        var _y = y;

            //        _gOs.Add(GameObjectFactory.Create<DrawableGameObject>(string.Format("Test_{0}_{1}", _x, _y), g =>
            //        {
            //            g.Tile = new Tile(GameEngine, tileWidth, tileHeight)
            //            {
            //                Texture = GameEngine.TextureData("Grass")
            //            };

            //            g.Position = new Base.ValueObjects.Vector2(_x == 0 ? _x + spacingLeft : _x, _y == 0 ? _y + spacingTop : _y);
            //        }));
            //    }
            //}

            //_testView = new GameView("Test", new Base.ValueObjects.Vector2(0, 0), new Base.ValueObjects.Vector2(0, 0));
            //_testView.GameObjects.Add(_playerGo);
            //_testView.GameObjects.AddRange(_gOs);

            _scriptEngine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));

            //var testScript1 = CreateScript("TestScript1", Path.Combine(GameEngine.Configuration.GameConfig.ScriptDirectory, "TestScript1.cs"));
            //var testScript2 = CreateScript("TestScript2", Path.Combine(GameEngine.Configuration.GameConfig.ScriptDirectory, "TestScript2.cs"));
            //var testScript3 = CreateScript("TestScript3", Path.Combine(GameEngine.Configuration.GameConfig.ScriptDirectory, "TestScript3.cs"));

            _scriptGo = GameEngine.ScriptEngine.AddScript("scriptGo1", "GameTestScript");
            _scriptGo2 = GameEngine.ScriptEngine.AddScript("dataScript", "DataScript");
            //GameEngine.ScriptEngine.AddScript(_scriptGo, "test2", "TestScript3");
            //_scriptGo = GameEngine.GameObjectFactory.Create<ScriptableGameObject>("ScriptGo1", go =>
            //{
            //    go.ObjectType = ObjectType.Enemy;

            //    //testScript1.CurrentMenagedScript.SetInitalPosition(new Vector2(0, 50));
            //    //testScript1.CurrentMenagedScript.SetInitalColor(new Color(1.0f, 0.0f, 0.0f, .75f));
            //    //go.Scripts.Add(testScript1);

            //    //testScript2.CurrentMenagedScript.SetInitalPosition(new Vector2(0, 300));
            //    //testScript2.CurrentMenagedScript.SetInitalColor(new Color(1.0f, 0.0f, 1.0f, .25f));
            //    //go.Scripts.Add(testScript2);

            //    //testScript3.CurrentMenagedScript.SetInitalPosition(new Vector2(0, 550));
            //    //testScript3.CurrentMenagedScript.SetInitalColor(Color.Tomato);
            //    //go.Scripts.Add(testScript3);

            //    //gameScript.CurrentMenagedScript.SetInitalPosition(new Vector2(0,0));

            //    go.ScriptManager = gameScript;
            //});

            return true;
        }
        public void Render(IRenderEngine engine)
        {
            
            //engine.Render(_playerGo2);

            //foreach (var go in _gOs)
            //    engine.Render(go);

            //engine.Render(_playerGo);

            //_testView.Draw(engine);

            _scriptGo.Draw(engine);
            _scriptGo2.Draw(engine);
           // GameEngine.ScriptEngine.Update();

            //engine.DrawCircle(new Base.ValueObjects.Vector2(100, 100), 100, Color.Blue);

            //_questManager.Draw(GameEngine.RenderEngine);
        }

        public void Update(float delta)
        {
            //_playerGo.Update(delta);
            //_playerGo2.Update(delta);

            //foreach (var go in _gOs)
            //    go.Update(delta);

            //_testView.Update(delta);

           // _scriptGo.Update(delta);

            GameEngine.ScriptEngine.Update(delta);

            //_questManager.Update(delta);
        }

        public void Dispose()
        {
            _questManager.Dispose();
            _scriptGo.Dispose();
            GameEngine.ScriptEngine.Dispose();
        }

        internal class GameView : BaseGameView
        {
            public GameView(string screenName, Base.ValueObjects.Vector2 screenPosition,
                            Base.ValueObjects.Vector2 playerStartPosition = null)
                   : base(screenName, screenPosition, playerStartPosition)
            {
            }

            public override void OnDraw(IRenderEngine renderEngine)
            {
                foreach (var go in GameObjects)
                    go.Draw(renderEngine);
            }

            public override void OnUpdate(float dt)
            {
                foreach (var go in GameObjects)
                    go.Update(dt);
            }
        }
    }
}