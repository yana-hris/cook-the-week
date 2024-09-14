namespace CookTheWeek.Web.Infrastructure.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class SanitizingModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(string) || context.Metadata.ModelType.IsEnum)
            {
                return new SanitizingModelBinder();
            }

            return null; // Fallback to default binders for other types
        }
    }
}
