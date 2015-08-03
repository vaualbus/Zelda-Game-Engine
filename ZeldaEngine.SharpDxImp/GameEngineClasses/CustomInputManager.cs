using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Toolkit.Input;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

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

        public bool IsKeyDown(Keys key)
        {
            return GetKey(key)  && !_lastKeys[(int) key];
        }

        public bool IsKeyUp(Keys key)
        {
            return !GetKey(key) && _lastKeys[(int)key];
        }

        public bool IsKeyUp<TData>(TData data) where TData : struct, IConvertible
        {
            if (!typeof(TData).IsEnum)
                throw new Exception("Cannot Parse a non Enum to this function. C# is broken");

            var pressedKey = ((Keys[])Enum.GetValues(typeof(TData))).First(t => data.Equals(t));
            if (pressedKey == Keys.None)
                throw new Exception("Two keys with the same name???. C# is broken");


            return IsKeyUp(pressedKey);
        }

        public bool IsKeyDown<TData>(TData data) where TData : struct, IConvertible
        {
            if (!typeof(TData).IsEnum)
                throw new Exception("Cannot Parse a non Enum to this function. C# is broken");

            var pressedKey = ((Keys[])Enum.GetValues(typeof(TData))).First(t => data.Equals(t));
            if (pressedKey == Keys.None)
                throw new Exception("Two keys with the same name???. C# is broken");


            return IsKeyDown(pressedKey);
        }

        public bool IsKeyUp(string keyName)
        {
            return IsKeyUp((Keys)Enum.Parse(typeof(Keys), keyName));
        }

        public bool IsKeyDown(string keyName)
        {
            return IsKeyDown((Keys)Enum.Parse(typeof(Keys), keyName));
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
    }
}