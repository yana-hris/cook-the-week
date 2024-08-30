namespace CookTheWeek.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2024;
        public const string PrivacyPolicyLastUpdateDateString = "28/07/2024";
        public const string TermsOfUseLastUpdateDateString = "27/08/2024";
        public const string CookiePolicyLastUpdateDateString = "27/08/2024";

        public const string CookieConsentName = ".AspNet.Consent";

        public const int DefaultPage = 1;
        public const int DefaultRecipesPerPage = 8;
        public const int DefaultIngredientsPerPage = 20;

        public static string[] FixedFooterActions =
            [
                "Index",
                "Error",
                "None",
                "Details",
                "Add",
                "Edit",
                "Login",
                "Registert",
                "Profile",
                "ChangePassword",
                "SetPassword",
                "AccessDeniedPathInfo",
            ];


        public const int RecipeCardTitleMaxLength = 50;
        public const int RecipeCardDescriptionMaxLength = 113;

        public static readonly decimal[] IngredientQtyFractionsArray = InitilizeFractionsArray();

        public static readonly Dictionary<string, decimal> QtyFractionOptions;       

        public const string HtmlEntityFractionSlash = "<span>&frasl;</span>";

        // Ingredients by categories FOR Recipe Details View
        public static int[] DiaryMeatSeafoodIngredientCategories = [1, 2, 11];
        public static int[] ProduceIngredientCategories = [8, 9];
        public static int[] LegumesIngredientCategories = [3];
        public static int[] PastaGrainsBakeryIngredientCategories = [4,7];
        public static int[] OilsHerbsSpicesSweetenersIngredientCategories = [5, 6, 10];
        public static int[] NutsSeedsAndOthersIngredientCategories = [12,13];

        public const string MealDateFormat = "dd-MM-yyyy";
        public const string DefaultMealPlanName = "[Your Meal Plan Name]";

        public const string StatusCode400BadRequestErrorMessage = "Bad Request! Try again submitting different data";
        public const string StatusCode500InternalServerErrorMessage = "An unexpected internal error occurred. Please try again later or contact administrator";

        public const int MealPlanTrimmedNameLnegth = 30;

        // Ingredients by categories FOR Shopping List View
        public static string[] ProductListCategoryNames = new string[]
        {
            "Fruits & Veggies",
            "Beans, Lentils and Legumes",
            "Nuts, Seeds and Others",
            "Bread & Bakery",
            "Meat & Seafood",
            "Diary, Cheese & Eggs",
            "Pasta & Rice",
            "Fats, Oils, Sauces and Broths",
            "Herbs, Spices and Sweeteners",
        };

        public static int[][] ProductListCategoryIds = new int[][]
        {
             new int[] { 8, 9 },
             new int[] { 3 },
             new int[] { 12, 13 },
             new int[] { 4 },
             new int[] { 2, 11 },
             new int[] { 1 },
             new int[] { 7 },
             new int[] { 10 },
             new int[] { 5, 6 },
        };

        public const string AppUserId = "e8ec0c24-2dd1-4a7a-aefc-b54bc9a8e403";
        public const string AppUserUsername = "appUser";
        public const string AppUserEmail = "appUser@yahoo.com";
        public const string AppUserPassword = "123456";

        public const string AdminUserId = "72ed6dd1-7c97-4af7-ab79-fc72e4a53b16";
        public const string AdminUserUsername = "adminUser";
        public const string AdminUserEmail = "admin@gmail.com";
        public const string AdminUserPassword = "admin1";

        public const string DeletedUserId = "DDBAEAB3-10D6-4993-BE38-59CD03967107";
        public const string DeletedUserUsername = "deletedUser";
        public const string DeletedUserEmail = "deletedUser@gmail.com";
        public const string DeletedUserPassword = "deletedUser1";

        public const string AdminAreaName = "Admin";
        public const string AdminRoleName = "Administrator";

        public const string UsersCacheKey = "UserCache";
        public const int UsersCacheDurationMinutes = 5;
        public const string RecipesCacheKey = "RecipesCache";

        public const string OnlineUsersCookieName = "IsOnline";
        public const int LastActivityBeforeOfflineMinutes = 10;

        // RecipeIngredient Measures 
        public static string[] DecimalMeasures = { "ml", "l", "g", "kg" };
        public static string[] FractionalMeasures = { "clove", "pc", "tsp", "tbsp", "cup", "bunch", "pkg", "slice", "pinch"};

        static GeneralApplicationConstants()
        {
            QtyFractionOptions = new Dictionary<string, decimal>
            {
                { "1/8", 1m / 8 },
                { "1/4", 1m / 4 },
                { "1/3", 1m / 3 },
                { "1/2", 1m / 2 },
                { "2/3", 2m / 3 },
                { "3/4", 3m / 4 }
            };
        }

        private static decimal[] InitilizeFractionsArray()
        {
            return new decimal[] { 1m / 8, 1m / 4, 1m / 3, 1m / 2, 2m / 3, 3m / 4 };
        }


    }
}
