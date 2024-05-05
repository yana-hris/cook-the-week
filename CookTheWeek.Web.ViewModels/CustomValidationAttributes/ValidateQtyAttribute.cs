namespace CookTheWeek.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;

    public class ValidateQtyAttribute : ValidationAttribute
    {
        public ValidateQtyAttribute() 
        {
            const string defaultErrorMessage = "Error with ingredient Qty";
            ErrorMessage ??= defaultErrorMessage;
        }
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            var model = (RecipeIngredientQtyFormModel)validationContext.ObjectInstance;

            if ((model.QtyDecimal.HasValue && (model.QtyWhole.HasValue || !string.IsNullOrEmpty(model.QtyFraction))) ||
                (!model.QtyDecimal.HasValue && (!model.QtyWhole.HasValue || string.IsNullOrEmpty(model.QtyFraction)))) 
            {
                return new ValidationResult("Please enter either a decimal quantity or a combination of a whole number and/or a fraction.");
            }

            if (model.QtyDecimal.HasValue && (model.QtyDecimal.Value < 0.001m ||
                model.QtyDecimal.Value > 9999.99m))
            {
                return new ValidationResult("Decimal quantity must be between 0.001 and 9999.99.");
            }

            if (!string.IsNullOrEmpty(model.QtyFraction))
            {
                var selectedFraction = FractionOptions
                    .FirstOrDefault(kv => kv.Key == model.QtyFraction);
                if (selectedFraction.Key == null)
                {
                    return new ValidationResult("Please enter a valid fraction.");
                }
            }

            if (model.QtyWhole.HasValue && (model.QtyWhole.Value < 1 ||
                model.QtyWhole.Value > 9999))
            {
                return new ValidationResult("Qty must be between 1 and 9999.");
            }

            return ValidationResult.Success;
        }
    }
}
