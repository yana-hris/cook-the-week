namespace CookTheWeek.Common.HelperMethods
{
    
    public static class CookingTimeHelper
    {
        public static string FormatCookingTime(TimeSpan cookingTime)
        {
            string space = (cookingTime.Hours > 0 && cookingTime.Minutes > 0) ? " " : "";
            string hours = cookingTime.Hours > 0 ? cookingTime.Hours.ToString() + "h" : "";
            string minutes = (cookingTime.Minutes > 0 || cookingTime.Hours == 0) ? cookingTime.Minutes.ToString() + " min" : "";
            string result = hours + space + minutes;

            return result;
        }
    }
}
