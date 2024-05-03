namespace CookTheWeek.Common.HelperMethods
{
    
    public static class IngredientHelper
    {
        public static string FormatIngredientQty(decimal qty)
        {
            if (qty == 0.0m || qty < 0.0m)
            {
                return "0";
            }
            
            if(qty % 1 == 0)
            {
                return ((int)qty).ToString();
            }
            else
            {
                return FractionGenerator(qty);
            }
            
        }

        private static string FractionGenerator(decimal fraction)
        {
            // Initialize variables for integer part, numerator, and denominator
            int integerPart = (int)Math.Floor(fraction);
            int numerator = 0;
            int denominator = 0;

            // Calculate the fractional part
            decimal fractionalPart = fraction - integerPart;

            // If the fractional part is greater than zero, convert it to numerator and denominator
            if (fractionalPart > 0)
            {
                int gcd = GCD((int)(fractionalPart * 10000), 10000);
                numerator = (int)(fractionalPart * 10000) / gcd;
                denominator = 10000 / gcd;
            }

            // If there's no fractional part, return the integer part
            if (numerator == 0)
            {
                return integerPart.ToString();
            }
            // If there's an integer part and a fractional part, return the combined string
            else
            {
                // If there's also an integer part, combine it with the fraction
                if (integerPart > 0)
                {
                    return $"{integerPart} {numerator}<span>&frasl;</span>{denominator}";
                }
                else
                {
                    return $"{numerator}<span>&frasl;</span>{denominator}";
                }
            }
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
