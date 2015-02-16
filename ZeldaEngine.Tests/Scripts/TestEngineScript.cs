using ZeldaEngine.Base;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests.Scripts
{
    public class TestEngineScript : GameScript
    {
        public void Run(string text)
        {
            _logger.LogWarning("Receive {0}", text);

            //if (Input.IsKeyDown(Keys.A))
            //    GameObject.Position.X += 21;
        }

        public void TestFunc()
        {
            _logger.LogError("Test Script Excuted!");
        }
    }
}