using System;
using ZeldaEngine.GameTest.ValueObjects;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.Abstracts
{
    public interface IGameDrawer
    {
        void PrintCharOnScreen(IGameObject charToPrint);

        bool Move(IGameObject @object, Vector2 newPos);
    }
}