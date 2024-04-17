namespace CookTheWeek.Common.Helpers
{
    public class DateGenerator
    {
        public static string[] GenerateNext7Days()
        {
            string[] next7Days = new string[7];

            DateTime currentDate = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                DateTime nextDay = currentDate.AddDays(i);
                string dayAsString = nextDay.ToString("dd-MM-yyyy");
                next7Days[i] = dayAsString;
            }

            return next7Days;
        }
    }
}
