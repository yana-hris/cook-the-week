namespace CookTheWeek.Common.HelperMethods
{
    
    public static class CookingTimeHelper
    {
        public static string FormatCookingTime(TimeSpan cookingTime)
        {
            string hours = cookingTime.Hours > 0 ? cookingTime.Hours.ToString() + "h " : "";
            string minutes = (cookingTime.Minutes > 0 || cookingTime.Hours == 0) ? cookingTime.Minutes.ToString() + "min" : "";
            string space = (cookingTime.Hours > 0 && cookingTime.Minutes > 0) ? " " : "";
            string result = (cookingTime.Hours == 0 && cookingTime.Minutes == 0) ? "0min" : hours + space + minutes;

            return result;
        }
    }
}
