namespace CookTheWeek.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2024;

        public const int DefaultPage = 1;
        public const int DefaultRecipesPerPage = 8;
        public const int DefaultIngredientsPerPage = 20;

        public const int RecipeCardTitleMaxLength = 50;
        public const int RecipeCardDescriptionMaxLength = 113;

        public static int[] DiaryMeatSeafoodIngredientCategories = [1, 2, 11];
        public static int[] ProduceIngredientCategories = [8, 9];
        public static int[] LegumesIngredientCategories = [3];
        public static int[] PastaGrainsBakeryIngredientCategories = [4,7];
        public static int[] OilsHerbsSpicesSweetenersIngredientCategories = [5, 6, 10];
        public static int[] NutsSeedsAndOthersIngredientCategories = [12,13];

        public const string MealDateFormat = "dd-MM-yyyy";
        public const string DefaultMealPlanName = "[Your Meal Plan Name]";

        public const int TrimmedMealPlanNameLnegth = 30;

        public const string AppUserId = "e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403";
        public const string AppUserUsername = "appUser";
        public const string AppUserEmail = "appUser@yahoo.com";
        public const string AppUserPassword = "123456";

        public const string AdminUserId = "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16";
        public const string AdminUserUsername = "adminUser";
        public const string AdminUserEmail = "admin@gmail.com";
        public const string AdminUserPassword = "admin1";

        public const string AdminAreaName = "Admin";
        public const string AdminRoleName = "Administrator";

        public const string UsersCacheKey = "UserCache";
        public const int UsersCacheDurationMinutes = 5;
        public const string RecipesCacheKey = "RecipesCache";

        public const string OnlineUsersCookieName = "IsOnline";
        public const int LastActivityBeforeOfflineMinutes = 10;
    }
}
