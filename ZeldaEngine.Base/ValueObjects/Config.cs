namespace ZeldaEngine.Base.ValueObjects
{
    public class Config
    {
        public GameConfig GameConfig { get; private set; }

        public Config(GameConfig gameConfig)
        {
            GameConfig = gameConfig;
        }
    }
}