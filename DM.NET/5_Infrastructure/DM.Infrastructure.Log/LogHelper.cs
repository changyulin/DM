using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DM.Infrastructure.Log
{
    public class LogHelper
    {
        public static readonly ILogger logger = LogFactory.GetLogger();

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        public static void Error(string message, Exception ex)
        {
            logger.Error(message, ex);
        }

        public static void HandleException(Exception ex, string policy = "Policy")
        {
            Boolean rethrow = false;
            var exManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
            try
            {
                rethrow = exManager.HandleException(ex, policy);
            }
            catch (Exception)
            {
                throw ex;
            }
        }

    }
}
