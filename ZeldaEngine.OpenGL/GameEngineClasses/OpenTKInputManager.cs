using System;
using System.Linq;
using OpenTK.Input;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game;
using ZeldaEngine.Base.Game.ValueObjects;

namespace ZeldaEngine.OpenGL.GameEngineClasses
{
    public class OpenTKInputManager : BaseGameClass, IInputManager
    {
        private readonly InputConfigurationDefinition _inputConfig;

        public OpenTKInputManager(IGameEngine gameEngine, InputConfigurationDefinition inputConfig)
            : base(gameEngine)
        {
            _inputConfig = inputConfig;
            GameEngine.Logger.LogInfo("Input Manager Initialized");
        }

        public bool IsKeyDown(Key key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public bool IsKeyUp(Key key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }

        public bool IsKeyUp<TData>(TData data) where TData : struct, IConvertible
        {
            if (!typeof(TData).IsEnum)
                throw new Exception("Cannot Parse a non Enum to this function. C# is broken");

            var pressedKey = ((Key[])Enum.GetValues(typeof(TData))).First(t => data.Equals(t));
            if (pressedKey == Key.Unknown)
                throw new Exception("Two keys with the same name???. C# is broken");


            return IsKeyUp(pressedKey);
        }

        public bool IsKeyDown<TData>(TData data) where TData : struct, IConvertible
        {
            if (!typeof(TData).IsEnum)
                throw new Exception("Cannot Parse a non Enum to this function. C# is broken");

            var pressedKey = ((Key[])Enum.GetValues(typeof(TData))).First(t => data.Equals(t));
            if (pressedKey == Key.Unknown)
                throw new Exception("Two keys with the same name???. C# is broken");


            return IsKeyDown(pressedKey);
        }

        public bool IsKeyUp(string keyName)
        {
            return IsKeyUp((Key) Enum.Parse(typeof (Key), keyName));
        }

        public bool IsKeyDown(string keyName)
        {
            return IsKeyDown((Key)Enum.Parse(typeof(Key), keyName));
        }
    }
}