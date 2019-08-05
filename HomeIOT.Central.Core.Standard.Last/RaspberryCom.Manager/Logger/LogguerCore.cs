//using System;
//using System.Threading.Tasks;
//using Windows.Storage;

//namespace RaspberryCom.Logger
//{
//    public static class LogguerManager
//    {

//        private static async Task InicializarLogger()
//        {
         
//        }

//        public static async void Log(LogLevel level, string message, params object[] args)
//        {
//            if (!startedLogguer)
//                await InicializarLogger();

//            while (!startedLogguer) { }
//            if (args != null && args.Length > 0)
//                await LoggingSession.Instance.LogToAllChannels(
//                            new LogEntry(
//                                level,
//                                message, args));
//            else if (args != null && args.Length == 0)
//                await LoggingSession.Instance.LogToAllChannels(
//                           new LogEntry(
//                               level,
//                               message));
//        }

//        public static async void Log(LogManagerLevel level, string message, params object[] args)
//        {
//            if (!startedLogguer)
//                await InicializarLogger();

//            while (!startedLogguer) { }
//            if (args != null && args.Length > 0)
//                await LoggingSession.Instance.LogToAllChannels(
//                            new LogEntry(
//                             CastLevel(level),
//                                message, args));
//            else if (args != null && args.Length == 0)
//                await LoggingSession.Instance.LogToAllChannels(
//                           new LogEntry(
//                              CastLevel(level),
//                               message));
//        }



//        public static async void LogMethod(string method, LogMethodAction logAction, string methodCall = "", params object[] args)
//        {
//            if (!startedLogguer)
//                await InicializarLogger();


//            var logMessage = "";

//            if (logAction == LogMethodAction.CALLMETHOD)
//            {
//                logMessage = string.Concat("LogMethod : ", method.ToUpper(), " - ", logAction.ToString(), " - Call : ", method.ToUpper());
//            }
//            else
//            {
//                logMessage = string.Concat("LogMethod : ", method.ToUpper(), " - ", logAction.ToString());
//            }

//            await LoggingSession.Instance.LogToAllChannels(
//                        new LogEntry(
//                             LogLevel.INFO,
//                            logMessage, args));
//        }
//        private static LogLevel CastLevel(LogManagerLevel level) {
//            switch (level)
//            {
//                case LogManagerLevel.TRACE:
//                    return LogLevel.TRACE;
//                    break;
//                case LogManagerLevel.DEBUG:
//                    return LogLevel.DEBUG;
//                    break;
//                case LogManagerLevel.INFO:
//                    return LogLevel.INFO;
//                    break;
//                case LogManagerLevel.WARN:
//                    return LogLevel.WARN;
//                    break;
//                case LogManagerLevel.ERROR:
//                    return LogLevel.ERROR;
//                    break;
//                case LogManagerLevel.FATAL:
//                    return LogLevel.FATAL;
//                    break;
//                default:
//                    return LogLevel.INFO;
//                    break;
//            }
//        }
//    }
//    public enum LogMethodAction
//    {
//        START,
//        END,
//        CALLMETHOD
//    }

//    public enum LogManagerLevel {
//        TRACE = 0,
//        DEBUG = 1,
//        INFO = 2,
//        WARN = 3,
//        ERROR = 4,
//        FATAL = 5
//    }
//}
