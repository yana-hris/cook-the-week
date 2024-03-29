namespace CookTheWeek.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2024;
        public const int DefaultPage = 1;
        public const int DefaultRecipesPerPage = 6;
        public const int DefaultIngredientsPerPage = 25;

        public static int[] MainIngredientsCategories = [1, 2, 4, 7, 11];
        public static int[] SecondaryIngredientsCategories = [3, 8, 9, 12, 13];
        public static int[] AdditionalIngredientsCategories = [5, 6, 10];

        public const string AdminAreaName = "Admin";
        public const string AdminRoleName = "Administrator";
        public const string DevelopmentAdminUserName = "Admin";
    }
}
