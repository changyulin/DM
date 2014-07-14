using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using DM.Infrastructure.Cache;

namespace DM.Infrastructure.Util.LangHelpers
{
    public class LangHelpers
    {
        /// <summary>
        /// get the string value by id
        /// </summary>
        /// <param name="stringID">string id</param>
        /// <param name="category">file name</param>
        /// <returns></returns>
        public static string GetString(string stringID, string category)
        {
            string langCult = GetLangCult();
            string[] langCultTemp = langCult.Split('-');
            string language = langCultTemp.Length > 0 ? langCultTemp[0] : string.Empty;
            string area = langCultTemp.Length > 1 ? langCultTemp[1] : string.Empty;
            XElement langXml = LangCache.Get(language, area, category).Value;
            XElement result = langXml.XPathSelectElement("String[@id='" + stringID + "']");
            if (result != null)
            {
                return result.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get language and culture from the current context
        /// </summary>
        /// <returns>Language/culture such as "en-US"</returns>
        public static string GetLangCult()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString();
        }

        /// <summary>
        /// Set language and culture on the current context
        /// </summary>
        /// <param name="langCult">Language/culture such as "en-US"</param>
        public static void SetLangCult(string langCult)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langCult);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCult);
        }
    }
}
