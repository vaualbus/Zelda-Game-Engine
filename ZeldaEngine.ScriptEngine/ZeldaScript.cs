using System;
using System.Runtime.InteropServices;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.ScriptEngine.Abstracts;

namespace ZeldaEngine.ScriptEngine
{
    public class ZeldaScript : IZeldaScript
    {
        protected readonly IScriptEngine _engine;

        protected readonly ScriptInfo _scriptInfo;

        public Vector2 Position { get; set; }

        public ZeldaScript()
        {
            Position = new Vector2();
        }

        /// <summary>
        /// This is called to init script component, run on the same tread as the script
        /// </summary>
        public virtual void Init()
        {
        }

        public virtual void Run()
        {
            Position.X += 20;
            Position.Y += 10;

            if (Position.X > 1000)
                Position.X = 0;

            if (Position.Y > 1000)
                Position.Y = 0;
        }

        /// <summary>
        /// This function is called when the game is initially loaded
        /// This function have not to be use for init script component
        /// but fo example to init game status, game custom action
        /// </summary>
        public virtual void ApplicationInit()
        {
            Position = new Vector2(20, 20);
        }

        protected virtual void Log(string message, ReportReason reason = ReportReason.Log)
        {
            _engine.Report(string.Format("{0} {1}", _scriptInfo.ScriptName, message), reason);
        }

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
