using System;
using System.Web.UI;
using DM.Infrastructure.Util.PageUtil;

namespace DM.WebUI
{
    public class BasePage : Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            PageLoadComplete();
        } 

        protected virtual void PageLoadComplete()
        {
            StyleHelpers.AddStyle(this.Page, "Style");
            ScriptHelpers.AddScript(this.Page, "Script");

            ExcutePrePageLoadCompleteJs();

            ExcutePageLoadCompletedJs();
        }



        protected virtual void ExcutePageLoadCompletedJs()
        {
            //StringBuilder js = new StringBuilder();
            //this.ClientScript.RegisterStartupScript(this.GetType(), string.Concat(this.ClientID, "_PageLoadCompleted"), js.ToString(), true);            
        }

        protected virtual void ExcutePrePageLoadCompleteJs()
        {
            //StringBuilder js = new StringBuilder();
            //js.AppendFormat(CultureInfo.InvariantCulture, "SBT.Tools.AL.AlertMessages = YAHOO.lang.JSON.parse('{0}').Strings;", LabelUtil.GetLabelXElementJson("ALAlert"));
            //this.ClientScript.RegisterStartupScript(this.GetType(), string.Concat(this.ClientID, "_PrePageLoadComplete"), js.ToString(), true);        
        }
    }
}