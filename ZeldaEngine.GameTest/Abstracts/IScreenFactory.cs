using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeldaEngine.GameTest.ValueObjects;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IScreenFactory
    {
        IScreen Create(string name, Vector2 screenPos,
                       ScreenChar @char,  IScreen parent = null,
                       IEnumerable<IGameComponent> compoents = null);

        IScreen Find(string name);

        IEnumerable<IScreen> Find(Vector2 pos);

        void Delete(string name);
    }
}
