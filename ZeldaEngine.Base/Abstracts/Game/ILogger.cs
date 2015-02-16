using System;
using System.Security;

namespace ZeldaEngine.Base.Abstracts.Game
{
    public interface ILogger
    {
        string LoggerName { get; }

        void LogInfo(string message, params object[] @params);

        void LogWarning(string message, params object[] @params);

        void LogDebug(string message, params object[] @params);

        void LogError(string message, params object[] @params);

        void LogType(Type scriptType, string message);
    }
}