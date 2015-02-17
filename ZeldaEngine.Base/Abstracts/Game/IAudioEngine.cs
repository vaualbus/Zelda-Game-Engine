namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IAudioEngine
    {
        void AddSong(string name);

        bool Play(string song);
    }
}