namespace ZeldaEngine.Base.ValueObjects
{
    public class GameConfig
    {
        public string OpenGLVersion { get; set; }
        
        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public string Title { get; private set; }

        public string BaseDirectory { get; private set; }

        public string ResourceDirectory { get; private set; }
        public string QuestDirectory { get; set; }
        public double Framerate { get; private set; }

        public string ScriptDirectory { get; private set; }

        public Vector2 ScreenPosition { get; private set; }



        public GameConfig(string title, int width = 1024, int height = 768,
                          string baseDirectory = "", 
                          string resourceDirectory = ConfigurationManager.DefaultResourcePath,
                          string questDirectory = ConfigurationManager.DefaultQuestsPath,
                          string scriptDirectory = ConfigurationManager.DefaultScriptPath,
                          int frameRate = 60)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            Title = title;
            BaseDirectory = baseDirectory;
            ResourceDirectory = resourceDirectory;
            QuestDirectory = questDirectory;
            Framerate = frameRate;
            ScriptDirectory = scriptDirectory;
        }
    }
}