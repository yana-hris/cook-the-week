namespace CookTheWeek.Common
{
    public static  class ValidationErrorMessages
    {
        public static class RecipeIngredient
        {
            public const string NameErrorMessage = "Ingredient name required";

            public const string QtyErrorMessage = "Invalid quantity";

            public const string MeasureErrorMessage = "Unit/measure required";

        }

        public static class RecipeIngredientQty
        {
            public const string DefaultErrorMessage = "Invalid quantity";

            public const string MissingFormInputErrorMessage = "Required either decimal or fractional quantity";

            public const string MissingFractionalOrWholeInputMessage = "Required either a whole and/or fractional quantity";

            public const string InvalidDecimalRangeErrorMessage = "Invalid quantity range, must be [0.001-9999.99]";

            public const string InvalidFractionErrorMessage = "Required fraction selection";

            public const string InvalidWholeQtyErrorMessage = "Invalid quantity range, must be [1-9999]";
        }
    }
}
