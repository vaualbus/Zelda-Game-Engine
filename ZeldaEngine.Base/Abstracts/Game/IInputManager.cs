using System;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IInputManager
    {
        Vector2 MousePosition { get; }

        bool IsKeyUp<TData>(TData data) where TData : struct, IConvertible;

        bool IsKeyDown<TData>(TData data) where TData : struct, IConvertible;

        bool IsKeyUp(string keyName);

        bool IsKeyDown(string keyName);

        bool IsMouseButtonPressed(string button);

        bool IsMouseButtonReleased(string button);
    }
}