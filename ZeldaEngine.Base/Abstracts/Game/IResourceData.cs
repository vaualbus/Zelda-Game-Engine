namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IResourceData
    {
        int Width { get; }

        int Height { get; }

        object Value { get;  } 
    }
}