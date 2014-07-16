using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace DM.Infrastructure.Util.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string GetDocType()
        {
            switch (HttpContext.Current.Request.Browser.Browser)
            {
                case "IE":
                    return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">";
                case "Firefox":
                    return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                default:
                    return ""; // let the browser use it's default?
            }
        }

        /// <summary>
        /// HttpContext.Current.Request.Browser.IsMobileDevice = false for IOS 7 and android devices
        /// </summary>
        /// <returns></returns>
        public static bool IsMobileDevice()
        {
            return HttpContext.Current.Request.Browser.IsMobileDevice || IsIPad() || IsAndroid() || IsIPhone() || IsWinPhone7();
        }

        public static bool IsIE()
        {
            return BrowserName().Equals("IE");
        }

        public static bool IsFirefox()
        {
            return BrowserName().Equals("Firefox");
        }

        public static bool IsSafari()
        {
            return BrowserName().Equals("AppleMAC-Safari");
        }

        public static bool IsChrome()
        {
            return BrowserName().Equals("Chrome");
        }

        public static bool IsWebkit()
        {
            return (IsSafari() || IsChrome());
        }

        public static bool IsIPad()
        {
            return BrowserUserAgent().Contains("iPad");
        }

        public static bool IsIPhone()
        {
            return BrowserUserAgent().Contains("iPhone");
        }

        public static bool IsAndroid()
        {
            return BrowserUserAgent().Contains("Android");
        }

        public static bool IsWinPhone7()
        {
            string userAgent = BrowserUserAgent();
            return (userAgent.Contains("MSIE 7.0") && userAgent.Contains("Windows Phone OS 7.0"));
        }

        public static string BrowserName()
        {
            return HttpContext.Current.Request.Browser.Browser;
        }

        public static int BrowserMajorVersion()
        {
            return HttpContext.Current.Request.Browser.MajorVersion;
        }

        public static string BrowserUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// Is device supported for this element?  Returns true if "Device" is specified and we match device.
        /// Returns true if "NotDevice" is specified and we do not match device.
        /// "Device" and "NotDevice" contain a semicolon delimited list of devices.
        /// </summary>
        /// <param name="elem">Element containing "Device" or "NotDevice"</param>
        /// <returns>True if this device is supported for this element.</returns>
        public static bool DeviceSupported(XElement elem)
        {
            bool passed = true;

            string reqdDevice = elem.Attribute("Device") != null ? elem.Attribute("Device").Value : null;
            if (!string.IsNullOrEmpty(reqdDevice))
            {
                passed = IsDeviceSupported(reqdDevice);

                return passed;
            }

            string notReqdDevice = elem.Attribute("NotDevice") != null ? elem.Attribute("NotDevice").Value : null;
            if (!string.IsNullOrEmpty(notReqdDevice))
            {
                passed = !IsDeviceSupported(notReqdDevice);

                return passed;
            }

            return true;        // device is supported by default
        }

        private static bool IsDeviceSupported(string devices)
        {
            bool passed = true;

            foreach (string device in devices.ToLower().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                switch (device)
                {
                    // can add more tests over time
                    case "mobile": passed = HtmlHelpers.IsMobileDevice(); break;
                    case "ipad": passed = HtmlHelpers.IsIPad(); break;
                }

                if (passed)
                    break;
            }

            return passed;
        }
    }
}
