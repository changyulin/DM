using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using DM.Infrastructure.Log;


namespace DM.Infrastructure.Util.HttpHelpers
{
    public static class HttpHelpers
    {
        
        public static XmlReader GetXml(string uri, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return GetXml(uriObj, timeout);
        }

        public static XmlReader GetXml(string uri, int timeout, bool ignoreHTTPSCert)
        {
            if (ignoreHTTPSCert)
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            return GetXml(uri, timeout);
        }

        public static XmlReader GetXml(Uri uri, int timeout)
        {
            string strXml = GetText(uri, timeout);
            return XmlHelpers.XmlHelpers.AggregateXml(strXml.Length == 0 ? "root" : "", strXml.ToString());
        }

        public static string GetText(string uri, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return GetText(uriObj, timeout);
        }
        public static string GetTextWithoutToken(string uri, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return GetText(uriObj, timeout);
        }
        public static string GetText(Uri uri, int timeout)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Timeout = timeout;
            webRequest.Method = "GET";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                CopyCookies(webRequest, HttpContext.Current.Request);

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (BufferedStream streamReader = new BufferedStream(webResponse.GetResponseStream(), 1024 * 1024))
                    {
                        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                        StringBuilder strXml = new StringBuilder();
                        byte[] buffer = new byte[1024 * 1024];
                        int charsRead;

                        while ((charsRead = streamReader.Read(buffer, 0, buffer.Length)) > 0)
                            strXml.Append(enc.GetString(buffer, 0, charsRead));

                        return strXml.ToString();
                    }
                }
            }
            catch (WebException e)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Exception calling {0}.", uri.ToString()), e);
                throw;
            }
        }

        public static void Delete(string uri, int timeout = 180000)
        {
            Uri uriObj = new Uri(uri);
            Delete(uriObj, timeout);
        }

        public static void Delete(Uri uri, int timeout = 180000)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Timeout = timeout;
            webRequest.Method = "DELETE";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                CopyCookies(webRequest, HttpContext.Current.Request);
            }

            try
            {
                webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Exception calling {0} for DELETE", uri.ToString()), ex);
                throw;
            }

        }

        public static XElement PutXmlReturnXml(string uri, XElement input, int timeout = 180000)
        {
            using (XmlReader xdr = input.CreateReader())
            {
                using (XmlReader xdrRes = PutXmlReturnXml(uri, xdr, timeout))
                {
                    return XElement.Load(xdrRes);
                }
            }
        }

        public static XmlReader PutXmlReturnXml(string uri, XmlReader reader, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return PutXmlReturnXml(uriObj, reader, timeout);
        }

        public static XmlReader PutXmlReturnXml(Uri uri, XmlReader reader, int timeout)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Timeout = timeout;
            webRequest.Method = "PUT";
            webRequest.ContentType = "text/xml";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                CopyCookies(webRequest, HttpContext.Current.Request);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            using (Stream stream = webRequest.GetRequestStream())
            {
                xmlDoc.Save(stream);
            }

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string strXml = streamReader.ReadToEnd();

                        try
                        {
                            // load to make sure xml is valid
                            if (!string.IsNullOrEmpty(strXml))
                                XmlHelpers.XmlHelpers.LoadXml(strXml);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("Invalid xml response:\n" + strXml + "\nInput xml:\n" + xmlDoc.OuterXml,ex);
                            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                                LogHelper.Error("Cookies:\n" + HttpContext.Current.Request.ServerVariables["HTTP_COOKIE"],ex);
                            throw new ApplicationException("Invalid XML");
                        }

                        // dummy up a root node if nothing to return to give valid xml
                        return XmlHelpers.XmlHelpers.AggregateXml(string.IsNullOrEmpty(strXml) ? "root" : "", strXml);
                    }
                }
            }
            catch (WebException e)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Exception calling {0} with xml:\n{1}", uri.ToString(), xmlDoc.OuterXml), e);
                throw;
            }
        }

        public static XmlReader PostXmlReturnXml(string uri, XmlReader reader, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return PostXmlReturnXml(uriObj, reader, timeout);
        }

        /// <summary>
        /// Igore invalid certificate for HTTPS request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// send request with ignoreHTTPSCert option
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="reader"></param>
        /// <param name="timeout"></param>
        /// <param name="ignoreHTTPSCert">true: always send the request even the HTTPS certficate is invalid. false: validate HTTPS certificate </param>
        /// <returns></returns>
        public static XmlReader PostXmlReturnXml(string uri, XmlReader reader, int timeout, bool ignoreHTTPSCert)
        {
            if (ignoreHTTPSCert)
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            return PostXmlReturnXml(uri, reader, timeout);
        }



        public static XmlReader PostXmlReturnXml(Uri uri, XmlReader reader, int timeout)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Timeout = timeout;
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                CopyCookies(webRequest, HttpContext.Current.Request);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            using (Stream stream = webRequest.GetRequestStream())
            {
                xmlDoc.Save(stream);
            }

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string strXml = streamReader.ReadToEnd();

                        try
                        {
                            // load to make sure xml is valid
                            if (!string.IsNullOrEmpty(strXml))
                                XmlHelpers.XmlHelpers.LoadXml(strXml);
                        }
                        catch (Exception)
                        {
                            StringBuilder cookieStr = new StringBuilder();
                            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
                            for (int i = 0; i < cookies.Count; i++)
                            {
                                cookieStr.AppendFormat(CultureInfo.InvariantCulture, "\n{0}={1}", cookies[i].Name, cookies[i].Value);
                            }
                            throw new ApplicationException("Invalid XML");
                        }

                        // dummy up a root node if nothing to return to give valid xml
                        return XmlHelpers.XmlHelpers.AggregateXml(string.IsNullOrEmpty(strXml) ? "root" : "", strXml);
                    }
                }
            }
            catch (WebException e)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Exception calling {0} with xml:\n{1}", uri.ToString(), xmlDoc.OuterXml), e);
                throw;
            }
        }

        public static XElement PostXmlReturnXml(string uri, XElement input, int timeout = 180000)
        {
            using (XmlReader xdr = input.CreateReader())
            {
                using (XmlReader xdrRes = PostXmlReturnXml(uri, xdr, timeout))
                {
                    return XElement.Load(xdrRes);
                }
            }
        }

        public static byte[] PostXmlReturnBinary(string uri, XmlReader reader, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return PostXmlReturnBinary(uriObj, reader, timeout);
        }

        public static byte[] PostXmlReturnBinary(Uri uri, XmlReader reader, int timeout)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Timeout = timeout;
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                CopyCookies(webRequest, HttpContext.Current.Request);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            using (Stream stream = webRequest.GetRequestStream())
            {
                xmlDoc.Save(stream);
            }

            using (StreamReader sreader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    sreader.BaseStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
        }

        public static byte[] PostFormReturnBinary(string url, Dictionary<string, string> paras, int timeout)
        {
            Uri uri = new Uri(url);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Timeout = timeout;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                CopyCookies(webRequest, HttpContext.Current.Request);

            using (Stream stream = webRequest.GetRequestStream())
            {
                StringBuilder postData = new StringBuilder(1000);
                foreach (var para in paras)
                {
                    if (postData.Length > 0)
                        postData.Append("&");
                    postData.Append(para.Key + "=" + HttpUtility.UrlEncode(para.Value));
                }
                byte[] buffer = Encoding.UTF8.GetBytes(postData.ToString());
                stream.Write(buffer, 0, buffer.Length);
            }

            using (StreamReader sreader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    sreader.BaseStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
        }

        public static byte[] GetBinary(string uri, int timeout)
        {
            Uri uriObj = new Uri(uri);
            return GetBinary(uriObj, timeout);
        }

        public static byte[] GetBinary(Uri uri, int timeout)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Timeout = timeout;
            webRequest.Method = "GET";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                CopyCookies(webRequest, HttpContext.Current.Request);

            using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
        }

        public static byte[] GetBinaryWithCredentials(string uri, NetworkCredential cred, int timeout)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Timeout = timeout;
            webRequest.Method = "GET";
            webRequest.Credentials = cred;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                CopyCookies(webRequest, HttpContext.Current.Request);

            using (StreamReader reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
        }

        public static byte[] PostReturnPDFBinary(string uri, string postParams, int timeout)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Timeout = timeout;
            webRequest.Method = "POST";
            webRequest.ContentType = "text/PDF";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                CopyCookies(webRequest, HttpContext.Current.Request);
            }

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postParams.ToString());
            webRequest.ContentLength = bytes.Length;

            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader sreader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        using (var memstream = new MemoryStream())
                        {
                            sreader.BaseStream.CopyTo(memstream);
                            return memstream.ToArray();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Exception calling {0} with xml:\n{1}", uri.ToString(), postParams), e);
                throw;
            }
        }

        public static string PostData(string url, string postParams, int timeout = 60000)
        {
            string data = string.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.Headers["Cookie"] = HttpContext.Current != null ? HttpContext.Current.Request.ServerVariables["HTTP_COOKIE"] : "";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                req.Timeout = timeout;

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postParams.ToString());
                req.ContentLength = bytes.Length;

                using (Stream s = req.GetRequestStream())
                    s.Write(bytes, 0, bytes.Length);

                WebResponse resp = req.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    data = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format("Error retrieving data using SearchCriteriaProxy.\n{0}\n{1}\n", url, postParams.ToString()), e);
                throw;
            }
            return data;
        }

        //public static object GenericPostData(Dictionary<string, string> postParams, string Url, bool passCookies, HttpRequest currentRequest, ref string type, NetworkCredential cred)
        //{
        //    string postData = "";
        //    int ct = 0;
        //    object rt = "Nothing Returned!";
        //    string contentType = "application/x-www-form-urlencoded";

        //    type = "text/plain";
        //    foreach (string k in postParams.Keys)
        //    {
        //        //alternate post XML
        //        if (k == "text/xml")
        //        {
        //            postData = postParams[k];
        //            contentType = "text/xml";
        //            break;
        //        }
        //        else
        //        {
        //            if (ct > 0)
        //            {
        //                postData += "&";
        //            }
        //            postData = postData + k + "=" + postParams[k];
        //            ct++;
        //        }
        //    }

        //    // Create a 'HttpWebRequest' object for the specified url.
        //    HttpWebRequest httpWebRequest = null;
        //    HttpWebResponse httpWebResponse = null;
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(Url))
        //        {
        //            Uri uri = new Uri(Url);

        //            byte[] postDataBuffer = Encoding.ASCII.GetBytes(postData);// ("data=194596");


        //            httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        //            httpWebRequest.KeepAlive = true;
        //            httpWebRequest.Timeout = 1000 * 60 * 15;
        //            httpWebRequest.Method = "POST";
        //            httpWebRequest.ProtocolVersion = new Version(1, 0);
        //            httpWebRequest.ContentType = contentType;
        //            httpWebRequest.ContentLength = postDataBuffer.Length;
        //            httpWebRequest.Credentials = cred; //credential/ no cookies
        //            if (passCookies && currentRequest != null && currentRequest.Cookies.Count > 0)
        //            {
        //                for (int i = 0; i < currentRequest.Cookies.Count; i++)
        //                {
        //                    Cookie oC = new Cookie();    // Convert between the System.Net.Cookie to a System.Web.HttpCookie
        //                    oC.Domain = httpWebRequest.RequestUri.Host;
        //                    oC.Expires = currentRequest.Cookies[i].Expires;
        //                    oC.Name = currentRequest.Cookies[i].Name;
        //                    oC.Path = currentRequest.Cookies[i].Path;
        //                    oC.Secure = currentRequest.Cookies[i].Secure;
        //                    oC.Value = currentRequest.Cookies[i].Value;
        //                    try
        //                    {
        //                        httpWebRequest.CookieContainer.Add(oC);
        //                    }
        //                    catch (Exception)
        //                    {
        //                    }
        //                }
        //            }

        //            //write request data 
        //            using (Stream requestStream = httpWebRequest.GetRequestStream())
        //            {
        //                requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);
        //                requestStream.Close();
        //            }

        //            //send request to server and wait for response.
        //            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        //            using (Stream responseStream = httpWebResponse.GetResponseStream())
        //            {
        //                using (BinaryReader br = new BinaryReader(responseStream))
        //                {
        //                    if (br.BaseStream.CanRead)
        //                    {
        //                        type = httpWebResponse.ContentType;
        //                        int l = Convert.ToInt32(httpWebResponse.ContentLength);
        //                        if (l < 1000)
        //                            l = 2000000;

        //                        rt = br.ReadBytes(l + 100);
        //                    }
        //                    br.Close();
        //                }
        //                responseStream.Close();
        //            }
        //        }
        //        else
        //            rt = "No URL!";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (httpWebResponse != null)
        //            httpWebResponse.Close();
        //        if (httpWebRequest != null)
        //            httpWebRequest.Abort();
        //        httpWebResponse = null;
        //        httpWebRequest = null;
        //    }

        //    return rt;
        //}

        public static object GenericPostData(string Url, NetworkCredential cred)
        {
            string postData = "";
            //int ct = 0;
            object rt = "Nothing Returned!";
            string contentType = "application/x-www-form-urlencoded";


            // Create a 'HttpWebRequest' object for the specified url.
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (!String.IsNullOrEmpty(Url))
                {
                    Uri uri = new Uri(Url);

                    byte[] postDataBuffer = Encoding.ASCII.GetBytes(postData);// ("data=194596");


                    httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                    httpWebRequest.KeepAlive = true;
                    httpWebRequest.Timeout = 1000 * 60 * 15;
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ProtocolVersion = new Version(1, 0);
                    httpWebRequest.ContentType = contentType;
                    httpWebRequest.ContentLength = postDataBuffer.Length;
                    httpWebRequest.Credentials = cred; //credential/ no cookies


                    //write request data 
                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);
                        requestStream.Close();
                    }

                    //send request to server and wait for response.
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (BinaryReader br = new BinaryReader(responseStream))
                        {
                            if (br.BaseStream.CanRead)
                            {
                                //type = httpWebResponse.ContentType;
                                int l = Convert.ToInt32(httpWebResponse.ContentLength);
                                if (l < 1000)
                                    l = 2000000;

                                rt = br.ReadBytes(l + 100);
                            }
                            br.Close();
                        }
                        responseStream.Close();
                    }
                }
                else
                    rt = "No URL!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (httpWebResponse != null)
                    httpWebResponse.Close();
                if (httpWebRequest != null)
                    httpWebRequest.Abort();
                httpWebResponse = null;
                httpWebRequest = null;
            }

            return rt;
        }

        public static DateTime GetFileLastModifiedDate(string uri, int timeout)
        {
            DateTime lastModifiedDate = new DateTime();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Timeout = timeout;
            webRequest.Method = "GET";

            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    lastModifiedDate = webResponse.LastModified;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Format(CultureInfo.InvariantCulture, "Error retrieving {0 }file.", uri), e);
                throw;
            }
            return lastModifiedDate;
        }

        /// <summary>
        /// Get the post param from input stream.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>post param xml</returns>
        public static XDocument GetPostStreamInput(HttpContext context)
        {
            Stream stream = context.Request.InputStream;
            string str = string.Empty;
            XDocument xdoc = XDocument.Parse("<root/>");
            if (stream.Length != 0)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    str = context.Server.UrlDecode(sr.ReadToEnd());
                    xdoc = XDocument.Parse(str);
                }
            }
            return xdoc;
        }

        public static string GetQueryValue(string queryStr, string key)
        {
            string[] qArray = queryStr.Split('&');
            for (int i = 0; i < qArray.Length; i++)
            {
                if (qArray[i] == "")
                    continue;
                string[] kv = qArray[i].Split('=');
                if (kv[0].ToLower(CultureInfo.InvariantCulture) == key.ToLower(CultureInfo.InvariantCulture))
                {
                    return kv[1];
                }
            }
            return null;
        }

        public static string RemoveQueryValue(string queryStr, string removeKey)
        {
            string[] qArray = queryStr.Split('&');
            List<string> copyArray = new List<string>();
            for (int i = 0; i < qArray.Length; i++)
            {
                if (qArray[i] == "")
                    continue;
                string[] kv = qArray[i].Split('=');
                if (kv[0].ToLower(CultureInfo.InvariantCulture) != removeKey.ToLower(CultureInfo.InvariantCulture))
                {
                    copyArray.Add(qArray[i]);
                }
            }
            return string.Join("&", copyArray);
        }

        public static void CopyCookies(HttpWebRequest webRequest, HttpRequest httpRequest)
        {
            if (httpRequest != null && webRequest != null)
            {
                webRequest.CookieContainer = new CookieContainer();
                string[] cookieKeys = new string[] { "advtsession", "advtsecure", "advtnonsecure" };
                foreach (string key in cookieKeys)
                {
                    if (httpRequest.Cookies[key] != null)
                        webRequest.CookieContainer.Add(new Cookie(key, httpRequest.Cookies[key].Value, "/", webRequest.RequestUri.Host));
                    else //if cookie is not passed, those values should be available in post request
                    {
                        if (httpRequest.Form[key] != null)
                            webRequest.CookieContainer.Add(new Cookie(key, httpRequest.Form[key], "/", webRequest.RequestUri.Host));
                    }
                }
            }
        }
        public static List<Cookie> getRemoteServerCookie(string url)
        {
            try
            {
                List<Cookie> result = new List<Cookie>();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                CookieContainer cookies = new CookieContainer();
                request.Method = "get";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.CookieContainer = cookies;
                Uri newUri = new Uri(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    foreach (Cookie activeCookie in request.CookieContainer.GetCookies(newUri))
                    {
                        result.Add(activeCookie);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("Error in getRemoteServerCookie.\n url: {0}", url), ex);
                throw;
            }
        }
        public static void AppendToLog(string url, int timeout = 100000)
        {
            HttpContext hc = HttpContext.Current;
            Uri uriObj = new Uri(url);
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uriObj);
            wr.Timeout = timeout;
            if (hc != null && hc.Request != null)
            {
                //copy cookies
                CopyCookies(wr, hc.Request);
            }
            else
            {
                LogHelper.Debug("Log url don't get cookies:" + uriObj.ToString());
            }
            wr.GetResponse().Close();
            if (wr != null)
            {
                wr.Abort();
                wr = null;
            }
        }
    }
}
