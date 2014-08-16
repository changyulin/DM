using System;
using log4net;

namespace DM.Infrastructure.Log
{
    public class Log4 : ILogger
    {
        public readonly ILog logInfo;
        public readonly ILog logDebug;
        public readonly ILog logError;

        public Log4()
        {
            logInfo = LogManager.GetLogger("logInfo");
            logDebug = LogManager.GetLogger("logDebug");
            logError = LogManager.GetLogger("logError");
        }

        public void Info(string message)
        {
            if (logInfo.IsInfoEnabled)
            {
                logInfo.Info(message);
            }
        }

        public void Debug(string message)
        {
            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(message);
            }
        }

        public void Error(string message, Exception ex)
        {
            if (logError.IsErrorEnabled)
            {
                logError.Error(message, ex);
            }
        }
    }
}
