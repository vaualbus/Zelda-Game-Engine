using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Abstracts;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomAudioEngine : IAudioEngine
    {
        public CustomAudioEngine(IGameEngine gameEngine)
        {
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