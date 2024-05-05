namespace CookTheWeek.Common.HelperMethods
{
    using static Common.GeneralApplicationConstants;
    
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
        /// <summary>
        /// Retturns the nearest fraction representation of decimals between 0.001 and 0.999 by rounding to a set of fractions
        /// </summary>
        /// <param name="fraction"></param>
        /// <returns></returns>
        private static string FractionGenerator(decimal fraction)
        {
            int wholeNumber = (int)Math.Truncate(fraction);
            fraction -= wholeNumber;

            decimal[] possibleFractions = IngredientQtyFractionsArray;

            decimal minDifference = decimal.MaxValue;
            decimal closestFraction = 0;

            foreach (decimal frac in possibleFractions)
            {
                decimal difference = Math.Abs(frac - fraction);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestFraction = frac;
                }
            }

            // Convert the closest fraction to a string representation
            string[] fractionStrings = IngredientQtyFractionsArray.Select(frac => frac.ToString()).ToArray();
            string closestFractionString = fractionStrings[Array.IndexOf(possibleFractions, closestFraction)];

            if (wholeNumber == 0)
            {
                return closestFractionString.Replace("/", HtmlEntityFractionSlash);
            }
            else
            {
                return $"{wholeNumber} {closestFractionString.Replace("/", HtmlEntityFractionSlash)}";
            }
        }
    }
}
