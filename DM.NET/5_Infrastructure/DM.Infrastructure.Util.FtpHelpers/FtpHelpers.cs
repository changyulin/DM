using System;
using System.IO;
using System.Net;
using System.Xml;

namespace DM.Infrastructure.Util.FtpHelpers
{
    public static class FtpHelpers
    {

        public static XmlReader GetXml(string uri, string userName, string password, int timeout)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Timeout = timeout;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(userName, password);

            using (FtpWebResponse webResponse = (FtpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string strXml = streamReader.ReadToEnd();
                    // load to make sure xml is valid
                    if (!string.IsNullOrEmpty(strXml))
                        XmlHelpers.XmlHelpers.LoadXml(strXml);

                    return XmlHelpers.XmlHelpers.AggregateXml(string.IsNullOrEmpty(strXml) ? "root" : "", strXml);
                }
            }

        }

        public static DateTime GetFileLastModifiedDate(string uri, string userName, string password, int timeout)
        {
            DateTime lastModifiedDate = new DateTime();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Timeout = timeout;
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            request.Credentials = new NetworkCredential(userName, password);
            using (FtpWebResponse webResponse = (FtpWebResponse)request.GetResponse())
            {
                lastModifiedDate = webResponse.LastModified;
            }
            return lastModifiedDate;
        }

        public static Stream GetFile(string uri, string userName, string password, int timeout)
        {
            Stream responseStream = null;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Timeout = timeout;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(userName, password);

            FtpWebResponse webResponse = (FtpWebResponse)request.GetResponse();
            responseStream = webResponse.GetResponseStream();
            return responseStream;
        }

        public static Stream GetByMethod(string uri, string userName, string password, int timeout, string method)
        {
            Stream responseStream = null;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Timeout = timeout;
            request.Method = method;
            request.Credentials = new NetworkCredential(userName, password);

            FtpWebResponse webResponse = (FtpWebResponse)request.GetResponse();
            responseStream = webResponse.GetResponseStream();
            return responseStream;
        }

    }
}
