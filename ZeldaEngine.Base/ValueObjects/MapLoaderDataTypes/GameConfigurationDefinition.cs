namespace ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes
{
    public class GameConfigurationDefinition
    {
        public string BaseDirectory { get; private set; }

        public string DefaultProjectDirectory { get; private set; }

        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public InputConfigurationDefinition InputConfiguration { get; private set; }

        public GameConfigurationDefinition(string baseDirectory, int screenWidth, int screenHeight,
                                           string defaultProjectDirectory = "", InputConfigurationDefinition inputConfiguration = null)
        {
            BaseDirectory = baseDirectory;
            DefaultProjectDirectory = defaultProjectDirectory;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            InputConfiguration = inputConfiguration;
        }
    }
}