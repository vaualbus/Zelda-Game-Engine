using System.Collections.Generic;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IGameComponent
    {
        string Name { get; set; }

        //IGameComponent Parent { get; set; }

        bool Action(IGameObject gameObject);

        //void SetParent(IGameComponent component);

        IGameEngine GameEngine { get; set; }
    }
}