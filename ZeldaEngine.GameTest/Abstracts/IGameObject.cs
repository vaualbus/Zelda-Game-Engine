using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaEngine.GameTest.ValueObjects;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IGameObject
    {
        string Name { get; }

        //The element to draw
        ScreenChar Texture { get; }

        Vector2 Position { get; set; }

        Vector2 Rotation { get; set; }

        Vector2 Scale { get; set; }

        void Draw(IGameDrawer drawer);

        IGameObject MoveTo(Vector2 newPos);
    }
}
