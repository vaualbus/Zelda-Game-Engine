namespace ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes
{
    public class InputConfigurationDefinition
    {
        public string KeyUp { get; private set; }

        public string KeyDown { get; private set; }

        public string KeyLeft { get; private set; }

        public string KeyRight { get; private set; }

        public string KeyA { get; private set; }

        public string KeyB { get; private set; }

        public string KeyStart { get; private set; }

        public string KeyMap { get; private set; }

        public InputConfigurationDefinition(string keyUp, string keyDown, string keyLeft, string keyRight, string keyA,
            string keyB, string keyStart, string keyMap)
        {
            KeyUp = keyUp;
            KeyDown = keyDown;
            KeyLeft = keyLeft;
            KeyRight = keyRight;
            KeyA = keyA;
            KeyStart = keyStart;
            KeyMap = keyMap;
            KeyB = keyB;
        }
    }
}