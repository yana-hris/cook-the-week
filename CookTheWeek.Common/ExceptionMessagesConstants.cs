namespace CookTheWeek.Common
{
    public static class ExceptionMessagesConstants
    {
        public static class DataRetrievalExceptionMessages
        {
            public const string RecipeDataRetrievalExceptionMessage = "An error occurred while retrieving recipe data.";

            public const string IngredientDataRetrievalExceptionMessage = "An error occurred while retrieving ingredient data.";

            public const string MealplanDataRetrievalExceptionMessage = "An error occurred while retrieving mealplan data.";

        }

        public static class  UnauthorizedExceptionMessages 
        {
            public const string UnauthorizedAccessExceptionMessage = "You have no rights to access this information.";
        }

        public static class  RecordNotFoundExceptionMessages
        {
            //RECIPE:
            // For single Recipe
            public const string RecipeNotFoundExceptionMessage = "Recipe not found.";

            // For Recipe collections
            public const string NoRecipesFoundExceptionMessage = "No recipe records found in the database";
        }
    }
}
