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

        public string KeyExit { get; private set; }

        public string KeyEx1 { get; private set; }

        public string KeyEx2 { get; private set; }

        public string KeyEx3 { get; private set; }

        public string KeyEx4 { get; private set; }

        public InputConfigurationDefinition(string keyUp, string keyDown, string keyLeft,
                                            string keyRight, string keyA, string keyB,
                                            string keyStart, string keyMap, string keyExit,
                                            string keyEx1, string keyEx2, string keyEx3,
                                            string keyEx4)
        {
            KeyUp = keyUp;
            KeyDown = keyDown;
            KeyLeft = keyLeft;
            KeyRight = keyRight;

            KeyA = keyA;
            KeyB = keyB;

            KeyStart = keyStart;
            KeyMap = keyMap;
            KeyExit = keyExit;


            KeyEx1 = keyEx1;
            KeyEx2 = keyEx2;
            KeyEx3 = keyEx3;
            KeyEx4 = keyEx4;
        }
    }
}