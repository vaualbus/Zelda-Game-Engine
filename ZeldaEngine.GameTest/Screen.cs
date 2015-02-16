using System.Linq;
using ZeldaEngine.GameTest.Abstracts;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest
{
    public class ScreenBase : IScreen
    {
        public IGameObjectFactory GameObjectFactory { get; private set; }
        
        public Vector2 ScreenPosition { get; set; }

        public virtual void Draw(IGameDrawer drawer)
        {
            foreach (var go in GameObjectFactory.GameObjects)
            {
                var components = GameObjectFactory.Find(go);
                IGameObject tempGO = null;
                foreach (var gameComponent in components)
                   tempGO = gameComponent.Action(go);

                (tempGO ?? go).Draw(drawer);
            }
        }
    }
}