namespace ZeldaEngine.Base.Game.Abstracts
{
    public interface IAudioEngine
    {
        void AddSong(string name);

        bool Play(string song);
    }
}