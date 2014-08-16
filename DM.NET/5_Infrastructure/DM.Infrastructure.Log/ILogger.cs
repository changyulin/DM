using System;

namespace DM.Infrastructure.Log
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message, Exception ex);
    }
}
