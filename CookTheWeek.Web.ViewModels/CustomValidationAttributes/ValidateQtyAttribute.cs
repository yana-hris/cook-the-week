﻿namespace CookTheWeek.Web.ViewModels.CustomValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.RecipeIngredientQtyValidation;

    public class ValidateQtyAttribute : ValidationAttribute
    {
        private readonly Dictionary<string, decimal> fractionOptions;
        public ValidateQtyAttribute() 
        {
            const string defaultErrorMessage = MissingFormInputErrorMessage;
            ErrorMessage ??= defaultErrorMessage;
            fractionOptions = QtyFractionOptions;
        }
        
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            var parentModel = (RecipeIngredientFormModel)validationContext.ObjectInstance;

            if (parentModel != null)
            {
                var model = (RecipeIngredientQtyFormModel)parentModel.Qty;

                if (model != null)
                {
                    if (model.QtyDecimal.HasValue)
                    {
                        if (model.QtyWhole.HasValue || !string.IsNullOrEmpty(model.QtyFraction))
                        {
                            return new ValidationResult(MissingFormInputErrorMessage);
                        }

                        if (model.QtyDecimal.Value < 0.001m || model.QtyDecimal.Value > 9999.99m)
                        {
                            return new ValidationResult(InvalidDecimalRangeErrorMessage);
                        }
                    }
                    else
                    {
                        if (!model.QtyWhole.HasValue && string.IsNullOrEmpty(model.QtyFraction))
                        {
                            return new ValidationResult(MissingFormInputErrorMessage);
                        }

                        if (model.QtyWhole.HasValue && (model.QtyWhole.Value < 1 || model.QtyWhole.Value > 9999))
                        {
                            return new ValidationResult(InvalidWholeQtyErrorMessage);
                        }

                        if (!string.IsNullOrEmpty(model.QtyFraction) && !fractionOptions.ContainsKey(model.QtyFraction))
                        {
                            return new ValidationResult(InvalidFractionErrorMessage);
                        }
                    }
                }
                else
                {
                    return new ValidationResult(MissingFormInputErrorMessage);
                }


                return ValidationResult.Success;
            }

            return new ValidationResult(MissingFormInputErrorMessage);

        }
    }
}
