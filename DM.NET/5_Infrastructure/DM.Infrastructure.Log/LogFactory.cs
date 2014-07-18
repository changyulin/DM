using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM.Infrastructure.Log
{
    public class LogFactory
    {
        public static ILogger GetLogger()
        {
            ILogger log = new DefaultLogger();
            return log;
        }
    }
}
