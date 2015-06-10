using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game.GameEngineClasses
{
    public abstract class GameEngine : IGameEngine
    {
        public GameConfig GameConfig { get; set; }

        public IInputManager InputManager { get; set; }

        public IRenderEngine RenderEngine { get; set; }

        public IScriptEngine ScriptEngine { get; set; }

        public IAudioEngine AudioEngine { get; set; }

        public IContentLoader ResourceLoader { get; protected set; }

        public ILogger Logger { get; private set; }

        public IGameObjectFactory GameObjectFactory
        {
            get; set;
        }

        public IResourceData Texture2DData(string assetName)
        {
            return null;
        }

        public GameEngine(ILogger logger)
        {
            Logger = logger;
        }
    }
}