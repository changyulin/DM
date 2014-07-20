using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DM.Infrastructure.Cache;

namespace DM.Infrastructure.Util.PageUtil
{
    public class ScriptHelpers
    {
        /// <summary>
        /// Add scriptFileName's script url to Page.Items
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="scriptItem">file name under App_Data/Script</param>
        public static void AddScript(Page page, string scriptFileName)
        {
            IEnumerable<XElement> scriptList = ScriptItem.GetJSList(scriptFileName);
            if (scriptList != null)
            {
                foreach (XElement x in scriptList)
                {
                    PageHelpers.Add(page, "ScriptList", x.Value);
                }
            }
        }

        /// <summary>
        /// load the Page.Items's script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadScript(Page page)
        {
            List<string> scriptList = page.Items["ScriptList"] as List<string>;
            if (scriptList != null)
            {
                bool optimizeJS = ConfigurationManager.AppSettings["OptimizeJS"] == "1";
                if (optimizeJS)
                {
                    // cache the scripts if not cached already
                    PageHelpers.Cache(ScriptCacheFactory.GetCacheManager(), scriptList);
                    string url = string.Format("{0}?hash={1}",
                        page.ResolveClientUrl("~/scriptoptimizer.ashx"),
                        HttpUtility.UrlEncode(PageHelpers.GetHash(scriptList)));
                    AddScriptToPage(page, url);
                    //or use: page.ClientScript.RegisterClientScriptInclude("baseall", url);
                }
                else
                {
                    foreach (string scriptUrl in scriptList)
                    {
                        AddScriptToPage(page, scriptUrl);
                    }
                }
            }
        }

        private static void AddScriptToPage(Page page, string scriptUrl)
        {
            HtmlGenericControl scriptHtml = new HtmlGenericControl("script");
            scriptHtml.Attributes.Add("type", "text/javascript");
            scriptHtml.Attributes.Add("src", scriptUrl);
            page.Header.Controls.Add(scriptHtml);
        }
    }
}
