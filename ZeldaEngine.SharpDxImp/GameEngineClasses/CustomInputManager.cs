using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharpDX.Toolkit.Input;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;
using Keys = SharpDX.Toolkit.Input.Keys;

namespace ZeldaEngine.SharpDxImp.GameEngineClasses
{
    public class CustomInputManager : IInputManager
    {
        private const int NumKeys = (int) (Keys.Zoom + 1);

        private readonly List<bool> _lastKeys; 

        private readonly SharpDxCoreEngine _sharpDxCoreEngine;

        private readonly InputConfigurationDefinition _inputConfigurationDefinition;

        private readonly KeyboardManager _keyboardManager;

        private readonly MouseManager _mouseManager;

        public Vector2 MousePosition => new Vector2(_mouseManager.GetState().X * _sharpDxCoreEngine.GameConfig.ScreenWidth, _mouseManager.GetState().Y * _sharpDxCoreEngine.GameConfig.ScreenHeight);

        public CustomInputManager(SharpDxCoreEngine sharpDxCoreEngine, InputConfigurationDefinition inputConfigurationDefinition)
        {
            _sharpDxCoreEngine = sharpDxCoreEngine;
            _inputConfigurationDefinition = inputConfigurationDefinition;

            _keyboardManager = new KeyboardManager(sharpDxCoreEngine);
            _mouseManager = new MouseManager(sharpDxCoreEngine);
            _lastKeys = new List<bool>(NumKeys);
        }

        public bool IsKeyUp(GameKeys key)
        { 
            return IsKeyUp(MatchKey(key));
        }

        public bool IsKeyDown(GameKeys key)
        {
            return IsKeyDown(MatchKey(key));
        }

        public bool IsKeyDown(Keys key)
        {
            return GetKey(key)  && !_lastKeys[(int) key];
        }

        public bool IsKeyUp(Keys key)
        {
            return !GetKey(key) && _lastKeys[(int)key];
        }


        public bool GetKey(Keys key)
        {
            return _keyboardManager.GetState().IsKeyDown(key);
        }

        public bool IsMouseButtonPressed(string btnName)
        {
            switch (btnName)
            {
                case "Left":
                    return _mouseManager.GetState().LeftButton.Down;

                case "Right":
                    return _mouseManager.GetState().RightButton.Down;

                default:
                    return false;
            }
        }

        public bool IsMouseButtonReleased(string btnName)
        {
            switch (btnName)
            {
                case "Left":
                    return !_mouseManager.GetState().LeftButton.Down;

                case "Right":
                    return !_mouseManager.GetState().RightButton.Down;

                default:
                    return false;
            }
        }

        public void Update()
        {
            for (var i = 0; i < NumKeys; i++)
                _lastKeys.Add(GetKey((Keys)i)); 
        }

        public Keys MatchKey(GameKeys key)
        {
        //public string KeyA { get; private set; }

        //public string KeyB { get; private set; }

        //public string KeyStart { get; private set; }

        //public string KeyMap { get; private set; }

        //public string Ex1 { get; private set; }

        //public string Ex2 { get; private set; }

        //public string Ex3 { get; private set; }

        //public string Ex4 { get; private set; }

            var keyUp = (Keys) Enum.Parse(typeof (Keys), _inputConfigurationDefinition.KeyUp);
            var keyDown = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyDown);
            var keyLeft = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyLeft);
            var keyRight = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyRight);

            var keyA = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyA);
            var keyB = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyA);

            var keyStart = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyStart);
            var keyMap = (Keys) Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyMap);
           var keyExit = (Keys)Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyExit);

            var keyEx1 = (Keys)Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyEx1);
            var keyEx2 = (Keys)Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyEx2);
            var keyEx3 = (Keys)Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyEx3);
            var keyEx4 = (Keys)Enum.Parse(typeof(Keys), _inputConfigurationDefinition.KeyEx4);


            switch (key)
            {
                case GameKeys.A:
                    return keyA;

                case GameKeys.B:
                    return keyB;

                case GameKeys.Up:
                    return keyUp;

                case GameKeys.Down:
                    return keyDown;

                case GameKeys.Left:
                    return keyLeft;

                case GameKeys.Right:
                    return keyRight;

                case GameKeys.Start:
                    return keyStart;

                case GameKeys.Map:
                    return keyMap;

                case GameKeys.Exit:
                    return keyExit;

                case GameKeys.Ex1:
                    return keyEx1;

                case GameKeys.Ex2:
                    return keyEx2;

                case GameKeys.Ex3:
                    return keyEx3;

                case GameKeys.Ex4:
                    return keyEx4;

                default:
                    throw new Exception("Invalid key!");
            }
        }
    }
}