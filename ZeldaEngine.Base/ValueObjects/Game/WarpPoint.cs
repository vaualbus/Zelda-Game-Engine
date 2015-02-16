using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Base.ValueObjects.Game
{
    public class WarpPoint
    {
        Vector2 Position { get; set; }

        IGameView ToScreen { get; set; }


    }
}