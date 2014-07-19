using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using DM.Infrastructure.Util.ScriptHelpers;
using DM.Infrastructure.Log;
using DM.Infrastructure.Cache;
using System.Globalization;
using System.Security.Cryptography;
using DM.Infrastructure.Util.HttpHelpers;
using DM.Infrastructure.Util.XmlHelpers;
using System.Configuration;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;

namespace DM.Infrastructure.Util.ScriptHelpers
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
                    Add(page, x.Value);
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
                    ScriptHelpers.CacheScripts(page, scriptList);
                    string url = string.Format("{0}?hash={1}",
                        page.ResolveClientUrl("~/scriptoptimizer.ashx"),
                        HttpUtility.UrlEncode(ScriptHelpers.GetHash(scriptList)));
                    page.ClientScript.RegisterClientScriptInclude("baseall", url);
                }
                else
                {
                    foreach (string scriptUrl in scriptList)
                    {
                        HtmlGenericControl scriptHtml = new HtmlGenericControl("script");
                        scriptHtml.Attributes.Add("type", "text/javascript");
                        scriptHtml.Attributes.Add("src", scriptUrl);
                        page.Header.Controls.Add(scriptHtml);
                    }
                }
            }
        }

        /// <summary>
        /// Get Cache Key
        /// </summary>
        /// <param name="setName">hash value</param>
        /// <returns></returns>
        public static string GetCacheKey(string setName)
        {
            return "ScriptOptimizer." + setName;
        }

        private static void Add(Page page, string script)
        {
            // get/create a string list in the page items collection that has the script path
            string scriptPath = page.ResolveUrl(script);

            List<string> list = page.Items["ScriptList"] as List<string>;
            if (list == null)
            {
                list = new List<string>();
                page.Items["ScriptList"] = list;
            }

            if (!list.Contains(scriptPath))
                list.Add(scriptPath);
        }

        private static void CacheScripts(Page page, List<string> scriptList)
        {
            ICacheManager cache = ScriptCacheFactory.GetCacheManager();
            string keyName = GetCacheKey(GetHash(scriptList));
            if (!cache.Contains(keyName))
            {
                // a bit of a hack.... move any scripts that have _last. in their name to the end
                // this is so we can override behavior
                // multiple scripts ending in _last order is undetermined
                List<string> scriptListCopy = new List<string>();
                foreach (string scriptName in scriptList)
                {
                    scriptListCopy.Add(scriptName);
                }
                // add scripts to cache
                using (MemoryStream memoryStream = new MemoryStream(8192))
                {
                    byte[] newline = new byte[] { 13, 10 };

                    foreach (string script in scriptListCopy)
                    {
                        byte[] fileBytes;
                        if (script.IndexOf("http:", StringComparison.OrdinalIgnoreCase) > -1 || script.IndexOf("https:", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            fileBytes = HttpHelpers.HttpHelpers.GetBinary(script, 10000);
                        }
                        else
                        {
                            string filename = HttpContext.Current.Server.MapPath(script);
                            fileBytes = File.ReadAllBytes(filename);
                        }

                        // check for unicode BOM
                        if (fileBytes.Length > 2 && fileBytes[0] == 0xEF && fileBytes[1] == 0xBB && fileBytes[2] == 0xBF)
                            memoryStream.Write(fileBytes, 3, fileBytes.Length - 3);
                        else
                            memoryStream.Write(fileBytes, 0, fileBytes.Length);
                        memoryStream.Write(newline, 0, newline.Length); // add a new line in case file doesn't end with one

                        LogHelper.Debug(script);
                    }

                    cache.Add(keyName, memoryStream.ToArray());
                }
            }
        }

        // combine file names and creation times to get a hash value
        private static string GetHash(List<string> scriptList)
        {
            StringBuilder allNames = new StringBuilder();
            foreach (string script in scriptList)
            {
                string fullPath = string.Empty;

                if (script.IndexOf("http:", StringComparison.OrdinalIgnoreCase) > -1 || script.IndexOf("https:", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    DateTime lastModifyData = HttpHelpers.HttpHelpers.GetFileLastModifiedDate(script, 10000);

                    if (lastModifyData != null)
                        allNames.Append(script).Append("|").Append(lastModifyData.Ticks.ToString(CultureInfo.InvariantCulture)).Append("|");
                    else
                        LogHelper.Debug("File not found: " + script);
                }
                else
                {
                    fullPath = HttpContext.Current.Server.MapPath(script);

                    if (File.Exists(fullPath))
                        allNames.Append(fullPath).Append("|").Append(File.GetLastWriteTime(fullPath).Ticks.ToString(CultureInfo.InvariantCulture)).Append("|");
                    else
                        LogHelper.Debug("File not found: " + fullPath);
                }
            }

            return GetHash(allNames.ToString());
        }

        private static string GetHash(string input)
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
