using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Tests.TestRig
{
    public class InMemoryResourceData : IResourceData
    {
        public int Width { get; }
        public int Height { get; }
        public object Value { get; }

        public InMemoryResourceData(IGameEngine gameEngine, string name)
        {
            gameEngine.Logger.LogInfo("InMemoryTexture: {0}", name);
        }
    }
}