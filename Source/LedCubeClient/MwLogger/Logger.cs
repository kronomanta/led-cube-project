using log4net;

namespace Logger
{
    public static class Logger
    {
        private static readonly ILog MyLogger = LogManager.GetLogger(typeof(Logger));

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();

        }

        public static void WriteLine(string message)
        {
            MyLogger.Debug(message);
        }

        public static void WriteLine(string format, params object[] args)
        {
            MyLogger.Debug(string.Format(format, args));
        }
    }
}