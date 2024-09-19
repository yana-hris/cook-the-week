namespace CookTheWeek.Web.Infrastructure.ModelBinders
{
    using System;
    using System.Threading.Tasks;
    using CookTheWeek.Web.ViewModels.Recipe;
    using Ganss.Xss;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Common.EntityValidationConstants.ErrorMessages;

    public class SanitizingModelBinder : IModelBinder
    {
        private readonly HtmlSanitizer sanitizer;

        private readonly HashSet<string> excludedProperties = new HashSet<string>
        {
            nameof(RecipeAddFormModel.ImageUrl),
            nameof(RecipeEditFormModel.ImageUrl),// Add other property names as needed
        };

        public SanitizingModelBinder()
        {
            sanitizer = new HtmlSanitizer();
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Ensure the model binding context is not null
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Get the value being bound
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            Console.WriteLine(valueProviderResult);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return; // No value to bind
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            // Handle both string and string? (nullable string) types
            if (bindingContext.ModelType == typeof(string) || Nullable.GetUnderlyingType(bindingContext.ModelType) == typeof(string))
            {
                string? value = valueProviderResult.FirstValue;

                // Check if the string is null or empty before sanitizing
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                // Skip sanitization if the property is in the excluded list
                var propertyName = bindingContext.ModelName;
                if (excludedProperties.Contains(propertyName))
                {
                    return;
                }

                // Sanitize non-empty strings
                string sanitizedValue = sanitizer.Sanitize(value);

                // Check if the sanitized result is empty and set a custom error message
                if (string.IsNullOrWhiteSpace(sanitizedValue))
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, InvalidInputErrorMessage);
                    return;
                }
                bindingContext.Result = ModelBindingResult.Success(sanitizedValue);
                return;
            }

            // Handle validation for select inputs (e.g., enums)
            if (bindingContext.ModelType.IsEnum)
            {
                var value = valueProviderResult.FirstValue;

                if (Enum.TryParse(bindingContext.ModelType, value, out var enumValue) && Enum.IsDefined(bindingContext.ModelType, enumValue))
                {
                    bindingContext.Result = ModelBindingResult.Success(enumValue);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid selection for {bindingContext.ModelName}");
                }
                return;
            }

            // If not a string or enum, delegate to the default model binder
            bindingContext.Result = ModelBindingResult.Failed();

        }
    }
}
