﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DM.Infrastructure.Cache;
using DM.Infrastructure.Util.ScriptHelpers;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace DM.WebUI.HttpHander.HttpHanderCommon
{
    /// <summary>
    /// ScriptOptimizer 的摘要说明
    /// </summary>
    public class ScriptOptimizer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            string hash = request["hash"];
            if (!string.IsNullOrEmpty(hash))
            {
                ICacheManager cache = ScriptCacheFactory.GetCacheManager();
                string keyName = ScriptHelpers.GetCacheKey(hash);
                byte[] responseBytes = cache[keyName] as byte[];
                if (responseBytes != null)
                {
                    HttpResponse response = context.Response;

                    response.AppendHeader("Content-Length", responseBytes.Length.ToString(CultureInfo.InvariantCulture));
                    response.ContentType = "text/javascript";
                    response.Cache.SetCacheability(HttpCacheability.Public);
                    response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
                    response.Cache.SetMaxAge(TimeSpan.FromDays(30));
                    response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

                    response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}