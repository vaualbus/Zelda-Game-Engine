using System;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IAudioEngine : IDisposable
    {
        void AddSong(string name);

        bool Play(string song);
    }
}