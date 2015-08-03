using System;

namespace ZeldaEngine.Base.ValueObjects.Game
{
    [Flags]
    public enum MovableDirection
    {
       Up,
       Down,
       Left,
       Right,
       UpDown,
       LeftRight,
       UDLR,
       All,
    }
}