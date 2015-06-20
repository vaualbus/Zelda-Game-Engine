using ZeldaEngine.Base;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests.Scripts
{
    public class AttachPropScript : GameScript
    {

        public int XPos { get; private set; }

        public int YPos { get; private set; }

        public DrawableGameObject TestGo { get; private set; }

        public override void Init()
        {
            Logger.LogInfo("Initializing the script");

            TestGo = GameObjectFactory.Create<DrawableGameObject>("test", g =>
            {
                g.Position = new Vector2(20, 20);
                g.Tile.Width = -1;
                g.Tile.Height = -50;
            })
            .AttachProperty(g => g.Tile.Width, () => XPos, this)
            .AttachProperty(g => g.Tile.Height, () => YPos, this);
        }

        public void Run()
        {
            Logger.LogInfo("Updating the script");

            TestGo.Update(CurrentTime);
        }

        public override void OnDraw()
        {
            TestGo.Draw(RenderEngine);
        }
    }
}