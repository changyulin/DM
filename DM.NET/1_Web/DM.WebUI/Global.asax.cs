using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using StackExchange.Profiling;
using DM.Infrastructure.Log;
using DM.Infrastructure.Util.StructureMapHelpers;
using DM.Infrastructure.Route;
using System.IO;
using System.Configuration;

namespace DM.WebUI
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            InitProfilerSettings();

            string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

            // load the configuration file for log4net
            string logFilePath = Path.Combine(configDir, "log.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(logFilePath));

            // initialize structure map
            string structureMapPath = Path.Combine(configDir, "StructureMap.config");
            StructureMapHelper.Initialize(structureMapPath);

            //load the configuration for Route
            RoutesConfig.Configure("RouteRule");

            // this is only done for testing purposes so we don't check in the db to source control
            // ((SampleWeb.Helpers.SqliteMiniProfilerStorage)MiniProfiler.Settings.Storage).RecreateDatabase();
        }

        protected void Application_BeginRequest()
        {
            MiniProfiler profiler = null;

            // might want to decide here (or maybe inside the action) whether you want
            // to profile this request - for example, using an "IsSystemAdmin" flag against
            // the user, or similar; this could also all be done in action filters, but this
            // is simple and practical; just return null for most users. For our test, we'll
            // profile only for local requests (seems reasonable)
            if (Request.IsLocal && ConfigurationManager.AppSettings["EnableMiniProfiler"] == "1")
            {
                profiler = MiniProfiler.Start();
            }

            using (profiler.Step("Application_BeginRequest"))
            {
                // you can start profiling your code immediately
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Exception ex = Server.GetLastError();
            LogHelper.Error("Global Error", ex.InnerException != null ? ex.InnerException : ex);
            Context.ClearError();
            //Response.Redirect("~/Error.aspx");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }
       
        
        /// <summary>
        /// Customize aspects of the MiniProfiler.
        /// </summary>
        private void InitProfilerSettings()
        {
            // some things should never be seen
            var ignored = MiniProfiler.Settings.IgnoredPaths.ToList();
            ignored.Add("WebResource.axd");
            ignored.Add("/Styles/");
            MiniProfiler.Settings.IgnoredPaths = ignored.ToArray();

            // MiniProfiler.Settings.Storage = new SampleWeb.Helpers.SqliteMiniProfilerStorage(SampleWeb.MvcApplication.ConnectionString);
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
        }

    }
}
