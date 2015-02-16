using System;
using System.Collections.Generic;
using ZeldaEngine.GameTest.ValueObjects;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IScreen
    {
        IEnumerable<IGameComponent> GameComponents { get;  }

        IGameObjectFactory GameObjectFactory { get; }

        Vector2 ScreenPosition { get; set; }

        ScreenChar ScreenChar { get; set; }

        void Draw();
    }
}
