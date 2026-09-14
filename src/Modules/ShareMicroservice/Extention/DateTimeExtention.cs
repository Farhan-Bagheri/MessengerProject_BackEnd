using System;
using System.Globalization;

namespace BusNet.Query.Extensions;

public static class DateTimeExtension
{
    public static DateTime ToMilady(this string persianaDate)
    {
        try
        {
            if (string.IsNullOrEmpty(persianaDate)) return DateTime.Now;

            persianaDate = persianaDate.Trim();
            DateTime result;
            string[] dateTimeParts = persianaDate.Split(' ');
            string dateStr = dateTimeParts.Length >= 1 ? dateTimeParts[0] : string.Empty;
            string timeStr = dateTimeParts.Length > 1 ? dateTimeParts[1] : string.Empty;

            if (!string.IsNullOrEmpty(timeStr))
            {
                // تقسیم کردن رشته تاریخ به سه قسمت: سال، ماه، روز
                string[] dateParts = dateStr.Split('/');
                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);

                // تقسیم کردن رشته زمان به دو قسمت: ساعت و دقیقه
                string[] timeParts = timeStr.Split(':');
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                //تبدیل تاریخ شمسی به میلادی

                result = new DateTime(year, month, day, hour, minute, 0, new PersianCalendar());
            }
            else
            {
                // تقسیم کردن رشته تاریخ به سه قسمت: سال، ماه، روز
                string[] dateParts = dateStr.Split('/');
                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);
                result = new DateTime(year, month, day, new PersianCalendar());
            }


            return result;
        }
        catch
        {
            return DateTime.Now;
        }
    }


    // متد برای دریافت بازه تاریخی ماه شمسی بر اساس ورودی Enum
    public static ((DateTime startDate, string startDatePersian), (DateTime endDate, string endDatePersian)) GetPersianMonthDateRange(PersianMonth month)
    {
        PersianCalendar persianCalendar = new PersianCalendar();

        int year = persianCalendar.GetYear(DateTime.Now); // سال شمسی جاری
        int monthNumber = (int)month;

        // محاسبه تاریخ شروع ماه
        DateTime startDate = persianCalendar.ToDateTime(year, monthNumber, 1, 0, 0, 0, 0);
        // محاسبه تاریخ پایان ماه
        DateTime endDate = persianCalendar.ToDateTime(year, monthNumber,
            persianCalendar.GetDaysInMonth(year, monthNumber), 23, 59, 59, 999);

        // تبدیل تاریخ‌ها به فرمت شمسی
        string startDatePersian = $"{year}/{monthNumber:00}/01";
        string endDatePersian = $"{year}/{monthNumber:00}/{persianCalendar.GetDaysInMonth(year, monthNumber)}";

        return ((startDate, startDatePersian), (endDate, endDatePersian));
    }

    public static DateTimeOffset ToDatetimeOffsetFromUtc(this DateTime date) =>
        new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Utc));

    public static DateTime ToDateTime(this string date)
    {
        try
        {
            DateTime dateTime = DateTime.Parse(date);

            return dateTime;
        }
        catch (Exception)
        {
            return date.ToMilady();
        }
    }

    public static DateTime ConvertPersianDateTimeStringToGregorian(string persianDateTime)
    {
        if (persianDateTime.Length != 14)
            throw new ArgumentException("Invalid date format. Expected format: yyyyMMddHHmmss");

        int year = int.Parse(persianDateTime.Substring(0, 4));
        int month = int.Parse(persianDateTime.Substring(4, 2));
        int day = int.Parse(persianDateTime.Substring(6, 2));
        int hour = int.Parse(persianDateTime.Substring(8, 2));
        int minute = int.Parse(persianDateTime.Substring(10, 2));
        int second = int.Parse(persianDateTime.Substring(12, 2));

        var persianCalendar = new PersianCalendar();
        return persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);
    }

    private static DateTime MiladyDate(int year, int month, int day)
    {
        return new DateTime(year, month, day, new PersianCalendar());
    }

    public static string ToPersianTime(this TimeSpan ts)
    {
        return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0');
    }

    public static string ToPersianTime(this TimeSpan? ts)
    {
        return ts.HasValue ? ts.Value.ToPersianTime() : "";
    }

    public static string ToPersianDate(this DateTime dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        try
        {
            return string.Format("{0}/{1}/{2}", pc.GetYear(dateTime).ToString().PadLeft(4, '0'),
                pc.GetMonth(dateTime).ToString().PadLeft(2, '0'),
                pc.GetDayOfMonth(dateTime).ToString().PadLeft(2, '0'));
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Sample Format = ds dd ms Y
    /// </summary>
    /// <param name="dateTime">تاریخ میلادی</param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToPersianDate(this DateTime dateTime, string format)
    {
        try
        {
            PersianDateTime persianaDate = new PersianDateTime(dateTime);
            return persianaDate.ToString(format).Replace("سهشنبه", "سه شنبه");
        }
        catch
        {
            return "";
        }
    }

    public static string ToPersianDayName(this DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Saturday => "شنبه",
            DayOfWeek.Sunday => "یکشنبه",
            DayOfWeek.Monday => "دوشنبه",
            DayOfWeek.Tuesday => "سه‌شنبه",
            DayOfWeek.Wednesday => "چهارشنبه",
            DayOfWeek.Thursday => "پنج‌شنبه",
            DayOfWeek.Friday => "جمعه",
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }

    public static string ToPersianDate(this DateTimeOffset dateTimeOffset, string format)
    {
        //@DateTime.Now.ToPersianDate("ds dd ms Y")
        var pc = new PersianCalendar();
        try
        {
            var date = DateTime.Parse(dateTimeOffset.ToString(), CultureInfo.InvariantCulture);

            return date.ToPersianDate(format);
        }
        catch
        {
            return "";
        }
    }

    private static string GetDayOfWeekString(int day)
    {
        string[] days = new string[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
        if (day <= days.Length)
        {
            return days[day];
        }

        return "";
    }

    private static string GetMonthString(int month)
    {
        string[] months = new string[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        if (month <= months.Length)
        {
            return months[month - 1];
        }

        return "";
    }

    public static string ToPersianDateTime(this DateTime dateTime)
    {
        try
        {
            return string.Format("{0}:{1} {2}", dateTime.Hour.ToString().PadLeft(2, '0'),
                dateTime.Minute.ToString().PadLeft(2, '0'), dateTime.ToPersianDate());
        }
        catch
        {
            return "";
        }
    }

    public static string ToPersianDate(this DateTime? dateTime)
    {
        if (dateTime != null)
            return dateTime.Value.ToPersianDate();

        return string.Empty;
    }

    public static string ToPersianDateTime(this DateTime? dateTime)
    {
        if (dateTime != null)
            return dateTime.Value.ToPersianDateTime();

        return string.Empty;
    }

    public static DateTime? ToGregorianDateTime(this string persianDate)
    {
        if (string.IsNullOrEmpty(persianDate))
            return null;
        try
        {
            var pc = new PersianCalendar();

            var arrPersianDateTime = persianDate.Split(' ');
            var arrPersianDate = arrPersianDateTime[0].Split('/');
            var arrPersianTime = new string[] { "0", "0", "0" };

            if (arrPersianDateTime.Length == 2)
            {
                arrPersianTime = arrPersianDateTime[1].Split(':');
            }

            var year = int.Parse(arrPersianDate[0]);
            var month = short.Parse(arrPersianDate[1]);
            var day = short.Parse(arrPersianDate[2]);

            var hour = short.Parse(arrPersianTime[0]);
            var minute = short.Parse(arrPersianTime[1]);
            var second = arrPersianTime.Length == 3 ? short.Parse(arrPersianTime[2]) : 0;

            return pc.ToDateTime(year, month, day, hour, minute, second, 0);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    ///  مدت زمان انتشار
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string TimeAgo(this DateTime dateTime)
    {
        string result = string.Empty;
        if (dateTime == DateTime.MinValue)
        {
            return "";
        }

        var timeSpan = DateTime.Now.Subtract(dateTime);

        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            result = $"{timeSpan.Seconds} ثانیه پیش";
        }
        else if (timeSpan <= TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1
                ? $" {timeSpan.Minutes} دقیقه پیش"
                : "دقایقی پیش";
        }
        else if (timeSpan <= TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1
                ? $" {timeSpan.Hours} ساعت پیش"
                : "ساعتی پیش  ";
        }
        else if (timeSpan <= TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1
                ? $" {timeSpan.Days} روز پیش"
                : "دیروز";
        }
        else if (timeSpan <= TimeSpan.FromDays(365))
        {
            result = timeSpan.Days > 30
                ? $" {timeSpan.Days / 30} ماه پیش"
                : "چند ماه پیش";
        }
        else
        {
            result = timeSpan.Days > 365
                ? $" {timeSpan.Days / 365} سال پیش"
                : "چند سال پیش";
        }

        return result;
    }

    public static long DateTimeMilliseconds()
    {
        var dateTime = DateTime.Now;
        PersianCalendar persianCalendar = new PersianCalendar();
        DateTime persianDate = persianCalendar.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
            dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        DateTime gregorianDate = persianCalendar.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
            dateTime.Minute, dateTime.Second, dateTime.Millisecond, PersianCalendar.PersianEra);
        long milliseconds = (long)(gregorianDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
        return milliseconds;
    }

    public static long ToDateTimeMilliseconds(this DateTime dateTime)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        DateTime persianDate = persianCalendar.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
            dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        DateTime gregorianDate = persianCalendar.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
            dateTime.Minute, dateTime.Second, dateTime.Millisecond, PersianCalendar.PersianEra);
        long milliseconds = (long)(gregorianDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
        return milliseconds;
    }

    public static DateTime MillisecondToDateTime(long millisecond)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1);
        DateTime gregorianDate = unixEpoch.AddMilliseconds(millisecond);

        PersianCalendar persianCalendar = new PersianCalendar();
        int persianYear = persianCalendar.GetYear(gregorianDate);
        int persianMonth = persianCalendar.GetMonth(gregorianDate);
        int persianDay = persianCalendar.GetDayOfMonth(gregorianDate);
        int hour = persianCalendar.GetHour(gregorianDate);
        int minute = persianCalendar.GetMinute(gregorianDate);
        int second = persianCalendar.GetSecond(gregorianDate);
        double milliseconds = persianCalendar.GetMilliseconds(gregorianDate);
        return persianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, Convert.ToInt32(milliseconds));
    }

    public static (DateTime StartDate, DateTime EndDate) GetMiladiRangeForPersianMonth(string persianDateStr)
    {
        // فرض: ورودی به فرمت "1404/01/01"
        var persianCalendar = new PersianCalendar();

        // پارس کردن ورودی
        var parts = persianDateStr.Split('/');
        if (parts.Length != 3)
            throw new ArgumentException("فرمت ورودی باید yyyy/MM/dd باشد.");

        int year = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);

        // محاسبه اولین روز ماه
        var startDate = persianCalendar.ToDateTime(year, month, 1, 0, 0, 0, 0);

        // محاسبه آخرین روز ماه
        int daysInMonth = persianCalendar.GetDaysInMonth(year, month);
        var endDate = persianCalendar.ToDateTime(year, month, daysInMonth, 0, 0, 0, 0);

        return (startDate, endDate);
    }

    public static string DifferenceTwoDateTime(DateTime startDate, DateTime endDate)
    {
        TimeSpan duration = TimeSpan.FromTicks(endDate.Ticks - startDate.Ticks);
        string result = $" {duration.Hours} ساعت و {duration.Minutes} دقیقه  ";
        return result.Replace("-", "");
    }

    public static (int year, int month, int day) GetPersianObject(this string persianDate)
    {
        if (string.IsNullOrWhiteSpace(persianDate))
            throw new ArgumentException("Persian date cannot be null or empty.");

        string[] dateParts = persianDate.Split('/');

        if (dateParts.Length != 3)
            throw new ArgumentException("Invalid Persian date format. Use yyyy/MM/dd.");

        if (!int.TryParse(dateParts[0], out int year) ||
            !int.TryParse(dateParts[1], out int month) ||
            !int.TryParse(dateParts[2], out int day))
            throw new ArgumentException("Persian date must contain only numeric values.");

        PersianCalendar persianCalendar = new PersianCalendar();

        // Validate month range (1 to 12)
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");

        // Validate day range based on the Persian calendar
        int daysInMonth = persianCalendar.GetDaysInMonth(year, month);
        if (day < 1 || day > daysInMonth)
            throw new ArgumentOutOfRangeException(nameof(day), $"Day must be between 1 and {daysInMonth} for the given month.");

        // Convert to DateTime to ensure it's a valid Persian date
        DateTime dateTime = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

        return (persianCalendar.GetYear(dateTime), persianCalendar.GetMonth(dateTime), persianCalendar.GetDayOfMonth(dateTime));
    }

    public static bool IsPersianDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
            return false;

        string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd" };
        if (!DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return false;

        // اگر سال بین 1300 تا 1500 باشه، احتمال زیاد شمسیه
        return dt.Year >= 1300 && dt.Year <= 1500;
    }

    public static string ConvertToPersianIfGregorian(this string date)
    {
        if (string.IsNullOrWhiteSpace(date))
            return date;

        string[] formats = { "yyyy-MM-dd", "yyyy/MM/dd" };
        if (!DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return date;

        // اگر میلادی باشه، تبدیل کن
        if (dt.Year > 1500)
        {
            var pc = new PersianCalendar();
            int y = pc.GetYear(dt);
            int m = pc.GetMonth(dt);
            int d = pc.GetDayOfMonth(dt);

            // تشخیص جداکننده از ورودی
            var separator = date.Contains("/") ? "/" : "-";

            return $"{y:0000}{separator}{m:00}{separator}{d:00}";
        }

        // اگر خودش شمسی بود یا میلادی قدیمی، همونو برگردون
        return date;
    }
}