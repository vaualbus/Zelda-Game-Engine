using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.MapLoaders;
using ZeldaEngine.ScriptEngine;

namespace ZeldaEngine.Game
{
    public class ZeldaGame : IGame
    {
        private GameScriptEngine _scriptEngine;

        private QuestManager _questManager;
        private List<GameObject> _gOs;
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

            _scriptEngine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));
            _scriptEngine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(Expression<>)));

            _scriptGo = GameEngine.ScriptEngine.AddScript("scriptGo1", "GameTestScript");
            _scriptGo2 = GameEngine.ScriptEngine.AddScript("dataScript", "DataScript");

            return true;
        }

        public void TestScreenGrid()
        {
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
        }

        public void Render(IRenderEngine engine)
        {
            _scriptGo.Draw(engine);
            _scriptGo2.Draw(engine);
         
            //GameEngine.ScriptEngine.Update();

            _questManager.Draw(GameEngine.RenderEngine);
        }

        public void Update(float delta)
        {
            GameEngine.ScriptEngine.Update(delta);

            _questManager.Update(delta);
        }

        public void Dispose()
        {
            _questManager.Dispose();
            _scriptGo.Dispose();

            GameEngine.ScriptEngine.Dispose();
        }
    }
}