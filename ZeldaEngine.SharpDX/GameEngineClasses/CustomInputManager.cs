using System;
using System.Linq;
using SharpDX.Toolkit.Input;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.SharpDx.GameEngineClasses
{
    public class CustomInputManager : IInputManager
    {
        private readonly InputConfigurationDefinition _inputConfigurationDefinition;
        private readonly SharpDxCoreEngine _sharpDxCoreEngine;
        private readonly KeyboardManager _keyboardManager;
        private KeyboardState _keyBoardState;

        public CustomInputManager(SharpDxCoreEngine sharpDxCoreEngine, InputConfigurationDefinition inputConfigurationDefinition)
        {   _sharpDxCoreEngine = sharpDxCoreEngine;
            _inputConfigurationDefinition = inputConfigurationDefinition;

            _keyboardManager = new KeyboardManager(_sharpDxCoreEngine);
        }

        public bool IsKeyDown(Keys key)
        {
            var state =_keyBoardState.IsKeyPressed(key);
            Update();
            return state;
        }

        public bool IsKeyUp(Keys key)
        {
            var temp = _keyBoardState.IsKeyReleased(key);
            Update();
            return temp;
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

        public void Update()
        {
            _keyBoardState = _keyboardManager.GetState();
        }
    }
}