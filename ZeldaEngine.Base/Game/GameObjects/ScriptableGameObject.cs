using System.Collections.Generic;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class ScriptableGameObject : GameObject
    {
        public ObjectType ObjectType { get; set; }
       public IScriptParamaterProvider ScriptParamProvider { get; set;  }

        public List<IScriptManager> Scripts { get; private set; }

        public ScriptableGameObject(IGameEngine gameEngine) 
            : base(gameEngine)
        {
            Scripts = new List<IScriptManager>();
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            //Test
            foreach (var script in Scripts)
                script.CurrentMenagedScript.Draw();
        }

        protected override void OnUpdate(float dt)
        {
            foreach (var script in Scripts)
            {
                var managedScript = script.CurrentMenagedScript;
                var @params = ScriptParamProvider.GetParamatersForScript(script.CurrentMenagedScript);

                script.ExcuteFunction("Run", @params);
            }
            base.OnUpdate(dt);
        }

        protected override void OnInit()
        {
            base.OnInit();
        }
    }
}