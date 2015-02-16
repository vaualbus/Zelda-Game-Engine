using ZeldaEngine.GameTest.Abstracts;
using ZeldaEngine.GameTest.ValueObjects;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.GameObjects
{
    public class TestGameObject : IGameObject
    {
        public string Name { get; private set; }
        
        public ScreenChar Texture { get; private set; }
       
        public Vector2 Position { get; set; }
        
        public Vector2 Rotation { get; set; }
        
        public Vector2 Scale { get; set; }
       
        public void Draw(IGameDrawer drawer)
        {
        }

        public IGameObject MoveTo(Vector2 newPos)
        {
            Position = newPos;
            return this;
        }
    }
}