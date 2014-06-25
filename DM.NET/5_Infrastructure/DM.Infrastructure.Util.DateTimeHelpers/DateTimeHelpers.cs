using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DM.Infrastructure.Util.DateTimeHelpers
{
    /// <summary>
    /// Date/Time helper methods
    /// 
    /// A list of time zone ID's can be found in the TimeZone.xml strings file
    /// Here's another source: http://blogs.msdn.com/b/kathykam/archive/2006/09/28/776458.aspx
    /// </summary>
    public static class DateTimeHelpers
    {

        public static Dictionary<DateTime, DateTime> LastDailyEndDateParam = new Dictionary<DateTime, DateTime>();
        public static DateTime LastDailyEndDate { get; set; }

        /// <summary>
        /// Convert a UTC time to a specific timezone
        /// </summary>
        /// <param name="dtUTC">UTC time</param>
        /// <param name="localTimeZoneID">local timezone ID</param>
        /// <returns>Local time</returns>
        public static DateTime ToLocal(DateTime dtUTC, string localTimeZoneID)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dtUTC, TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneID));
        }

        /// <summary>
        /// Convert a local time to UTC
        /// </summary>
        /// <param name="dtLocal">Local time</param>
        /// <param name="localTimeZoneID">Local timezone ID</param>
        /// <returns></returns>
        public static DateTime ToUTC(DateTime dtLocal, string localTimeZoneID)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dtLocal, TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneID));
        }

        /// <summary>
        /// Does a timezone support daylight savings?
        /// </summary>
        /// <param name="localTimeZoneID">Local timezone ID</param>
        /// <returns>True if daylight savings is supported</returns>
        public static bool SupportsDaylightSavings(string localTimeZoneID)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneID).SupportsDaylightSavingTime;
        }

        /// <summary>
        /// Get the current time for a local timezone
        /// </summary>
        /// <param name="localTimeZoneID">Local timezone ID</param>
        /// <returns>Local time for a given timezone</returns>
        public static DateTime DateTimeLocal(string localTimeZoneID)
        {
            return ToLocal(DateTime.UtcNow, localTimeZoneID);
        }

        /// <summary>
        /// Get yesterday's date
        /// </summary>
        /// <returns>Yesterday's date</returns>
        public static DateTime GetYesterday()
        {
            return DateTime.Now.AddDays(-1);
        }
        /// <summary>
        /// Get currentday's date
        /// </summary>
        /// <returns>currentday's date</returns>
        public static DateTime GetCurrentDay()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// Get current year
        /// </summary>
        /// <returns>current year</returns>
        public static string GetCurrentYear()
        {
            return DateTime.Now.Year.ToString();
        }
        /// <summary>
        /// Get the system list of timezones
        /// </summary>
        /// <returns>List of known timezones</returns>
        public static ReadOnlyCollection<TimeZoneInfo> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }




        /// <summary>
        /// Get the last day of the previous month from a given years ago
        /// </summary>
        /// <param name="backYears">How many years back</param>
        /// <returns>Last month end date</returns>
        public static DateTime GetLastMonthEndDate(int backYears)
        {
            DateTime now = DateTime.Now;
            // get day before the first of this month
            return new DateTime(now.Year - backYears, now.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// Get the last day of the given date
        /// </summary>
        /// <param name="date">given date</param>
        /// <returns>last day of the month date</returns>
        public static DateTime GetLastMonthEndDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// get the month end date of the give date
        /// </summary>
        /// <param name="date">2012-5-2</param>
        /// <returns>2012-5-31</returns>
        public static DateTime GetMonthEndDate(DateTime date)
        {
            DateTime newDate = date.AddMonths(1);
            return new DateTime(newDate.Year, newDate.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// Get the last quarter end date from a given years ago
        /// </summary>
        /// <param name="backYears">How many years back</param>
        /// <returns>Last quarter end date</returns>
        public static DateTime GetLastQuarterEndDate(int backYears)
        {
            DateTime now = DateTime.Now;
            int thisQuarterStart = 0;
            switch (now.Month)
            {
                case 1:
                case 2:
                case 3:
                    thisQuarterStart = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    thisQuarterStart = 4;
                    break;
                case 7:
                case 8:
                case 9:
                    thisQuarterStart = 7;
                    break;
                case 10:
                case 11:
                case 12:
                    thisQuarterStart = 10;
                    break;
            }

            // get day before the first day of this quarter
            return new DateTime(now.Year - backYears, thisQuarterStart, 1).AddDays(-1);
        }
    }
}
