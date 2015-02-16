using System;
using ZeldaEngine.Base.Game;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface IInputManager
    {
        bool IsKeyUp<TData>(TData data) where TData : struct, IConvertible;

        bool IsKeyDown<TData>(TData data) where TData : struct, IConvertible;

        bool IsKeyUp(string keyName);

        bool IsKeyDown(string keyName);
    }
}