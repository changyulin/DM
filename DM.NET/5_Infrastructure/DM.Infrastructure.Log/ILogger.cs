using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Infrastructure.Log
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message, Exception ex);
    }
}
