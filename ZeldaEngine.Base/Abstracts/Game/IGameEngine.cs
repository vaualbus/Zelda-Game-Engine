using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.Abstracts;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameEngine
    {
        Config Configuration { get; set; }

        IInputManager InputManager { get; set; }

        IRenderEngine RenderEngine { get; set; }

        IScriptEngine ScriptEngine { get; set; }

        IAudioEngine AudioEngine { get; set; }

        IContentLoader ResourceLoader { get; }

        ILogger Logger { get; }

        IResourceData TextureData(string assetName);
    }
}