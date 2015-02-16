using ZeldaEngine.Base.Game.GameScripts;

namespace ZeldaEngine.Base.ValueObjects.Game
{
    public class ScriptTuple
    {
        public GameScript Script { get; set; }

        public ScriptState State { get; set; }

        public ScriptTuple()
        {
            Script = new EmptyGameScript();
            State = ScriptState.NotSet;
        }
    }
}