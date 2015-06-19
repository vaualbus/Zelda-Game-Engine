using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Tests.TestRig
{
    public class InMemoryResourceLoader : IContentLoader
    {
        private readonly IGameEngine _gameEngine;

        public InMemoryResourceLoader(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public TData Load<TData>(string assetName)
        {
            _gameEngine.Logger.LogInfo("Loading asset: {0}", assetName);
            return default(TData);
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}