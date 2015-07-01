using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game
{
    public class DefaultAudioEngine : BaseGameClass, IAudioEngine
    {
        public DefaultAudioEngine(IGameEngine gameEngine) : 
            base(gameEngine)
        {
            GameEngine.Logger.LogInfo("IAudio Engine Initialized");
        }

        public void AddSong(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool Play(string song)
        {
            throw new System.NotImplementedException();
        }
    }
}