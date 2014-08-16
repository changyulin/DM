using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Text;

namespace DM.Infrastructure.Log
{
    public class Logging : ILogger
    {
        public void Info(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "Info";
            logEntry.Message = message;
            logEntry.TimeStamp = DateTime.Now;
            logEntry.Severity = TraceEventType.Information;
            logEntry.Priority = 1;
            logEntry.Categories.Add("Info");
            Logger.Write(logEntry);
        }

        public void Debug(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "Debug";
            logEntry.Message = message;
            logEntry.TimeStamp = DateTime.Now;
            logEntry.Severity = TraceEventType.Verbose;
            logEntry.Priority = 2;
            logEntry.Categories.Add("Debug");
            Logger.Write(logEntry);
        }

        public void Error(string message, Exception ex)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = "Error";
            StringBuilder exMessage = new StringBuilder(100);
            exMessage.AppendLine(message);
            exMessage.AppendLine("Message: "+ex.Message);
            exMessage.AppendLine("Source: " + ex.Source);
            exMessage.AppendLine("StackTrace: " + ex.StackTrace);
            logEntry.Message = exMessage.ToString();
            logEntry.TimeStamp = DateTime.Now;
            logEntry.Severity = TraceEventType.Error;
            logEntry.Priority = 3;
            logEntry.Categories.Add("Error");
            Logger.Write(logEntry);
        }


    }
}
