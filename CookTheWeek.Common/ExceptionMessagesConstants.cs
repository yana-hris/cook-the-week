﻿namespace CookTheWeek.Common
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
            public const string RecipeEditAuthorizationExceptionMessage = "You must be the recipe owner to edit it.";

            public const string RecipeDeleteAuthorizationMessage = "You must be the recipe owner to delete it.";
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

            // For MEAL:
            public const string MealNotFoundExceptionMessage = "Meal not found.";

            // For MEALPLAN:
            public const string MealplanNotFoundExceptionMessage = "Mealplan not found.";

            // For USER:
            public const string UserNotFoundExceptionMessage = "No user found by this id";
        }

        /// <summary>
        /// Exception Messages, returned when creating and editing entities in the database results in an error
        /// </summary>
        public static class InvalidOperationExceptionMessages
        {
            public const string InvalidRecipeOperationDueToMealPlansInclusion = "Please note this recipe is included in meal plans!";

            public const string RecipeUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe was not added to the Database.";

            public const string RecipeIngredientsUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe ingredients were not added to the Database.";

            public const string RecipeStepsUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe steps were not added to the Database.";

            public const string MealplanUnsuccessfullyAddedExceptionMessage = "Something went wrong and the mealplan was not added to the Database.";

            public const string UserUnsuccessfullyCreatedException = "User creation failed.";
        }

        public static class SmtpExceptionMessages
        {
            public const string EmailConfirmationUnsuccessfullySentToUser = "The token for email confirmation was unsuccessfully sent to the user.";
        }

        public static class InvalidCastExceptionMessages
        {
            public const string RecipeAddOrEditModelUnsuccessfullyCasted = "Failed to cast RecipeFormModel.";
        }

        public static class ArgumentNullExceptionMessages
        {
            public const string TokenNullExceptionMessage = "The validation token is missing.";
        }
    }
}
