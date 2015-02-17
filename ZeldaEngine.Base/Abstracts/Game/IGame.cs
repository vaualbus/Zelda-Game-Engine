using System;

namespace ZeldaEngine.Base.Abstracts.Game
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