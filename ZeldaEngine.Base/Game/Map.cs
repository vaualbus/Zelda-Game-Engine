using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game
{
    public class Map
    {
        private readonly IGameEngine _gameEngine;

        public MapType MapType { get; private set; }

        public IEnumerable<IGameView> GameViews { get; private set; }

        private readonly Dictionary<string, string> _scriptFiles;

        public Map(IGameEngine gameEngine, Dictionary<string, string> scriptFiles)
        {
            _gameEngine = gameEngine;
            _scriptFiles = scriptFiles;
        }

        public void Draw(IRenderEngine renderEngine)
        {
            foreach (var view in GameViews)
            {
                view.Draw(renderEngine);
            }
        }

        public void Update(float dt)
        {
            foreach (var view in GameViews)
            {
                view.Update(dt);
            }
        }

        public Map CreateMap(MapDefinition mapDesc, Dictionary<string, string> scriptFileTable)
        {
            var screenPosX = 0;
            var screenPosY = 0;
            var screenPosition = new Dictionary<ScreenDefinition, Vector2>();
            
            for (var i = 0; i < mapDesc.Screens.Count(); i++)
            {
                screenPosition.Add(mapDesc.Screens.ToList()[i], new Vector2(screenPosX, screenPosY));
                if (screenPosX > 1600)
                {
                    screenPosY += 100;
                    screenPosX = 0;
                }
                screenPosX += 100;
            }

            var gameViews = new List<IGameView>();
            
            //Runtime Stuff
            foreach (var screenDescription in mapDesc.Screens)
            {
                var screen = new MapTempGameView(screenDescription.ScreenName, screenPosition[screenDescription], new Vector2(screenDescription.PlayerStartPositionX, screenDescription.PlayerStartPositionY));
                foreach (var enemyDescription in screenDescription.Enemies)
                {
                    var enemy = _gameEngine.GameObjectFactory.Find(enemyDescription.Name) as EnemyGameObject;
                    if (enemy == null)
                    {
                        _gameEngine.Logger.LogError("Invalid Enemy name, enemy {0} not found", enemyDescription.Name);
                        continue;
                    }

                    enemy.UpdateEnemy(enemy, enemyDescription);
                    
                    _gameEngine.GameObjectFactory.UpdateGameObject(enemy);

                    for (var i = 0; i < enemyDescription.Count; i++)
                        screen.GameObjects.Add(enemy);
                }

                if (screen.Scripts != null)
                {
                    foreach (var scriptType in screenDescription.Scripts)
                    {
                        var go = (GameObject) _gameEngine.GameObjectFactory.Find(scriptType.GameObjectName);
                        var createdScript = _gameEngine.ScriptEngine.AddScript(go, _scriptFiles, scriptType);

                        screen.Scripts.Add(createdScript);
                    }
                }
                foreach (var tile in screenDescription.Tiles)
                {
                    if (tile.GameObject != null)
                    {
                        //Update the game object info
                        var go = _gameEngine.GameObjectFactory.GetFromDefinition(tile.GameObject);
                        go.UpdateTypes(tile.GameObject.Properties);

                        var goName = string.Format("tile_{0}_{1}", tile.TilePositionX, tile.TilePositionY);
                        if (_gameEngine.GameObjectFactory.Find(goName) != null)
                            continue;

                        var tGo = _gameEngine.GameObjectFactory.Create<DrawableGameObject>(
                            string.Format("tile_{0}_{1}", tile.TilePositionX, tile.TilePositionY),
                            t =>
                            {
                                t.Position = new Vector2(tile.TilePositionX, tile.TilePositionY);
                                t.Tile = new Tile(_gameEngine, 0, 0, tile.LayerNumber)
                                {
                                    Texture = _gameEngine.Texture2DData(tile.TextureAssetName)
                                };
                                // t.ParentGameObjects.Add(go);
                            });

                        tGo.Init();

                        screen.GameObjects.Add(go);
                    }
                }

                gameViews.Add(screen);
            }

            GameViews = gameViews;

            return this;
        }

        internal class MapTempGameView : BaseGameView
        {
            public MapTempGameView(string screenName, Vector2 screenPosition, Vector2 playerStartPosition = null) 
                : base(screenName, screenPosition, playerStartPosition)
            {
            }
        }
    }
}