using System.Globalization;

namespace ShareMicroservice.Query.Extentions;

public static class DateTimeExtension
{
    public static string ToPersianDate(this DateTime dateTime)
    {
        var persianCalendar = new PersianCalendar();

        return $"{persianCalendar.GetYear(dateTime):0000}/" +
               $"{persianCalendar.GetMonth(dateTime):00}/" +
               $"{persianCalendar.GetDayOfMonth(dateTime):00}";
    }
    public static string ToPersianDateTime(this DateTime dateTime)
    {
        var persianCalendar = new PersianCalendar();

        return $"{persianCalendar.GetYear(dateTime):0000}/" +
               $"{persianCalendar.GetMonth(dateTime):00}/" +
               $"{persianCalendar.GetDayOfMonth(dateTime):00} " +
               $"{dateTime:HH:mm}";
    }

    public static DateTime ToMilady(this string persianDate)
    {
        if (string.IsNullOrWhiteSpace(persianDate))
            throw new ArgumentException("تاریخ نمی‌تواند خالی باشد.", nameof(persianDate));

        var parts = persianDate.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var dateParts = parts[0].Split('/');

        if (dateParts.Length != 3)
            throw new FormatException("فرمت تاریخ باید yyyy/MM/dd باشد.");

        if (!int.TryParse(dateParts[0], out var year) ||
            !int.TryParse(dateParts[1], out var month) ||
            !int.TryParse(dateParts[2], out var day))
        {
            throw new FormatException("تاریخ شمسی نامعتبر است.");
        }

        var hour = 0;
        var minute = 0;
        var second = 0;

        if (parts.Length > 1)
        {
            var timeParts = parts[1].Split(':');

            if (timeParts.Length < 2 || timeParts.Length > 3)
                throw new FormatException("فرمت ساعت باید HH:mm یا HH:mm:ss باشد.");

            if (!int.TryParse(timeParts[0], out hour) ||
                !int.TryParse(timeParts[1], out minute) ||
                (timeParts.Length == 3 && !int.TryParse(timeParts[2], out second)))
            {
                throw new FormatException("ساعت نامعتبر است.");
            }
        }

        var calendar = new PersianCalendar();

        return calendar.ToDateTime(
            year,
            month,
            day,
            hour,
            minute,
            second,
            0);
    }
}