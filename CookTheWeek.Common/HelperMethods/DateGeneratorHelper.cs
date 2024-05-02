namespace CookTheWeek.Common.HelperMethods
{
    using static Common.GeneralApplicationConstants;
    public class DateGenerator
    {
        /// <summary>
        /// Accepts a datetime and returns an array of the 7 subsequent days
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public static string[] GenerateNext7Days(DateTime? startDate)
        {
            string[] next7Days = new string[7];

            if (startDate.HasValue && startDate.Value != DateTime.MinValue)
            {
                next7Days[0] = startDate.Value.ToString(MealDateFormat);

                for (int i = 1; i < 7; i++)
                {
                    DateTime nextDay = startDate.Value.AddDays(i);
                    string dayAsString = nextDay.ToString(MealDateFormat);
                    next7Days[i] = dayAsString;
                }
            }

            return next7Days;
        }

        /// <summary>
        /// Method overloading for generating the next 7 days array without input parameters
        /// </summary>
        /// <returns></returns>
        public static string[] GenerateNext7Days()
        {
            return GenerateNext7Days(DateTime.Today); 
        }
    }
}
