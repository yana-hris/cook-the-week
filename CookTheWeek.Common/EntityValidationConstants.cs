﻿namespace CookTheWeek.Common
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
            public const int CookingTimeMaxValue = 720;

            public static int[] ServingsOptions =
            [
                2,
                4,
                6,
                8,
                10,
                12
            ];
        }

       
        public static class Ingredient
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class RecipeIngredient
        {
            public const double QtyMinValue = 0.001;
            public const double QtyMaxValue = 10000.00;
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

        public static class ApplicationUser
        {
            public const int UsernameMinLength = 3;
            public const int UsernameMaxLength = 50;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;    
        }
    }
}
