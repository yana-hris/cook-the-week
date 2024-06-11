namespace CookTheWeek.Common
{
    public static  class ValidationErrorMessages
    {
        public static class RecipeIngredient
        {
            public const string NameErrorMessage = "Please enter ingredient.";

            public const string QtyErrorMessage = "Invalid qty.";

            public const string MeasureErrorMessage = "Please choose a unit.";

        }

        public static class RecipeIngredientQty
        {
            public const string DefaultErrorMessage = "Invalid qty.";

            public const string MissingFormInputErrorMessage = "Please enter either a decimal or a fraction quantity.";

            public const string InvalidDecimalRangeErrorMessage = "Decimal quantity must be between [0.001-9999.99].";

            public const string InvalidFractionErrorMessage = "Please enter a valid fraction.";

            public const string InvalidWholeQtyErrorMessage = "Qty must be between in the range [1-9999].";
        }
    }
}
