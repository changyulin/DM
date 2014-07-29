using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace DM.Infrastructure.Cache
{
    public class LangItem : CacheConfigRefID<LangItem, LangCacheFactory>
    {

        protected override string ConfigSubFolder { get { return "Lang"; } }
        private string Language { set; get; } // en, zh
        private string Area { set; get; } // US, CN

        protected override XElement GetFromSource()
        {
            string file = string.Format(CultureInfo.InvariantCulture, this.ConfigFileFormat, this.RefID);
            string path = Path.Combine(this.ConfigFolder, this.ConfigSubFolder);
            if (!string.IsNullOrEmpty(this.Language))
            {
                path = Path.Combine(path, this.Language);
            }
            if (!string.IsNullOrEmpty(this.Area))
            {
                path = Path.Combine(path, this.Area);
            }
            path = Path.GetFullPath(Path.Combine(path, file));
            if (File.Exists(path))
            {
                return XElement.Load((new XPathDocument(path)).CreateNavigator().ReadSubtree());
            }
            else
            {
                return XElement.Parse("<Strings/>");
            }
        }

        public static LangItem Get(string language, string area, string refID)
        {
            ICacheFactory cf = new LangCacheFactory();
            ICache cache = cf.GetCacheManager();
            string keyName = MakeKeyName(language, area, refID);
            LangItem item = cache.Get<LangItem>(keyName);
            if (item != null)
            {
                item.Remove();
                item = null;
            }

            if (item == null)
            {
                item = new LangItem();
                item.Language = language;
                item.Area = area;
                item.RefID = refID;
                item.SetValue(item.GetFromSource());
                if (!item.IsDefaultData())
                {
                    cache.Add(keyName, item);
                }
            }
            return item;
        }

        public static string MakeKeyName(string language, string area, string refID)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}:{3}", typeof(LangItem).Name, language, area, refID);
        }



    }
}
