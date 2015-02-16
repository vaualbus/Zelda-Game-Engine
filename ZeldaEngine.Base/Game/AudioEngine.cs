using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Abstracts;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game
{
    public class AudioEngine : BaseGameClass, IAudioEngine
    {
        public AudioEngine(IGameEngine gameEngine) : 
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