using ZeldaEngine.Game.ValueObjects;

namespace ZeldaEngine.Game.Abstracts
{
    public interface IUIContext
    {
        string ControlName { get; }

        UIState State { get; } 

        IUIElement Element { get; }
    }
}