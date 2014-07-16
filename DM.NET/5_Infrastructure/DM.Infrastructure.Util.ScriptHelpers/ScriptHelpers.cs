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

namespace DM.Infrastructure.Util.ScriptHelpers
{
    public class ScriptHelpers
    {
        /// <summary>
        /// load script
        /// </summary>
        /// <param name="page"></param>
        /// <param name="scriptItem">file name under App_Data/Script</param>
        public static void LoadScript(Page page, string scriptItem)
        {
            IEnumerable<XElement> scriptList = ScriptItem.GetJSList(scriptItem);
            if (scriptList == null || scriptList.Count() == 0)
            {
                return;
            }

            foreach (XElement x in scriptList)
            {
                string value = x.Value;
                if (XmlHelpers.XmlHelpers.GetAttribute(x, "NoOptimize") == "1")
                {
                    Load(page, value, false);
                }
                else
                {
                    Load(page, value);
                }
            }
        }

        public static void Load(Page page, string script)
        {
            Load(page, script, NeedOptimize(page));
        }

        private static void Load(Page page, string script, bool optimize)
        {
            if (!optimize)
            {
                if (ScriptManager.GetCurrent(page) != null)
                {
                    // ensure no duplicate scripts are added to the composite script
                    ScriptReferenceCollection scripts = ScriptManager.GetCurrent(page).Scripts;
                    scripts.Add(new ScriptReference(script));
                }
                else
                    throw new ApplicationException("No script manager registered!");
            }
            else
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
        }

        private static bool NeedOptimize(Page page)
        {
            // find master page class to make sure we're using the base.master
            //bool baseMaster = false;
            //MasterPage master = page.Master;
            //while (master != null && master.Master != null)
            //{
            //    master = master.Master;
            //}
            //if (master != null && master.ToString().IndexOf("base_master", StringComparison.OrdinalIgnoreCase) > -1)
            //{
            //    baseMaster = true;
            //}
            bool baseMaster = true;

            bool optimize = ConfigurationManager.AppSettings["OptimizeJS"] == "1" && baseMaster;
            return optimize;
        }

        public static void CacheScripts(Page page, List<string> scriptList)
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
        public static string GetHash(List<string> scriptList)
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

        public static string GetCacheKey(string setName)
        {
            return "ScriptOptimizer." + setName;
        }

        private static string GetHash(string input)
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
