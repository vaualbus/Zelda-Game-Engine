using System;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Game.Abstracts
{
    public interface IGame : IDisposable
    {
        IGameEngine GameEngine { get; set; }

        void HandleInput(float delta);

        void Update(float delta);

        void Render(IRenderEngine engine);

        bool Init();
    }
}