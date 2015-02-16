namespace ZeldaEngine.Base.ValueObjects
{
    public class Config
    {
        public GameConfig GameConfig { get; private set; }

        public GameScriptConfig GameScriptConfig { get; private set; }

        public Config(GameScriptConfig gameScriptConfig, GameConfig gameConfig)
        {
            GameScriptConfig = gameScriptConfig;
            GameConfig = gameConfig;
        }
    }
}