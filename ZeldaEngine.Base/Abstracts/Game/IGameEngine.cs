using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface  IGameEngine
    {
        GameConfig GameConfig { get; set; }

        IInputManager InputManager { get; set; }

        IRenderEngine RenderEngine { get; set; }

        IScriptEngine ScriptEngine { get; set; }

        IAudioEngine AudioEngine { get; set; }

        IContentLoader ResourceLoader { get; }

        ILogger Logger { get; }

        IGameObjectFactory GameObjectFactory { get; set; }

        IResourceData Texture2DData(string assetName);

        void ExitGame();
    }
}