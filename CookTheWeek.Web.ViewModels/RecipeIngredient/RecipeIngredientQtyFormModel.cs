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
            string[] decimalMeasures = {"ml", "l", "g", "kg" };

            if (decimalQty % 1 != 0)
            {
                if (decimalMeasures.Contains(measure.ToLower())) // Check if the measure cannot be fraction
                {
                    model.QtyDecimal = decimalQty * 1.0m;
                    model.QtyWhole = null;
                    model.QtyFraction = null;
                    return model;
                }
                else
                {
                    // If not, Separate the decimal into whole number and fractional parts
                    decimal fractionalPart = 0.0m;

                    if (decimalQty > 1.0m)
                    {
                        model.QtyDecimal = null;
                        model.QtyWhole = (int)decimalQty;
                        fractionalPart = decimalQty - model.QtyWhole.Value;
                    }
                    else
                    {
                        model.QtyDecimal = null;
                        model.QtyWhole = null;
                    }

                    // Find the closest fraction from the predefined options
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

                    // Set the closest fraction and return the model
                    model.QtyFraction = closestFraction;

                    return model;
                }
            }
            else // Check if the decimal quantity is an integer
            {
                if (decimalMeasures.Contains(measure.ToLower())) // Check if the measure cannot be fraction
                {
                    model.QtyDecimal = decimalQty * 1.0m;
                    model.QtyWhole = null;
                    model.QtyFraction = null;
                    return model;
                }
                else
                {
                    model.QtyWhole = (int)decimalQty;
                    model.QtyFraction = null;
                    model.QtyDecimal = null;
                    return model;
                }                
            }
        }
    }
}
