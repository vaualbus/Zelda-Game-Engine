using System;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;

namespace ZeldaEngine.Tests.TestRig
{
    public class TestLogger : ILogger
    {
        public string LoggerName
        {
            get { return "TestLogger"; }
        }

        public void LogInfo(string message, params object[] @params)
        {
            var consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Info: " + message, @params);
            Console.ForegroundColor = consoleColor;
        }

        public void LogWarning(string message, params object[] @params)
        {
            var consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: " + message, @params);
            Console.ForegroundColor = consoleColor;
        }

        public void LogDebug(string message, params object[] @params)
        {
            var consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Debug: " + message, @params);
            Console.ForegroundColor = consoleColor;
        }

        public void LogError(string message, params object[] @params)
        {
            var consoleColor = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message, @params);
            Console.ForegroundColor = consoleColor;
        }

        public void LogType(Type scriptType, string message)
        {
            throw new NotImplementedException();
        }
    }
}