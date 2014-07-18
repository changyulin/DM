using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Infrastructure.Log
{
    public class LogHelper
    {
        public static readonly log4net.ILog logDebug = log4net.LogManager.GetLogger("logDebug");
        public static readonly log4net.ILog logError = log4net.LogManager.GetLogger("logError");

        public static void Debug(string message)
        {
            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(message);
            }
        }

        public static void Error(string message, Exception ex)
        {
            if (logError.IsErrorEnabled)
            {
                logError.Error(message, ex);
            }
        }
    }
}
