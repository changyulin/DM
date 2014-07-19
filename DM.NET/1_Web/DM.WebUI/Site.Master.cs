using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DM.Infrastructure.Util.FeatureHelpers;
using DM.Infrastructure.Util.ScriptHelpers;
using StackExchange.Profiling;

namespace DM.WebUI
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LiteralControl miniProfilerScript = new LiteralControl(MiniProfiler.RenderIncludesAsString());
            miniProfiler.Controls.Add(miniProfilerScript);
        }

        protected void Page_PreRender(object sender, EventArgs o)
        {
            List<string> scriptList = Page.Items["ScriptList"] as List<string>;
            if (scriptList != null)
            {
                // cache the scripts if not cached already
                ScriptHelpers.CacheScripts(Page, scriptList);

                string url = string.Format("{0}?hash={1}", Page.ResolveClientUrl("~/scriptoptimizer.ashx"), HttpUtility.UrlEncode(ScriptHelpers.GetHash(scriptList)));
                Page.ClientScript.RegisterClientScriptInclude("baseall", url);
            }
            else
            {
                // not optimizing scripts, make sure jquery gets added
                this.ScriptMgr.FrameworkPath = FeatureHelpers.GetFeature("jQueryURL", "InternalURL");
            }
        }
    }
}
