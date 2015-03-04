namespace ZeldaEngine.Base.ValueObjects
{
    public class GameEviromentCollection
    {
        public GameConfig GameConfig { get; private set; }
        
        public GameEviromentCollection(GameConfig gameConfig)
        {
            GameConfig = gameConfig;
        }
    }
}