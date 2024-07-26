namespace CookTheWeek.Common.Extensions
{
    public static class NumberExtensions
    {
        public static string ToStringWithSuffix(this int number)
        {
            if (number <= 0) return number.ToString();

            int lastTwoDigits = number % 100;
            int lastDigit = number % 10;

            string suffix;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            {
                suffix = "th";
            }
            else
            {
                switch (lastDigit)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }

            string result = $"{number}<sup>{suffix}</sup>";

            return result;
        }
    }
}
