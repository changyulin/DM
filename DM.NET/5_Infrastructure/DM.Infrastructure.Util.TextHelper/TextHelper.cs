using System;
using System.Text.RegularExpressions;
using System.Web;

namespace DM.Infrastructure.Util.TextHelper
{
    public class TextHelper
    {
        private static TextHelper bll = null;
        private static object instanceObj = new object();

        public TextHelper()
        {
        }

        /// <summary>
        /// 文本帮助类实例
        /// </summary>
        public static TextHelper Instance
        {
            get
            {
                if (bll == null)
                {
                    lock (instanceObj)
                    {
                        if (bll == null)
                        {
                            bll = new TextHelper();
                        }
                    }
                }
                return bll;
            }
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码</param>
        /// <returns>已经去除后的文字</returns>
        public string NoHtml(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
              RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpUtility.HtmlEncode(Htmlstring).Trim();
            
            return Htmlstring;
        }

        /// <summary>
        /// 转换为人民币大写
        /// </summary>
        /// <param name="strAmount"></param>
        /// <returns></returns>
        public string ConvertAmount(string strAmount)
        {
            string strTemp = "";
            string s = double.Parse(strAmount).ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            strTemp = Regex.Replace(d, ".", delegate(Match m) { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });

            return strTemp + "整";
        }

        /// <summary>
        /// 去掉邮件地址中的如：(财经线)字符
        /// </summary>
        /// <param name="oriAd"></param>
        /// <returns></returns>
        public static string ConvertToRealAd(string oriAd)
        {
            string address = oriAd;
            try
            {
                Regex reg = new Regex(@"\(.*?\)");
                address = reg.Replace(oriAd, "");
            }
            catch
            {
            }
            return address;
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            else
                return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 判断是否为整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            else
                return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 检查是否为日期型
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static bool IsDate(string strDate)
        {
            bool bl = false;
            if (String.IsNullOrEmpty(strDate))
                bl = false;
            else
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(strDate);
                    bl = true;
                }
                catch
                {
                    bl = false;
                }
            }

            return bl;
        }

        /// <summary>
        /// 转换日期格式并且返回
        /// </summary>
        /// <param name="strDate">要转换的字符串</param>
        /// <param name="strFormat">日期格式</param>
        /// <returns>如果为日期则转换为指定的格式，否则返回原字符串</returns>
        public static string ConvertDateFormat(string strDate, string strFormat)
        {
            string strTemp = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(strDate))
                    strTemp = Convert.ToDateTime(strDate).ToString(strFormat);
            }
            catch
            {
                strTemp = strDate;
            }
            return strTemp;
        }

        /// <summary>
        /// 比较两个日期是否相同
        /// </summary>
        /// <param name="strDate1"></param>
        /// <param name="strDate2"></param>
        /// <returns></returns>
        public static bool CompareDateEqual(string strDate1, string strDate2)
        {
            bool bl = false;
            try
            {
                DateTime dt1 = Convert.ToDateTime(Convert.ToDateTime(strDate1).ToString("yyyy-MM-dd"));
                DateTime dt2 = Convert.ToDateTime(Convert.ToDateTime(strDate2).ToString("yyyy-MM-dd"));

                bl = DateTime.Equals(dt1, dt2);
            }
            catch
            {
                bl = false;
            }
            return bl;
        }
    }
}
