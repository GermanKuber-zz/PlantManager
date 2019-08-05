//using System;
//using System.Threading.Tasks;
//using Windows.Storage;
//using WindowsUniversalLogger.Interfaces;
//using WindowsUniversalLogger.Interfaces.Channels;
//using WindowsUniversalLogger.Logging;
//using WindowsUniversalLogger.Logging.Channels;
//using WindowsUniversalLogger.Logging.Sessions;

//namespace HomeIOT.Central.Core.Logger
//{
//    public static class WindowsUniversalLoggerCurrentLogger
//    {
//        static ILoggingSession  session = LoggingSession.Instance;
//        private static bool startedLogguer = false;

//        private static async Task InicializarLogger()
//        {
//            var dateTimeFileName = DateTime.Now.ToString("MM-dd-yyyy__HH-mm-ss");

//            var fileName = string.Concat("logHomeIotCentral_", dateTimeFileName, "dat");
//            ILoggingChannel channel = new FileLoggingChannel(
//                                        "HomeIotCentral",
//                                        ApplicationData.Current.LocalFolder,
//                                        fileName);
           
//            await channel.Init();
//            session.AddLoggingChannel(channel);
//            startedLogguer = true;
//        }

//        public static async void Log(LogLevel level, string message, params object[] args)
//        {
//            if (!startedLogguer)
//               await  InicializarLogger();

//            await LoggingSession.Instance.LogToAllChannels(
//                        new LogEntry(
//                            level,
//                            message, args));
//        }
//    }
//}
