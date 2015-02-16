using System;
using System.IO;
using ZeldaEngine.Base;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.SharpDx;


namespace ZeldaEngine.SharpDXTest
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

            var scriptConfig = new GameScriptConfig(gameConfig.BaseDirectory, ConfigurationManager.GetDefaultProjectDirectory());
            var config = new Config(scriptConfig, gameConfig);

            using (var app = new SharpDxCoreEngine(new ZeldaGame(), config, new GameLogger(config)))
                app.Run();
        }
    }
}