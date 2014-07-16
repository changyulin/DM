using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

        }
    }
}
