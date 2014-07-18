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
    public class ScriptItem : CacheConfigRefID<ScriptItem, ConfigCacheFactory>
    {
        protected override string ConfigSubFolder { get { return "Script"; } }

        public static IEnumerable<XElement> GetJSList(string refID)
        {
            XElement scriptItem = ScriptItem.Get(refID).Value;
            List<XElement> jsList = new List<XElement>();
            if (scriptItem == null)
            {
                return jsList;
            }
            IEnumerable<XElement> scriptItems = scriptItem.Elements();
            foreach (XElement item in scriptItems)
            {
                if (item.Name.LocalName == "Script" && Valid(item))
                {
                    jsList.Add(item);
                }
                else if(item.Name.LocalName == "Include")
                {
                    ScriptItem.Get(item.Attribute("ID").Value);
                }
            }
            return jsList;
        }

        private static bool Valid(XElement jsItem)
        {
            string browser = XmlHelpers.GetAttribute(jsItem, "Browser");
            bool browserPass = browser.Length == 0 || HtmlHelpers.BrowserName().Equals(browser, StringComparison.OrdinalIgnoreCase);
            if (!browserPass)
                return false;

            // if the browser is specified, check if the version number is specified
            if (!string.IsNullOrEmpty(browser))
            {
                int version;
                if (Int32.TryParse(XmlHelpers.GetAttribute(jsItem, "BrowserMaxVersion"), out version))
                {
                    if (HtmlHelpers.BrowserMajorVersion() > version)
                        return false;
                }

                if (Int32.TryParse(XmlHelpers.GetAttribute(jsItem, "BrowserMinVersion"), out version))
                {
                    if (HtmlHelpers.BrowserMajorVersion() < version)
                        return false;
                }
            }

            bool devicePass = HtmlHelpers.DeviceSupported(jsItem);
            if (!devicePass)
                return false;

            string environment = XmlHelpers.GetAttribute(jsItem, "Environment");
            bool environmentPass = environment.Length == 0 || ConfigurationManager.AppSettings["Environment"].Equals(environment, StringComparison.OrdinalIgnoreCase);

            return environmentPass;
        }
    }
}
