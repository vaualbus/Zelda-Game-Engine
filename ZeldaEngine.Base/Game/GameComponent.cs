using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.RenderingEngine;

namespace ZeldaEngine.Base.Game
{
    public abstract class GameComponent : IGameComponent
    {
        public string Name { get; set; }

       // public IGameComponent Parent { get; set; }

        public IGameEngine GameEngine { get; set; }

        public IRenderEngine RenderEngine => GameEngine.RenderEngine;

        public IInputManager InputManager => GameEngine.InputManager;

       
        public abstract bool Action(IGameObject gameObject);


        //public void SetParent(IGameComponent component)
        //{
        //    Parent = component;
        //}
    }
}