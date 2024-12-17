namespace CookTheWeek.Services.Data.Helpers
{
    using System;
    using System.Globalization;

    internal static class DateFormatter
    {
        internal static string FormatLocalizedDate(DateTime date, string userLocale)
        {
            try
            {
                var culture = new CultureInfo(userLocale);
                return date.ToString("f", culture); // Full date/time pattern
            }
            catch (CultureNotFoundException)
            {
                // Fallback to a default format if the culture is invalid
                return date.ToString("f", CultureInfo.InvariantCulture);
            }
        }
    }
}
