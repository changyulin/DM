using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace DM.Infrastructure.Log
{
    public class DefaultLogger : ILogger
    {
        public readonly ILog logDebug;
        public readonly ILog logError;

        public DefaultLogger()
        {
            logDebug = LogManager.GetLogger("logDebug");
            logError = LogManager.GetLogger("logError");
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
