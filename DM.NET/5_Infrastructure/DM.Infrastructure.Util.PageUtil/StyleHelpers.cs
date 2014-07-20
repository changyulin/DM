using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DM.Infrastructure.Cache;

namespace DM.Infrastructure.Util.PageUtil
{
    public class StyleHelpers
    {
        /// <summary>
        /// Add scriptFileName's script url to Page.Items
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="scriptItem">file name under App_Data/Script</param>
        public static void AddStyle(Page page, string styleFileName)
        {
            IEnumerable<XElement> styleList = StyleItem.GetStyleList(styleFileName);
            if (styleList != null)
            {
                foreach (XElement x in styleList)
                {
                    PageHelpers.Add(page, "StyleList", x.Value);
                }
            }
        }

        /// <summary>
        /// load the Page.Items's script
        /// </summary>
        /// <param name="page"></param>
        public static void LoadStyle(Page page)
        {
            List<string> styleList = page.Items["StyleList"] as List<string>;
            if (styleList != null)
            {
                bool optimizeStyle = ConfigurationManager.AppSettings["OptimizeStyle"] == "1";
                if (optimizeStyle)
                {
                    // cache the style if not cached already
                    PageHelpers.Cache(StyleCacheFactory.GetCacheManager(), styleList);
                    string url = string.Format("{0}?hash={1}",
                        page.ResolveClientUrl("~/StyleOptimizer.ashx"),
                        HttpUtility.UrlEncode(PageHelpers.GetHash(styleList)));
                    AddStyleToPage(page, url);
                }
                else
                {
                    foreach (string styleUrl in styleList)
                    {
                        AddStyleToPage(page, styleUrl);
                    }
                }
            }
        }

        
        private static void AddStyleToPage(Page page, string styleUrl)
        {
            HtmlLink css = new HtmlLink();
            css.Href = styleUrl;
            css.Attributes.Add("type", "text/css");
            css.Attributes.Add("rel", "stylesheet");
            page.Header.Controls.Add(css);
        }
    }
}
