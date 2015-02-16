namespace ZeldaEngine.Base.ValueObjects
{
    public class GameEviromentCollection
    {
        public Config Config { get; private set; }
        
        public GameEviromentCollection(Config gameConfig)
        {
            Config = Config;
        }
    }
}