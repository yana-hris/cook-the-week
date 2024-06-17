namespace CookTheWeek.Web.ViewModels.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;
    using static Common.ValidationErrorMessages.RecipeIngredientQty;

    public class ValidateQtyAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly Dictionary<string, decimal> fractionOptions;
        public ValidateQtyAttribute() 
        {
            const string defaultErrorMessage = DefaultErrorMessage;
            ErrorMessage ??= defaultErrorMessage;
            fractionOptions = QtyFractionOptions;
        }
        
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            var parentModel = (RecipeIngredientFormViewModel)validationContext.ObjectInstance;
            var model = (RecipeIngredientQtyFormModel)parentModel.Qty;


            if ((model.QtyDecimal.HasValue && (model.QtyWhole.HasValue || !string.IsNullOrEmpty(model.QtyFraction))) ||
                (!model.QtyDecimal.HasValue && (!model.QtyWhole.HasValue && string.IsNullOrEmpty(model.QtyFraction)))) 
            {
                return new ValidationResult(MissingFormInputErrorMessage);
            }

            if (model.QtyDecimal.HasValue && (model.QtyDecimal.Value < 0.001m ||
                model.QtyDecimal.Value > 9999.99m))
            {
                return new ValidationResult(InvalidDecimalRangeErrorMessage);
            }

            if (!string.IsNullOrEmpty(model.QtyFraction) && !fractionOptions.ContainsKey(model.QtyFraction))
            {
                return new ValidationResult(InvalidFractionErrorMessage);
            }

            if (model.QtyWhole.HasValue && (model.QtyWhole.Value < 1 ||
                model.QtyWhole.Value > 9999))
            {
                return new ValidationResult(InvalidWholeQtyErrorMessage);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "validateqty"
            };
            
            yield return rule;
        }

    }
}
