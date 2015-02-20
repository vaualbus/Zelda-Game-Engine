using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.MapLoaders;
using ZeldaEngine.Base.Services;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.ScriptEngine;
using ZeldaEngine.SharpDx;
using ZeldaEngine.SharpDx.GameEngineClasses;
using Vector2 = SharpDX.Vector2;

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
        public IGameEngine GameEngine { get; set; }


        public void HandleInput(float delta)
        {
        }

        public bool Init()
        {
            _gOs = new List<GameObject>();

            _scriptEngine = new GameScriptEngine(GameEngine.Configuration, GameEngine);
            _scriptEngine.InitializeEngine();

            GameEngine.ScriptEngine = _scriptEngine;

            _questManager = new QuestManager(GameEngine, new JsonMapLoader(GameEngine));
            _questManager.LoadQuest("TestQuest");

            _playerGo = GameObjectFactory.Create<PlayerGameObject>("Player", g =>
             {
                 g.Tile = new Tile(GameEngine, 50, 50)
                 {
                     Texture = GameEngine.TextureData("Default")
                 };

                 g.Position = new Base.ValueObjects.Vector2(10, 10);
                 g.MoveVelocity = 50f;
             });

            const int sizeX = 1024;
            const int sizeY = 768;
            const int tileWidth = 50;
            const int tileHeight = 50;
            const int spacing = 100;
            const int spacingLeft = 200;
            const int spacingTop = 200;

            for (var x = 0; x < sizeX; x += tileWidth + spacing)
            {
                for (var y = 0; y < sizeY; y += tileHeight + spacing)
                {
                    var _x = x;
                    var _y = y;

                    _gOs.Add(GameObjectFactory.Create<DrawableGameObject>(string.Format("Test_{0}_{1}", _x, _y), g =>
                    {
                        g.Tile = new Tile(GameEngine, tileWidth, tileHeight)
                        {
                            Texture = GameEngine.TextureData("Grass")
                        };

                        g.Position = new Base.ValueObjects.Vector2(_x == 0 ? _x + spacingLeft : _x, _y == 0 ? _y + spacingTop : _y);
                    }));
                }
            }

            _testView = new GameView("Test", new Base.ValueObjects.Vector2(0, 0), new Base.ValueObjects.Vector2(0, 0));
            _testView.GameObjects.Add(_playerGo);
            _testView.GameObjects.AddRange(_gOs);

            _scriptEngine.ScriptCompiler.AdditionalAssemblies.Add(Assembly.GetAssembly(typeof(System.Drawing.Color)));

            var testScript = CreateScript("TestScript", Path.Combine(GameEngine.Configuration.GameConfig.ScriptDirectory, "TestScript.cs"));
            _scriptGo = GameObjectFactory.Create<ScriptableGameObject>("ScriptGo1", go =>
            {
                go.ScriptParamProvider = _scriptEngine.ParamsProvider;
                go.ObjectType = ObjectType.Enemy;

                go.Scripts.Add(testScript);
            });

            return true;
        }
        public void Render(IRenderEngine engine)
        {
            
            //engine.Render(_playerGo2);

            //foreach (var go in _gOs)
            //    engine.Render(go);

            //engine.Render(_playerGo);

            _testView.Draw(engine);

            _scriptGo.Draw(engine);

            //_questManager.Draw(GameEngine.RenderEngine);
        }

        public void Update(float delta)
        {
#if ENABLE_SCRIPT
            foreach (var view in _questManager.LoadedQuest.Maps.SelectMany(map => map.GameViews))
                GameEngine.ScriptEngine.Update(view, delta);
#endif
            //_playerGo.Update(delta);
            //_playerGo2.Update(delta);

            //foreach (var go in _gOs)
            //    go.Update(delta);

            _testView.Update(delta);

            _scriptGo.Update(delta);

            _questManager.Update(delta);
        }

        /// <summary>
        /// TODO(albus95): Dummy function to test script working in game.
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="scriptFilename"></param>
        /// <returns></returns>
        private ScriptManager CreateScript(string scriptName, string scriptFilename)
        {
            var script = _scriptEngine.ScriptCompiler.Compile(scriptFilename);

            var scriptManager = new ScriptManager(_scriptEngine, _scriptEngine.ScriptCompiler, _scriptEngine.ScriptRepository, null,
                new InternalScriptActivator(), _scriptEngine.Logger);

            scriptManager.AddScript(null, script, scriptName);

            return scriptManager;
        }

        public void Dispose()
        {
            _questManager.Dispose();
            //_scriptEngine.Dispose();
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