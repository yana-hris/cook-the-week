namespace CookTheWeek.Web.Infrastructure.ModelBinders
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;

    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = 
                bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            try
            {
                var result = JsonConvert.DeserializeObject(value, bindingContext.ModelType);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (JsonException)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName, "Invalid JSON format.");
            }

            return Task.CompletedTask;
        }
    }
    }
}
