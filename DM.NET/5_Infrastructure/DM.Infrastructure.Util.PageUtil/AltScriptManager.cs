using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;

namespace DM.Infrastructure.Util.PageUtil
{
    /// <summary>
    /// A ScriptManager control that doesn't impose the MicrosoftAjax dependency or add ASP.NET Ajax startup scripts.
    /// </summary>
    public class AltScriptManager : ScriptManager
    {
        private string _frameworkPath;

        public AltScriptManager()
        {
            this.EnableScriptLocalization = false;
            EnablePartialRendering = false;
        }

        [
        Category("Behavior"),
        DefaultValue(false),
        ]
        public new bool EnablePartialRendering
        {
            get
            {
                return base.EnablePartialRendering;
            }
            set
            {
                base.EnablePartialRendering = value;
            }
        }

        [
        Category("Behavior"),
        DefaultValue(""),
        ]
        public string FrameworkPath
        {
            get
            {
                return _frameworkPath ?? String.Empty;
            }
            set
            {
                _frameworkPath = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.ClientScript.RegisterStartupScript(typeof(ScriptManager), "AppInitialize", String.Empty, false);
            Page.ClientScript.RegisterClientScriptBlock(typeof(ScriptManager), "FrameworkLoadedCheck", String.Empty, false);
        }

        protected override void OnPreRender(EventArgs e)
        {
            ScriptReference frameworkScriptReference = new ScriptReference("MicrosoftAjax.js", typeof(ScriptManager).Assembly.FullName);
            frameworkScriptReference.Path = FrameworkPath;
            frameworkScriptReference.ScriptMode = ScriptMode.Release;
            Scripts.Add(frameworkScriptReference);
            base.OnPreRender(e);
        }
    }
}
