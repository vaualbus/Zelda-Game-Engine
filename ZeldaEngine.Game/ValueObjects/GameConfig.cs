namespace ZeldaEngine.Game.ValueObjects
{
    public class GameConfig
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public string Title { get; private set; }

        public string BaseDirectory { get; private set; }

        public string ResourceDirectory { get; private set; }

        public GameConfig(string title, int width, int height, string baseDirectory, string resourceDirectory)
        {
            Width = width;
            Height = height;
            Title = title;
            BaseDirectory = baseDirectory;
            ResourceDirectory = resourceDirectory;
        }
    }
}