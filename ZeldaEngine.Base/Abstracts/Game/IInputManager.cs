using System;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IInputManager
    {
        Vector2 MousePosition { get; }

        bool IsKeyUp(GameKeys key);

        bool IsKeyDown(GameKeys key);

        bool IsMouseButtonPressed(string button);

        bool IsMouseButtonReleased(string button);

        void Update();
    }
}