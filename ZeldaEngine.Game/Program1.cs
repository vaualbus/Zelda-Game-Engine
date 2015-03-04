using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.OpenGL.Game;

namespace ZeldaEngine.Game
{
    public class Program1
    {
        public static void Main()
        {
            GameConfig gameConfig;
            if (File.Exists(ConfigurationManager.ConfigurationFileName))
            {
                if (!ConfigurationManager.CheckEngineDirectory())
                    ConfigurationManager.UpdateEngineDirectory();

                gameConfig = ConfigurationManager.GetConfiguration();
            }
            else
            {
                //We need to create the config file than save the configuration.
                gameConfig = new GameConfig("Zelda Engine", 1024, 768, AppDomain.CurrentDomain.BaseDirectory);
                ConfigurationManager.CreateConfiguration(gameConfig);
            }

//#if DEBUG
//            gameConfig = new GameConfig("My Zelda Classic", 1024, 768, AppDomain.CurrentDomain.BaseDirectory, @".\Resources", 30);
           
//#else
//            var gameConfig = new GameConfig("My Zelda Classic", 1024, 768, AppDomain.CurrentDomain.BaseDirectory, @".\Resources", 30);
//#endif
            using (var game = new CoreEngine(new ZeldaGame(), gameConfig))
            {
                game.CreateWindow();
                game.Start();
            }
        }
    }
}