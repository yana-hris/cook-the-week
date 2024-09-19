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
            public const string RecipeAuthorizationExceptionMessage = "You must be the recipe owner to edit it.";
        }

        public static class  RecordNotFoundExceptionMessages
        {
            //RECIPE:
            // For single Recipe
            public const string RecipeNotFoundExceptionMessage = "Recipe not found.";

            // For Recipe collections
            public const string NoRecipesFoundExceptionMessage = "No recipe records found in the database";

            // INGREDIENT:
            public const string IngredientNotFoundExceptionMessage = "Ingredient not found.";

            // FAV RECIPES
            public const string FavouriteRecipeNotFoundExceptionMessage = "Favourite recipe not found.";
        }

        public static class InvalidOperationExceptionMessages
        {
            public const string InvalidRecipeOperationDueToMealPlansInclusion = "Please note this recipe is included in meal plans!";
        }
    }
}
