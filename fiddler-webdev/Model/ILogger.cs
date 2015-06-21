namespace Fiddler.Webdev.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public interface ILogger
    {
        bool IsTraceEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }



        void Debug(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "");
        void Error(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "");
        void Fatal(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "");
        void Info(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "");
        void Warn(Func<string> message, Exception e = null, [CallerMemberName]string callingMethod = "");
    }
}
