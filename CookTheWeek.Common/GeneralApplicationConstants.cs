namespace CookTheWeek.Common
{
    public static class GeneralApplicationConstants
    {
        public const int ReleaseYear = 2024;
        public const string PrivacyPolicyLastUpdateDateString = "28/07/2024";
        public const string TermsOfUseLastUpdateDateString = "27/08/2024";
        public const string CookiePolicyLastUpdateDateString = "27/08/2024";

        public const string CookieConsentName = ".AspNet.Consent";
        public const string ReturnUrl = "ReturnUrl";
        public const string ContactFormModelWithErrors = "ContactFormModelWithErrors";
        public const string HasActiveMealPlanClaimName = "HasActiveMealPlan";

        public const int DefaultPage = 1;
        public const int DefaultRecipesPerPage = 8;
        public const int DefaultIngredientsPerPage = 20;
        public static readonly IReadOnlyList<int> PerPageOptions = new List<int> { 8, 12, 16, 20, 24 };

        public const double DefaultPdfPageWidth = 210.0d;
        public const double DefaultPdfPageHeight = 297.0d;

        public const int GuidLength = 36;
        public static readonly Guid DeletedRecipeId = new Guid("00000000-0000-0000-0000-000000000001");

        public const int RecipeCardTitleMaxLength = 50;
        public const int RecipeCardDescriptionMaxLength = 113;

        public static readonly decimal[] IngredientQtyFractionsArray = InitilizeFractionsArray();

        public static readonly Dictionary<string, decimal> QtyFractionOptions;       

        public const string HtmlEntityFractionSlash = "<span>&frasl;</span>";

        // Ingredients by categories FOR Recipe Details View
        // TODO: Delete when ready
        public static int[] DiaryMeatSeafoodIngredientCategories = [1, 2, 11];
        public static int[] ProduceIngredientCategories = [8, 9];
        public static int[] LegumesIngredientCategories = [3];
        public static int[] PastaGrainsBakeryIngredientCategories = [4,7];
        public static int[] OilsHerbsSpicesSweetenersIngredientCategories = [5, 6, 10];
        public static int[] NutsSeedsAndOthersIngredientCategories = [12,13];

        public const string MealDateFormat = "dd-MM-yyyy";
        public const string MealPlanDateFormat = "ddd, dd-MM-yyyy";
        public const string DefaultMealPlanName = "[Your Meal Plan Name]";

        public const string UserNotFoundErrorMessage = "User not found.";
        public const string IncorrectPasswordErrorMessage = "Incorrect current password.";
        public const string StatusCode400BadRequestErrorMessage = "Bad Request! Try again submitting different data";
        public const string StatusCode500InternalServerErrorMessage = "An unexpected internal error occurred. Please try again later or contact administrator";

        public const int MealPlanTrimmedNameLnegth = 30;

        // For meal and Recipe Details Views
        public static readonly Dictionary<string, int[]> RecipeAndMealDetailedProductListCategoryDictionary = 
            new Dictionary<string, int[]>
        {
            { "Meat, Diary Products & Seafood", new int[] { 1, 11, 2 } },
            { "Produce (Fruit & Vegetables)", new int[] { 9, 8 } },
            { "Pasta, Grains & Bakery", new int[] { 7, 4 } },
            { "Beans, Lentils and Legumes", new int[] { 3 } },
            { "Nuts, Seeds & Others", new int[] { 12, 13 } },
            { "Herbs, Spices, Oils & Sauces", new int[] { 10, 5, 6 } }
        };

        // For Shopping List View:
        public static readonly Dictionary<string, int[]> ShoppingListCategoryGroupDictionary = 
            new Dictionary<string, int[]>
        {
            { "Fruits & Veggies", new int[] { 8, 9 } },
            { "Beans, Lentils and Legumes", new int[] { 3 } },
            { "Nuts, Seeds and Others", new int[] { 12, 13 } },
            { "Bread & Bakery", new int[] { 4 } },
            { "Meat & Seafood", new int[] { 2, 11 } },
            { "Diary, Cheese & Eggs", new int[] { 1 } },
            { "Pasta & Rice", new int[] { 7 } },
            { "Fats, Oils, Sauces and Broths", new int[] { 10 } },
            { "Herbs, Spices and Sweeteners", new int[] { 5, 6 } }
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

        public const int TokenExpirationDefaultHoursTime = 24;

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

        public const string VeganTagDescription = "Recipes labeled as vegan contain no animal-derived products, including meat, dairy, eggs, or honey. They are crafted using only plant-based ingredients to suit ethical, dietary, or environmental preferences.";
        public const string VegetarianTagDescription = "These recipes exclude meat, poultry, and seafood but may include animal by-products like dairy, eggs, and honey. Perfect for those embracing a meat-free lifestyle.";
        public const string PescatarianTagDescription = "Recipes include seafood and fish but exclude other types of meat. These are great for individuals who prefer a diet with occasional fish-based protein.";
        public const string GlutenFreeTagDescription = "Certified gluten-free recipes exclude wheat, barley, rye, and related derivatives. They are safe for people with celiac disease, gluten intolerance, or those on a gluten-free diet.";
        public const string DiaryFreeTagDescription = "These recipes contain no milk, cream, cheese, or other dairy products. Ideal for those with lactose intolerance, dairy allergies, or following a dairy-free diet.";
        public const string NutFreeTagDescription = "Recipes marked as nut-free contain no tree nuts or peanuts. Perfect for individuals with nut allergies or those avoiding nuts for dietary reasons.";

        public const string ChristmasTagDescription = "Festive recipes inspired by the Christmas season. These include traditional and creative holiday treats to celebrate with family and friends.";
        public const string EasterTagDescription = "Recipes crafted with Easter traditions in mind, featuring light and seasonal dishes like eggs, spring vegetables, and festive baked goods.";

        public const string QuickRecipeDescription = "Recipes that can be prepared swiftly, perfect for busy schedules. Most take less than 30 minutes from start to finish.";
        public const string FifteenMinuteMealsDescription = "Even faster options that can be ready in just 15 minutes or less, ideal for quick lunches or dinners.";
        public const string SlowCookedDescription = "Dishes that require slow cooking for deep, rich flavors. Best suited for hearty meals or when you have time to let the food simmer.";
        public const string NoCookDescription = "Recipes that require no cooking, ideal for hot weather or quick preparation. They often include salads, sandwiches, and raw dishes.";

        public const string KidFriendlyDescription = "Recipes designed to appeal to children, featuring simple, flavorful, and approachable meals for younger palates.";
        public const string BudgetFriendlyDescription = "Affordable recipes that use cost-effective and accessible ingredients. Great for cooking delicious meals without breaking the bank.";

        public const string SpringRecipesDescription = "Seasonal recipes featuring fresh produce like asparagus, peas, and leafy greens. Perfect for celebrating the renewal of spring.";
        public const string SummerRecipesDescription = "Light and refreshing recipes with seasonal ingredients like tomatoes, cucumbers, and berries. Ideal for hot weather or outdoor meals.";
        public const string AutumnRecipesDescription = "Cozy dishes using seasonal produce like pumpkins, squash, apples, and root vegetables. Perfect for cooler weather and comforting meals.";
        public const string WinterRecipesDescription = "Hearty and warming recipes for the cold season, featuring ingredients like potatoes, citrus fruits, and spices for robust flavors.";

        public const string ClassicRecipesDescription = "Timeless recipes that are beloved staples across generations. These dishes often evoke a sense of nostalgia and tradition.";
        public const string HealthyRecipesDescription = "Recipes designed with balanced nutrition in mind, focusing on whole, nutrient-rich ingredients.";
        public const string LightRecipesDescription = "Lighter recipes with lower calories or smaller portions, perfect for those seeking a less heavy meal option.";
    }
}
