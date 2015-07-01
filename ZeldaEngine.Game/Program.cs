using System;
using System.IO;
using ZeldaEngine.Base;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.SharpDxImp;

namespace ZeldaEngine.Game
{
    /// <summary>
    /// Simple ZeldaEngineSharpDXTest application using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
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
                gameConfig = new GameConfig("Zelda Engine", 300, 200, AppDomain.CurrentDomain.BaseDirectory);
                ConfigurationManager.CreateConfiguration(gameConfig);
            }

            using (var app = new SharpDxCoreEngine(new ZeldaGame(), gameConfig, new GameLogger(gameConfig)))
                app.Run();
        }
    }
}