namespace CookTheWeek.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;

    public class ValidateQtyAttribute : ValidationAttribute
    {
        private readonly Dictionary<string, decimal> fractionOptions;
        public ValidateQtyAttribute() 
        {
            const string defaultErrorMessage = "Error with ingredient Qty";
            ErrorMessage ??= defaultErrorMessage;
            fractionOptions = FractionOptions;
        }
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            var parentModel = (RecipeIngredientFormViewModel)validationContext.ObjectInstance;
            var model = (RecipeIngredientQtyFormModel)parentModel.Qty;


            if ((model.QtyDecimal.HasValue && (model.QtyWhole.HasValue || !string.IsNullOrEmpty(model.QtyFraction))) ||
                (!model.QtyDecimal.HasValue && (!model.QtyWhole.HasValue && string.IsNullOrEmpty(model.QtyFraction)))) 
            {
                return new ValidationResult("Please enter either a decimal quantity or a combination of a whole number and/or a fraction.");
            }

            if (model.QtyDecimal.HasValue && (model.QtyDecimal.Value < 0.001m ||
                model.QtyDecimal.Value > 9999.99m))
            {
                return new ValidationResult("Decimal quantity must be between 0.001 and 9999.99.");
            }

            if (!string.IsNullOrEmpty(model.QtyFraction) && !fractionOptions.ContainsKey(model.QtyFraction))
            {
                return new ValidationResult("Please enter a valid fraction.");
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
