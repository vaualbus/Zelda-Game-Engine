using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameEngineClasses;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Game.MapLoaders
{
    public class MyMapLoader : IQuestLoader
    {
        private readonly IGameEngine _gameEngine;

        // [screen number, screen name, screen type, song screen name/id, color palette, intro text]
        // Types: 
        //		0 solid
        //		1 walkable
        //		2 enemy
        //		3 teleporter
        //		4 block
        //		5 entrance/exit
        // game object name:
        //		drGO => drawable game object
        //		enGO => enemy game object
        //		....

        public MyMapLoader(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        // [screen number, screen name, screen type, song screen name/id, color palette, intro text]
        // x,y-type-{ R, G, B, A : COLOR DEFAULT => not care }-layer number-texture name-game object name/id [go properties, in_class_prop_name => value]-script name-{ script params [ type => value ] }-
        public QuestDefinition Load(string filePath)
        {
            var sr = new StreamReader(filePath);
            var loadedScreens = new List<ScreenDefinition>();
            var loadedTiles = new List<TileDefinition>();
            int screenLoadedCount = 0;
            var mapName = "";
            var spawnPos = new Vector2();
            var subScreenName = "";

            try
            {
                var line = "";
                var isMapInfoLoaded = false;

                while ((line = sr.ReadLine()) != null)
                {
                    var screenType = MapType.Empty;
                    var songName = string.Empty;
                    var introText = string.Empty;
                    var tileColor = "";
                    var gameObjectName = string.Empty;
                    var scriptName = string.Empty;
                    var colorPalette = 0;

                    if (line.StartsWith("//") || line.StartsWith("/*"))
                        continue;

                    if (line == " ")
                        continue;

                    if (line == string.Empty)
                        continue;

                    if (line.StartsWith("{") && line.EndsWith("}"))
                    {
                        //load the map Initial data
                        //{mapName-LinkStartPosition-subScreenName}
                        var args = line.Split('-');

                        mapName = args[0].Replace("{", string.Empty);
                        spawnPos = Vector2.Parse(args[1]);
                        subScreenName = args[2].Replace("}", string.Empty);

                        isMapInfoLoaded = true;
                    }
                    else if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        //We need to parse the screen info
                        var args = line.Split('[')[1].Replace("]", string.Empty);
                        var mapArguments = args.Split(',');
                        var screenNumber = int.Parse(mapArguments[0]);
                        var screenName = mapArguments[1];

                        if (mapArguments.Length > 2)
                        {
                            MapType.TryParse(mapArguments[2], out screenType);
                            if (mapArguments.Length > 3)
                            {
                                songName = mapArguments[3];
                                if (mapArguments.Length > 4)
                                {
                                    colorPalette = int.Parse(mapArguments[4]);
                                    if (mapArguments.Length > 5)
                                    {
                                        introText = mapArguments[5];
                                    }
                                }
                            }
                        }

                        loadedScreens.Add(new ScreenDefinition(screenNumber, screenName, screenType, 0, 0, songName,
                                                                colorPalette, introText, loadedTiles, null, null));
                        loadedTiles.Clear();

                        screenLoadedCount++;
                    }
                    else
                    {
                        var gameObjectProp = new Dictionary<string, object>();
                        var gameScriptParams = new Dictionary<string, object>();

                        //Now we parse each tile description
                        // x,y-type-{ R, G, B, A : COLOR DEFAULT => not care }-layer number-texture name-game object name/id {go properties, in_class_prop_name => value}-script name-{ script params [ type => value ] }-
                        var args = line.Split('-');
                        var tilePos = Vector2.Parse(args[0]);
                        TileType tileType;
                        TileType.TryParse(args[1], out tileType);

                        if (args[2].StartsWith("{") && args[2].EndsWith("}"))
                            ParseColor(args[2], out tileColor);

                        var layerNumber = args.Length > 2 ? int.Parse(args[3]) : 0;
                        var textureAssetName = args.Length > 3 ? args[4] : "";

                        if (args.Length > 4)
                        {
                            ParseGameObject(args[4], out gameObjectName, gameObjectProp);

                            if (args.Length > 5)
                                ParseScript(args[5], out scriptName, gameScriptParams);
                        }
                        //loadedTiles.Add(new TileDefinition(tilePos.X, tilePos.Y, textureAssetName, 
                        //                                    tileType, tileColor, layerNumber, 
                        //                                    new GameObjectDefinition(gameObjectName, gameObjectProp),
                        //                                    new GameScriptDefinition(scriptName, gameScriptParams)));
                    }

                    if (!isMapInfoLoaded)
                        throw new Exception("You haven't provide the map information");
                }
            }
            catch (Exception ex)
            {
               _gameEngine.Logger.LogError("An error has occuring while loading the map file: {0}", ex.Message);
#if DEBUG
                throw new Exception(ex.Message);
#endif
            }

            _gameEngine.Logger.LogInfo("Loaded: {0} screens. Info: {1}", screenLoadedCount,
                string.Join(", ", loadedScreens.Select(t => t.Id)));

            var loadedMap = new MapDefinition(mapName, spawnPos.X, spawnPos.Y, subScreenName, loadedScreens);

            //Tha we will loop through the loaded objects and intentiad the appropiate tiles/ game object and set the properties
            var gameViews = new List<IGameView>();
            foreach (var screen in loadedMap.Screens)
            {
            }

            return null;
        }

        public void Draw()
        {
        }

        public void Update(float dt)
        {
        }

        private void ParseGameObject(string args, out string gameObjectName, Dictionary<string, object> gameObjectProp)
        {
            //game object name/id {go properties, in_class_prop_name => value}
            var gameObjectArgs = args.Split('{');
            gameObjectName = gameObjectArgs[0];

            if (gameObjectArgs.Length > 1)
                ParseParmaterizedValues(gameObjectProp, gameObjectArgs);
        }

        private void ParseScript(string args, out string scriptName, Dictionary<string, object> gameScriptParams)
        {
            //script name-{ script params [ type => value ]
            var scriptObjectArgs = args.Split('{');
            scriptName = scriptObjectArgs[0];

            if (scriptObjectArgs.Length > 1)
                ParseParmaterizedValues(gameScriptParams, scriptObjectArgs);
        }

        private static void ParseParmaterizedValues(Dictionary<string, object> gameObjectProp, string[] gameObjectArgs)
        {
            var valuess = gameObjectArgs[1].Split(',').Select(t => t.Split(';')).ToList();

            var correctProperties = (from values in valuess from value in values select value.Split('=', '>'))
                .ToDictionary(k => k[0].Replace(" ", string.Empty), v => v[2].Replace("}", string.Empty));

            foreach (var correctValue in correctProperties)
            {
                if (correctValue.Value.EndsWith("d"))
                {
                    gameObjectProp.Add(correctValue.Key,
                        ParseValue(typeof(double), correctValue.Value.Replace("d", string.Empty)));
                }
                else if (correctValue.Value.EndsWith("f"))
                {
                    gameObjectProp.Add(correctValue.Key, ParseValue(typeof(float), correctValue.Value.Replace("f", string.Empty)));
                }
                else if (correctValue.Value.Contains("."))
                {
                    gameObjectProp.Add(correctValue.Key, ParseValue(typeof(float), correctValue.Value));
                }
                else if (correctValue.Value.Replace(" ", string.Empty) == "true")
                    gameObjectProp.Add(correctValue.Key, true);
                else if (correctValue.Value.Replace(" ", string.Empty) == "false")
                    gameObjectProp.Add(correctValue.Key, false);
                else if (correctValue.Value.StartsWith(" \"") && correctValue.Value.StartsWith(" \""))
                    gameObjectProp.Add(correctValue.Key, correctValue.Value);
                else if (!correctValue.Value.Contains("."))
                {
                    gameObjectProp.Add(correctValue.Key, ParseValue(typeof(int), correctValue.Value));
                }
            }
        }

        private static object ParseValue(Type type, string value)
        {
            if (type == typeof(float))
                return float.Parse(value, CultureInfo.InvariantCulture);

            if (type == typeof(double))
                return double.Parse(value, CultureInfo.InvariantCulture);

            if (type == typeof(float))
                return float.Parse(value, CultureInfo.InvariantCulture);

            var hasParser = type.GetMethod("Parse", new[] { typeof(string) });
            if (hasParser != null)
                return hasParser.Invoke(null, new object[] { value });
            return null;
        }

        private void ParseColor(string colorString, out string tileColor)
        {
            var color = colorString.Replace("{", string.Empty).Replace("}", string.Empty);
            //if (color == "COLOR DEFAULT")
            //{
            //    tileColor = Color.White;
            //    return;
            //}

            //var colorArgs = color.Split(',');
            //var r = int.Parse(colorArgs[0]);
            //var g = int.Parse(colorArgs[1]);
            //var b = int.Parse(colorArgs[2]);
            //var a = colorString.Length > 3 ? int.Parse(colorArgs[3]) : 0;

            tileColor = color;
        } 
    }
}