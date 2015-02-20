using System;
using System.Diagnostics;
using System.IO;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Base
{
    public class GameLogger : ILogger
    {
        private readonly StreamWriter _logFileWriter;
        private static readonly int ErrorLoggerCounter = 0;
        private static int _loggerNumber;

        public string LoggerName
        {
            get { return "GameLog"; }
        }

        static GameLogger()
        {
            ErrorLoggerCounter++;
        }

        public GameLogger(Config config)
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            _logFileWriter = new StreamWriter(string.Format("{0}\\Logs\\{1}_{2}.log", config.GameConfig.BaseDirectory, LoggerName, _loggerNumber));
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
#if DEBUG
            Debug.WriteLine("[" + ErrorLoggerCounter + "] Debug: " + message, @params);
#endif
        }

        public void LogError(string message, params object[] @params)
        {
            _logFileWriter.WriteLine("[" + ErrorLoggerCounter + "] Error: " + message, @params);
            _logFileWriter.Flush();

#if DEBUG
            throw new Exception(string.Format("Error: {0} ({1})", message, string.Join(", ", @params)));
#endif
        }

        public void LogType(Type scriptType, string message)
        {
            throw new NotImplementedException();
        } 
    }
}