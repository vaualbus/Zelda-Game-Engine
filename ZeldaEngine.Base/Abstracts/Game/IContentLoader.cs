namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IContentLoader
    {
        TData Load<TData>(string assetName);

        void Update();
    }
}