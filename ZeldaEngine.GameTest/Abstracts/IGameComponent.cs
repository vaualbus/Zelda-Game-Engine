using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IGameComponent
    {
        string Name { get; }

        Vector2 Position { get; set; }

        void Draw(/* IGameDrawer drawer */);
    }
}
