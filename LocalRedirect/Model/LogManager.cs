namespace Fiddler.LocalRedirect.Model
{
    using System.Diagnostics;

    public class LogManager
    {
        private static readonly ILogger logger = null;
        
        static LogManager()
        {                                
            logger = LogManager.Create<LogManager>();            
        }
        
        public static ILogger CreateCurrentClassLogger()
        {
            StackFrame stackFrame = new StackFrame(1, false);
            return Create(stackFrame.GetMethod().DeclaringType.FullName);
        }

        public static ILogger Create<T>()
        {
            return Create(typeof(T).FullName);
        }

        public static ILogger Create(string name=null)
        {            
            return new Logger(name);            
        }
    }
}
