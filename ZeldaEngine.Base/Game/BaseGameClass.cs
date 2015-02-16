using System;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.Game
{
    public abstract class BaseGameClass : IDisposable
    {
        protected IGameEngine GameEngine { get; private set; }

        protected BaseGameClass(IGameEngine gameEngine)
        {
            GameEngine = gameEngine;
        }

        public virtual void Dispose() { }
    }
}