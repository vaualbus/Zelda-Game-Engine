using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class ScriptableGameObject : GameObject
    {
        public ObjectType ObjectType { get; set; }

        public ScriptType ScriptType { get; set; } 

        public IScriptManager ScriptManager { get; set; }

        public GameObject GameObject { get; set; }

        public ScriptableGameObject(IGameEngine gameEngine) 
            : base(gameEngine)
        {
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            ScriptManager.CurrentMenagedScript.Draw();
        }

        protected override void OnUpdate(float dt)
        {
            if (ScriptManager == null)
                return;

            base.OnUpdate(dt);
        }
    }
}