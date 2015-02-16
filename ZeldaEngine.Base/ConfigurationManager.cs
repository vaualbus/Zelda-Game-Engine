using System;
using System.IO;
using Newtonsoft.Json;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base
{
    public static class ConfigurationManager
    {

#region Default Engine Configuration

        public const string QuestFileExtension = "json";

        public const string DefaultApplicationTitle = "Better Zelda Engine";
        public const string ConfigurationFileName = "Config.json";

        public const int DefaultWindowHeight = 1024;
        public const int DefaultWindowWidth = 768;

        public const string DefaultResourcePath = "Resources";
        public const string DefaultQuestsPath = "Quests";
        public const string DefaultScriptPath = "Scripts";
        public const string DefaultProjectDir = "Projects";

        public const string DefaultUpKey = "Up";
        public const string DefaultDownKey = "Down";
        public const string DefaultLeftKey = "Left";
        public const string DefaultRightKey = "Right";
        public const string DefaultAKey = "A";
        public const string DefaultBKey = "B";
        public const string DefaultSpaceKey = "Space";
        public const string DefaultQKey = "Q";

#endregion

        public static GameConfig GetConfiguration()
        {
            using (var ms = new StreamReader(ConfigurationFileName))
            {
                var config = JsonConvert.DeserializeObject<GameConfigurationDefinition>(ms.ReadToEnd());
                ms.Close();

                return new GameConfig(DefaultApplicationTitle,
                    config.ScreenWidth <= 0 ? DefaultWindowWidth : config.ScreenWidth,
                    config.ScreenHeight <= 0 ? DefaultWindowHeight : config.ScreenHeight, config.BaseDirectory,
                    DefaultResourcePath, DefaultQuestsPath, DefaultScriptPath);
            }
        }

        internal static GameConfigurationDefinition GetConfigurationDefinition()
        {
            using (var ms = new StreamReader(ConfigurationFileName))
                return JsonConvert.DeserializeObject<GameConfigurationDefinition>(ms.ReadToEnd());
        }

        public static void CreateConfiguration(GameConfig gameConfig)
        {
            var configuration = new GameConfigurationDefinition(gameConfig.BaseDirectory, gameConfig.ScreenWidth,
                gameConfig.ScreenHeight, DefaultProjectDir,
                new InputConfigurationDefinition(DefaultUpKey, DefaultDownKey, DefaultLeftKey, DefaultRightKey,
                    DefaultAKey, DefaultBKey, DefaultSpaceKey, DefaultQKey));

            var text = JsonConvert.SerializeObject(configuration);
            using (var fw = new FileStream(ConfigurationFileName, FileMode.Create, FileAccess.Write))
            using (var sr = new StreamWriter(fw))
                sr.Write(text);

            GetDefaultProjectDirectory();
        }

        public static string GetDefaultProjectDirectory()
        {
            using (var sr = new StreamReader(ConfigurationFileName))
            {
                var config = JsonConvert.DeserializeObject<GameConfigurationDefinition>(sr.ReadToEnd());

                if (config.DefaultProjectDirectory == string.Empty)
                {
                    var projDir = Path.Combine(GetConfiguration().BaseDirectory, DefaultProjectDir);
                    if (!Directory.Exists(projDir))
                        Directory.CreateDirectory(projDir);

                    config = new GameConfigurationDefinition(config.BaseDirectory, config.ScreenWidth, config.ScreenHeight, projDir, config.InputConfiguration);

                    sr.Close();
                       
                    var text = JsonConvert.SerializeObject(config);
                    File.WriteAllText(ConfigurationFileName, text);

                    return projDir;
                }

                return config.DefaultProjectDirectory;
            }
        }

        public static InputConfigurationDefinition GetInputConfiguration()
        {
            using (var ms = new StreamReader(ConfigurationFileName))
            {
                var config = JsonConvert.DeserializeObject<GameConfigurationDefinition>(ms.ReadToEnd());
                ms.Close();

                if (config.InputConfiguration != null)
                    return config.InputConfiguration;
            }

            return null;
        }

        public static bool CheckEngineDirectory()
        {
            return GetConfiguration().BaseDirectory == AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void UpdateEngineDirectory()
        {
            using (var sr = new StreamReader(ConfigurationFileName))
            {
                var config = JsonConvert.DeserializeObject<GameConfigurationDefinition>(sr.ReadToEnd());

                config = new GameConfigurationDefinition(AppDomain.CurrentDomain.BaseDirectory, config.ScreenWidth, config.ScreenHeight,
                   Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultProjectDir), config.InputConfiguration);

                sr.Close();

                var text = JsonConvert.SerializeObject(config);
                File.WriteAllText(ConfigurationFileName, text);
            }
        }
    }
}