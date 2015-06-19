using System;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.ScriptEngine;

namespace ZeldaEngine.Base
{
    public class GameScript
    {
        protected IScriptEngine Engine;

        protected  ScriptInfo ScriptInfo;

        protected  ILogger Logger;

        protected IRenderEngine RenderEngine;

        protected IAudioEngine AudioEngine;

        protected IContentLoader ResourceLoader;

        protected IInputManager InputManager;

        protected IGameObjectFactory GameObjectFactory;

        /// <summary>
        /// Later on we need to provide only the game base configuration so scripters cannot screw up the engine
        /// </summary>
        protected GameConfig Config => Engine.GameEngine?.GameConfig;

        public IGameObject GameObject { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 InitialPosition;

        public object ScriptObjectColor { get; set; }
        public float CurrentTime { get; set; }

        protected GameScript()
        {
            Position = new Vector2();
        }

        /// <summary>
        /// This is called to init script component, run on the same tread as the script
        /// </summary>
        public virtual void Init()
        {
            OnInit();
        }

        public virtual void OnInit() { }

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

        public void Draw()
        {
            OnDraw();
        }

        public virtual void OnDraw(IGameView gameView, IRenderEngine renderEngine) {  }

        public virtual void OnDraw() { }

        public virtual INpc GetNpc(string npcName)
        {
            throw new NotImplementedException();
        }

        public virtual IItem GetItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public virtual void WaitFrame()
        {
        }

        public virtual void SetInitalPosition(Vector2 position)
        {
            InitialPosition = position;
            Position = position;
        }

        public virtual void SetInitialColor(object color)
        {
            ScriptObjectColor = color;
        }

        public IResourceData LoadTexture2D(string name)
        {
           return Engine.GameEngine.Texture2DData(name);
        }

        public virtual void ExitGame()
        {
            Engine.GameEngine.ExitGame();
        }
    }
}
