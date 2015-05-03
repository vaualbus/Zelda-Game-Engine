using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests
{
    public class TestGameEngine : IGameEngine
    {
        public IAudioEngine AudioEngine { get; set; }

        public GameConfig GameConfig { get; set; }

        public IGameObjectFactory GameObjectFactory { get; set; }

        public IInputManager InputManager { get; set; }

        public ILogger Logger { get; }

        public IRenderEngine RenderEngine { get; set; }

        public IContentLoader ResourceLoader { get; }

        public IScriptEngine ScriptEngine { get; set; }

        public TestGameEngine(GameConfig gameConfig, ILogger logger)
        {
            GameConfig = gameConfig;
            Logger = logger;
            GameObjectFactory = new GameObjectFactory(this);

        }

        public IResourceData TextureData(string assetName)
        {
            throw new NotImplementedException();
        }
    }
}
