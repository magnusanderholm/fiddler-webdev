namespace Fiddler.Webdev.Model
{
    using System;
    using System.Runtime.CompilerServices;


    class Logger : ILogger
    {
        private readonly Fiddler.Logger logger;
        public enum LogLevel { Debug = 0, Info = 1, Warn = 2, Error = 3, Fatal = 4 };
        private readonly string name;
        private const string logFormat = "[{0}] ({1}.{2}) - {3}";

        public Logger(string name)
        {
            logger = FiddlerApplication.Log;
            this.name = name;
        }

        public bool IsTraceEnabled { get { return true; } }
        public bool IsDebugEnabled { get { return true; } }
        public bool IsInfoEnabled { get { return true; } }
        public bool IsWarnEnabled { get { return true; } }
        public bool IsErrorEnabled { get { return true; } }
        public bool IsFatalEnabled { get { return true; } }

        public void Debug(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "")
        {
            Log(LogLevel.Debug, message, e, callingMethod);
        }


        public void Info(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "")
        {
            Log(LogLevel.Info, message, e, callingMethod);
        }


        public void Warn(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "")
        {
            Log(LogLevel.Warn, message, e, callingMethod);
        }


        public void Error(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "")
        {
            Log(LogLevel.Error, message, e, callingMethod);
        }


        public void Fatal(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "")
        {
            Log(LogLevel.Fatal, message, e, callingMethod);
        }

        private void Log(LogLevel logLevel, Func<string> message, Exception e, string callingMethod)
        {
            string logMsg = string.Format(
                logFormat,                                
                logLevel.ToString(),
                name,
                callingMethod,
                message());
            if (e != null)
                logMsg += Environment.NewLine + e.ToString();
            logger.LogString(logMsg);            
        }
    }
}
