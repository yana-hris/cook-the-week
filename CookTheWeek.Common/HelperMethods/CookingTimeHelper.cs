namespace CookTheWeek.Common.HelperMethods
{
    
    public static class CookingTimeHelper
    {
        public static string FormatCookingTime(int totalTimeInMinutes)
        {
            // Calculate hours and minutes from total minutes
            int hours = totalTimeInMinutes / 60;
            int minutes = totalTimeInMinutes % 60;

            // Format the string based on the presence of hours and minutes
            string space = (hours > 0 && minutes > 0) ? " " : "";
            string hoursString = hours > 0 ? $"{hours}h" : "";
            string minutesString = (minutes > 0 || hours == 0) ? $"{minutes} min" : "";

            return hoursString + space + minutesString;
        }
    }
}
