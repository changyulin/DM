using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DM.Infrastructure.Util.HtmlHelpers;
using DM.Infrastructure.Util.XmlHelpers;

namespace DM.Infrastructure.Cache
{
    public class StyleItem : CacheConfigRefID<StyleItem, ConfigCacheFactory>
    {
        protected override string ConfigSubFolder { get { return "Style"; } }

        /// <summary>
        /// Get style url list from cache
        /// </summary>
        /// <param name="refID">file name</param>
        /// <returns>style url list</returns>
        public static IEnumerable<XElement> GetStyleList(string refID)
        {
            XElement styleItem = StyleItem.Get(refID).Value;
            List<XElement> styleList = new List<XElement>();
            if (styleItem != null)
            {
                IEnumerable<XElement> scriptItems = styleItem.Elements();
                foreach (XElement item in scriptItems)
                {
                    if (item.Name.LocalName == "Style" && Valid(item))
                    {
                        styleList.Add(item);
                    }
                    else if (item.Name.LocalName == "Include")
                    {
                        StyleItem.Get(item.Attribute("ID").Value);
                    }
                }
            }
            return styleList;
        }

        private static bool Valid(XElement styleItem)
        {
            if (styleItem.Attribute("Browser") != null)
            {
                if (styleItem.Attribute("Browser").Value != HtmlHelpers.BrowserName())
                {
                    return false;
                }

                if (styleItem.Attribute("MajorVersion") != null)
                {
                    if (styleItem.Attribute("MajorVersion").Value != HtmlHelpers.BrowserMajorVersion().ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
