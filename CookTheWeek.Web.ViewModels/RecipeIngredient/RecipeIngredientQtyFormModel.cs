namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    using static Common.GeneralApplicationConstants;
    public class RecipeIngredientQtyFormModel
    {
        [Display(Name = "Qty Whole")]
        public int? QtyWhole { get; set; }

        [Display(Name = "Qty Fraction")]
        public string? QtyFraction { get; set; }

        [Display(Name = "Qty Decimal")]
        public decimal? QtyDecimal { get; set; }

        public decimal GetDecimalQtyValue()
        {
            decimal decimalQty = 0m;

            if (QtyDecimal.HasValue)
            {
                decimalQty = QtyDecimal.Value;
            }
            else if (QtyWhole.HasValue && !string.IsNullOrEmpty(QtyFraction))
            {
                // Calculate decimal value from whole number and fraction
                var matchedFraction = FractionOptions.FirstOrDefault(kv => kv.Key == QtyFraction);
                if (matchedFraction.Key != null)
                {
                    decimal fractionValue = matchedFraction.Value;
                    decimalQty = QtyWhole.Value + fractionValue;
                }
                else
                {
                    throw new InvalidOperationException("Invalid fraction value.");
                }
            }
            else if (QtyWhole.HasValue)
            {
                // If only whole number is provided
                decimalQty = QtyWhole.Value;
            }

            return decimalQty;
        }

        public static RecipeIngredientQtyFormModel ConvertFromDecimalQty(decimal decimalQty)
        {
            RecipeIngredientQtyFormModel model = new RecipeIngredientQtyFormModel();

            // Check if the decimal quantity is an integer
            if (decimalQty % 1 == 0)
            {
                model.QtyWhole = (int)decimalQty;
                model.QtyFraction = null;
                model.QtyDecimal = null;
                return model;
            }
           
            // If not, Separate the decimal into whole number and fractional parts
            model.QtyWhole = (int)decimalQty;
            decimal fractionalPart = decimalQty - model.QtyWhole.Value;

            // Find the closest fraction from the predefined options
            decimal minDifference = decimal.MaxValue;
            string closestFraction = null;

            foreach (var fractionOption in FractionOptions)
            {
                decimal difference = Math.Abs(fractionOption.Value - fractionalPart);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestFraction = fractionOption.Key;
                }
            }

            // Set the closest fraction and return the model
            model.QtyFraction = closestFraction;
            model.QtyDecimal = null;

            return model;
        }
    }
}
