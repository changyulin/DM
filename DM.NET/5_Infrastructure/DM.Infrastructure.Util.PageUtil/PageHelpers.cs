using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DM.Infrastructure.Cache;
using DM.Infrastructure.Log;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace DM.Infrastructure.Util.PageUtil
{
    public class PageHelpers
    {
        /// <summary>
        /// Get Cache Key
        /// </summary>
        /// <param name="setName">hash value</param>
        /// <returns></returns>
        public static string GetCacheKey(string hash)
        {
            return "Optimizer." + hash;
        }

        /// <summary>
        /// add the url to page item
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="itemKey">Page.Item's key</param>
        /// <param name="url">url</param>
        public static void Add(Page page, string itemKey, string url)
        {
            string scriptPath = page.ResolveUrl(url);

            List<string> list = page.Items[itemKey] as List<string>;
            if (list == null)
            {
                list = new List<string>();
                page.Items[itemKey] = list;
            }

            if (!list.Contains(scriptPath))
                list.Add(scriptPath);
        }

        /// <summary>
        /// add these url's content to cache
        /// </summary>
        /// <param name="cache">cache manager</param>
        /// <param name="urlList">url list</param>
        public static void Cache(ICache cache, List<string> urlList)
        {
            string keyName = GetCacheKey(GetHash(urlList));
            if (!cache.Contains(keyName))
            {
                List<string> urlListCopy = new List<string>();
                foreach (string urlName in urlList)
                {
                    urlListCopy.Add(urlName);
                }
                // add to cache
                using (MemoryStream memoryStream = new MemoryStream(8192))
                {
                    byte[] newline = new byte[] { 13, 10 };

                    foreach (string url in urlListCopy)
                    {
                        byte[] fileBytes;
                        if (url.IndexOf("http:", StringComparison.OrdinalIgnoreCase) > -1 || url.IndexOf("https:", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            fileBytes = HttpHelpers.HttpHelpers.GetBinary(url, 10000);
                        }
                        else
                        {
                            string filename = HttpContext.Current.Server.MapPath(url);
                            fileBytes = File.ReadAllBytes(filename);
                        }

                        // check for unicode BOM
                        if (fileBytes.Length > 2 && fileBytes[0] == 0xEF && fileBytes[1] == 0xBB && fileBytes[2] == 0xBF)
                            memoryStream.Write(fileBytes, 3, fileBytes.Length - 3);
                        else
                            memoryStream.Write(fileBytes, 0, fileBytes.Length);
                        memoryStream.Write(newline, 0, newline.Length); // add a new line in case file doesn't end with one

                    }

                    cache.Add(keyName, memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// combine file names and creation times to get a hash value
        /// </summary>
        /// <param name="urlList">url list</param>
        /// <returns>hash value</returns>
        public static string GetHash(List<string> urlList)
        {
            StringBuilder allNames = new StringBuilder();
            foreach (string url in urlList)
            {
                string fullPath = string.Empty;

                if (url.IndexOf("http:", StringComparison.OrdinalIgnoreCase) > -1 || url.IndexOf("https:", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    DateTime lastModifyData = HttpHelpers.HttpHelpers.GetFileLastModifiedDate(url, 10000);

                    if (lastModifyData != null)
                        allNames.Append(url).Append("|").Append(lastModifyData.Ticks.ToString(CultureInfo.InvariantCulture)).Append("|");
                    else
                        LogHelper.Debug("File not found: " + url);
                }
                else
                {
                    fullPath = HttpContext.Current.Server.MapPath(url);

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
