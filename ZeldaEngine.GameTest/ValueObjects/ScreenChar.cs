using ZeldaEngine.ScriptEngine.ValueObjects;

namespace ZeldaEngine.GameTest.ValueObjects
{
    public class ScreenChar
    {
        public Vector2 CharPos { get; set; }

        public string CharString { get; private set; }

        public ScreenChar(string @char)
        {
            CharString = @char;
            CharPos = new Vector2(0, 0);
        }
    }
}
