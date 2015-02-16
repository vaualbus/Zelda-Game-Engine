using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TestCOM
{
    public enum ReportReason
    {
        Log,
        Warning,
        Error
    }

    public interface ITestLogger
    {
        string FilePath { get; set; }

        void Init();

        void Log(string message, ReportReason reason);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B6472D9B-8F1B-4328-9A1A-8BEC5087A1EB")]
    public class TestLogger : ITestLogger
    {
        private StreamWriter _fileLogger;

        public string FilePath { get; set; }

        public TestLogger()
        {
        }

        [ComVisible(true)]
        public void Init()
        {
            _fileLogger = new StreamWriter(string.Format("{0}/Log.log", FilePath));
        }

        [ComVisible(true)]
        public void Log(string message, ReportReason reason)
        {
           _fileLogger.WriteLine("{0}: {1}", GetMessageType(reason), message); 
        }

        private string GetMessageType(ReportReason reason)
        {
            switch (reason)
            {
                case ReportReason.Error:
                    return "Error";

                case ReportReason.Warning:
                    return "Warning";

                case ReportReason.Log:
                    return "Log";

                default:
                    return "Unknow message";
            }
        }
    }
}