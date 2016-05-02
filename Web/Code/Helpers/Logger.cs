using log4net;
using System;

namespace RecordLabel.Web
{
    public static class Logger
    {
        /// <summary>
        /// Logs the supplied exception as Error with a logger of a given type
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="loggerType"></param>
        public static void LogError(Exception exception, string loggerType)
        {
            ILog logger = LogManager.GetLogger(loggerType);
            logger.Error(exception);
        }

        /// <summary>
        /// Logs the supplied exception as Error automatically determining logger type
        /// </summary>
        /// <param name="exception"></param>
        public static void LogError(Exception exception)
        {
            LogError(exception, exception.TargetSite.DeclaringType.Name);
        }
    }
}