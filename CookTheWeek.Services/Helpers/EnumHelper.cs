namespace CookTheWeek.Services.Data.Helpers
{
    using System.ComponentModel;

    using CookTheWeek.Web.ViewModels;

    public static class EnumHelper
    {
        /// <summary>
        /// Converts an enum type to a collection of SelectViewModel instances.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>A collection of SelectViewModel.</returns>
        public static ICollection<SelectViewModel> GetEnumAsSelectViewModel<TEnum>() where TEnum : Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(enumValue => new SelectViewModel
                {
                    Id = Convert.ToInt32(enumValue),
                    Name = GetEnumDescription(enumValue)
                })
                .ToList();
        }

        /// <summary>
        /// Gets the description of an enum value, or its string representation if no description is available.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description or string representation of the enum value.</returns>
        public static string GetEnumDescription<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var fieldInfo = typeof(TEnum).GetField(enumValue.ToString());
            var descriptionAttribute = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute?.Description ?? enumValue.ToString();
        }
    }
}
