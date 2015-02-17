using ZeldaEngine.Base;

namespace ZeldaEngine.Tests.Scripts
{
    public class TestEngineScript : GameScript
    {
        public void Run(string text)
        {
            Logger.LogWarning("Receive {0}", text);

            //if (Input.IsKeyDown(Keys.A))
            //    GameObject.Position.X += 21;
        }

        public void TestFunc()
        {
            Logger.LogError("Test Script Excuted!");
        }
    }
}