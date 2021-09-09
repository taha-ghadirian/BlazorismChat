using System.Globalization;

namespace BlazorismChat.ClientLibraries.Convertors;

public static class DateConvertor
{
    /// <summary>
    /// Convert date Time to shamsi
    /// </summary>
    /// <param name="time">DateTime to Convert</param>
    /// <returns>Shamsi DateTime</returns>
    public static DateTime ToShamsi(this DateTime time)
    {
        PersianCalendar calendar = new PersianCalendar();
        return new DateTime(
            calendar.GetYear(time),
            calendar.GetMonth(time),
            calendar.GetDayOfMonth(time),
            calendar.GetHour(time),
            calendar.GetMinute(time),
            calendar.GetSecond(time)
        );
    }

    /// <summary>
    /// Get Full Time Text
    /// </summary>
    /// <param name="time">DateTime to Convert</param>
    /// <returns>YY/MM/DD  HH:MM</returns>
    public static string ToFullText(this DateTime time)
        => $"{time.ToDateText()}  {time.ToHourText()}";

    /// <summary>
    /// Get Date Only Text
    /// </summary>
    /// <param name="time">DateTime to Convert</param>
    /// <returns>YY/MM/DD</returns>
    public static string ToDateText(this DateTime time)
        => $"{time.Year:0000}/{time.Month:00}/{time.Day:00}";

    /// <summary>
    /// Get Hour only text
    /// </summary>
    /// <param name="time">DateTime to Convert</param>
    /// <returns>HH:MM</returns>
    public static string ToHourText(this DateTime time)
        => $"{time.Hour:00}:{time.Minute:00}";
}
