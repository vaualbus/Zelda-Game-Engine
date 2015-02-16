using System;
using System.Runtime.InteropServices;
using ZeldaEngine.ScriptEngine.Abstracts;
using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.ScriptEngine
{
    public interface IZeldaScript
    {
        Vector2 Position { get; set; }

        INpc GetNpc(string npcName);

        IItem GetItem(string itemName);

        void ApplicationInit();

        void Init();

        void Run();
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B6472D9B-8F1B-4328-9A1A-8BEC5087A1EB")]
    public class ZeldaScript : IZeldaScript
    {
        [ComVisible(false)]
        protected readonly IScriptEngine _engine;

        [ComVisible(false)]
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
