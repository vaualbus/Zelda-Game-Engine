using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;

namespace ZeldaEngine.Tests.GameTests
{
    public class TestGame : IGame
    {
        public IGameEngine GameEngine { get; set; }

        public bool Init()
        {
            return true;
        }

        public void HandleInput(float delta)
        {
        }

        public void Update(float delta)
        {
        }

        public void Render(IRenderEngine engine)
        {
        }

        public void Dispose()
        {
        }
    }
}
