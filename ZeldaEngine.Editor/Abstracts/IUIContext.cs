using ZeldaEngine.Game.ValueObjects;

namespace ZeldaEngine.Game.Abstracts
{
    public interface IUIContext
    {
        UIState State { get; } 

        IUIElement Sender { get; }
    }
}