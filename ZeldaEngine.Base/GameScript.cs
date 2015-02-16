using System;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public class GameScript
    {
        protected readonly IScriptEngine _engine;

        protected readonly ScriptInfo _scriptInfo;

        protected readonly ILogger _logger;

        public IGameObject GameObject { get; set; }

        public Vector2 Position { get; set; }

        protected GameScript()
        {
            Position = new Vector2();
        }

        /// <summary>
        /// This is called to init script component, run on the same tread as the script
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// This function is called when the game is initially loaded
        /// This function have not to be use for init script component
        /// but fo example to init game status, game custom action
        /// </summary>
        public virtual void ApplicationInit()
        {
        }

        public void Draw(IGameView gameView, IRenderEngine renderEngine)
        {
            OnDraw(gameView, renderEngine);
        }

        public virtual void OnDraw(IGameView gameView, IRenderEngine renderEngine) {  }

        public virtual INpc GetNpc(string npcName)
        {
            throw new NotImplementedException();
        }

        public virtual IItem GetItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
