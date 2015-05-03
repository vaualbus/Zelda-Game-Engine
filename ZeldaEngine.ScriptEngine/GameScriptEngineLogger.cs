using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;

namespace ZeldaEngine.ScriptEngine
{
    public class GameScriptEngineLogger : ILogger
    {
        private readonly StreamWriter _logFileWriter;

        private static readonly int ErrorLoggerCounter = 0;
        private static int _loggerNumber = 0;

        static GameScriptEngineLogger()
        {
            ErrorLoggerCounter++;
        }

        public string LoggerName
        {
            get { return "ScriptEngineLogger"; }
        }

        public GameScriptEngineLogger(IScriptEngine engine)
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            _logFileWriter = new StreamWriter(string.Format("{0}\\Logs\\{1}_{2}.log", engine.GameEngine.GameConfig.BaseDirectory, LoggerName, _loggerNumber));
            _loggerNumber++;
        }

        public void LogInfo(string message, params object[] @params)
        {
            _logFileWriter.WriteLine("[" + ErrorLoggerCounter + "] Info: " + message, @params);
            _logFileWriter.Flush();
        }

        public void LogWarning(string message, params object[] @params)
        {
            _logFileWriter.WriteLine("[" + ErrorLoggerCounter + "] Warning: " + message, @params);
            _logFileWriter.Flush();
        }

        public void LogDebug(string message, params object[] @params)
        {
            _logFileWriter.WriteLine("[" + ErrorLoggerCounter + "] Debug: " + message, @params);
            _logFileWriter.Flush();
        }

        public void LogError(string message, params object[] @params)
        {
            _logFileWriter.WriteLine("[" + ErrorLoggerCounter + "] Error: " + message, @params);
            _logFileWriter.Flush();
        }

        public void LogType(Type scriptType, string message)
        {
            throw new NotImplementedException();
        }
    }
}