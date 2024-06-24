namespace CookTheWeek.Web.ViewModels.CustomValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ValidateRangeBasedOnCollectionSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string collectionPropertyName;
        private readonly string errorMessage;

        public ValidateRangeBasedOnCollectionSizeAttribute(string collectionName, string errorMessage)
        {
            this.collectionPropertyName = collectionName;
            this.errorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            // Get the collection property
            var property = validationContext.ObjectType.GetProperty(collectionPropertyName);

            if (property == null)
            {
                return new ValidationResult($"Unknown property: {collectionPropertyName}");
            }

            // Get the collection value
            var collection = property.GetValue(validationContext.ObjectInstance, null) as ICollection<object>;

            if (collection == null)
            {
                return new ValidationResult($"Property {collectionPropertyName} is not a collection");
            }

            int collectionCount = collection.Count;

            // Validate the value against the collection count
            if (value is int intValue && (intValue < 1 || intValue > collectionCount))
            {
                return new ValidationResult(string.Format(errorMessage, collectionCount));
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, 
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "validaterangebasedoncollectionsize"
            };

            rule.ValidationParameters["collectionPropertyName"] = collectionPropertyName;

            yield return rule;
        }
    }
}
