namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.RecipeIngredient;
    using static Common.GeneralApplicationConstants;
    public class RecipeIngredientQtyFormModel
    {
        [Display(Name = "Qty Whole")]
        [Range(QtyWholeMinValue, QtyWholeMaxValue)]
        public int? QtyWhole { get; set; }


        [Display(Name = "Qty Fraction")]
        public string? QtyFraction { get; set; }


        [Display(Name = "Qty Decimal")]
        [Range(QtyMinDecimalValue, QtyMaxDecimalValue)]
        public decimal? QtyDecimal { get; set; }

        public decimal GetDecimalQtyValue()
        {
            decimal decimalQty = 0m;

            if (QtyDecimal.HasValue)
            {
                decimalQty = QtyDecimal.Value;
            }
            else if (!string.IsNullOrEmpty(QtyFraction))
            {
                // Calculate decimal value from whole number and fraction
                var matchedFraction = QtyFractionOptions.FirstOrDefault(kv => kv.Key == QtyFraction);
                
                if (matchedFraction.Key != null)
                {
                    decimal fractionValue = matchedFraction.Value;
                    decimalQty = fractionValue;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                if (QtyWhole.HasValue)
                {
                    decimalQty += QtyWhole.Value;
                }
            }
            else if (QtyWhole.HasValue)
            {
                // If only whole number is provided
                decimalQty = QtyWhole.Value;
            }

            return decimalQty;
        }

        public static RecipeIngredientQtyFormModel ConvertFromDecimalQty(decimal decimalQty, string measure)
        {
            RecipeIngredientQtyFormModel model = new RecipeIngredientQtyFormModel();

            // If Measure is ml, l, g or kg and the number is not int => it is decimal
            bool isFractionalMeasure = FractionalMeasures.Contains(measure.ToLower());

            // If the quantity is fractional and measure cannot be fractional
            if (decimalQty % 1 != 0)
            {
                if (!isFractionalMeasure)
                {
                    model.QtyDecimal = decimalQty;
                    model.QtyWhole = null;
                    model.QtyFraction = null;
                    return model;
                }

                // Split the decimal quantity into whole and fractional parts
                if (decimalQty > 1)
                {
                    model.QtyWhole = (int)decimalQty;
                    decimal fractionalPart = decimalQty - model.QtyWhole.Value;
                    model.QtyFraction = GetClosestFraction(fractionalPart);
                }
                else
                {
                    model.QtyWhole = null;
                    model.QtyFraction = GetClosestFraction(decimalQty);
                }

                model.QtyDecimal = null;
                return model;
            }

            // If the quantity is whole
            if (!isFractionalMeasure)
            {
                model.QtyDecimal = decimalQty;
                model.QtyWhole = null;
                model.QtyFraction = null;
                return model;
            }

            model.QtyWhole = (int)decimalQty;
            model.QtyFraction = null;
            model.QtyDecimal = null;
            return model;
        }

        private static string GetClosestFraction(decimal fractionalPart)
        {
            decimal minDifference = decimal.MaxValue;
            string closestFraction = "";

            foreach (var fractionOption in QtyFractionOptions)
            {
                decimal difference = Math.Abs(fractionOption.Value - fractionalPart);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestFraction = fractionOption.Key;
                }
            }

            return closestFraction;
        }
    }
}
