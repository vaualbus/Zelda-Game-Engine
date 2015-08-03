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

        public virtual void AddSong(string name)
        {
            GameEngine.Logger.LogInfo("Default Audio Engine adding {0}", name);
        }

        public virtual bool Play(string song)
        {
            GameEngine.Logger.LogInfo("Default Audio Engine: Playing {0}", song);
            return true;
        }
    }
}