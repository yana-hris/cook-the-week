namespace CookTheWeek.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TrimToChar(this string input, int count)
        {
            input = input.Trim();

            if(count < input.Length && input.Length > 3)
            {
                input = input.Substring(0, count-2) + "..";
            }

            return input;
        }
    }
}
