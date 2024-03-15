namespace CookTheWeek.Common
{
    public static class EntityValidationConstants
    {
        public static class Recipe
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            public const int DescriptionMaxLength = 1000;

            public const int ImageUlrMinLength = 15;
            public const int ImageUlrMaxLength = 2048;

            public const int InstructionsMinLength = 10;
            public const int InstructionsMaxLength = 2000;

            public const int ServingsMinValue = 1;
            public const int ServingsMaxValue = 12;

            public const int CookingTimeMinValue = 10;
            public const int CookingTimeMaxValue = 4320;
            
            
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
