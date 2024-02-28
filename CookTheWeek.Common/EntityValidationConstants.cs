namespace CookTheWeek.Common
{
    public static class EntityValidationConstants
    {
        public static class Recipe
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 50;

            public const int DescriptionMaxLength = 1000;

            public const int ImageUlrMaxLength = 2048;
        }

        public static class Ingredient
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class Measure
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class Specification
        {
            public const int DescriptionMinLength = 2;
            public const int DescriptionMaxLength = 50;
        }

        public static class RecipeCategory
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class IngredientCategory
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }
    }
}
