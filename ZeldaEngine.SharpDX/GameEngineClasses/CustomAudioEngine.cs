using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomAudioEngine : IAudioEngine
    {
        private readonly IGameEngine _gameEngine;

        public CustomAudioEngine(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public void AddSong(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool Play(string song)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}