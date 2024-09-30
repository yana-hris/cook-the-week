namespace CookTheWeek.Common
{
    public static class ExceptionMessagesConstants
    {
        public static class DataRetrievalExceptionMessages
        {
            public const string RecipeDataRetrievalExceptionMessage = "Recipe data retrieval failed.";

            public const string IngredientDataRetrievalExceptionMessage = "Ingredient data retrieval failed.";

            public const string MealplanDataRetrievalExceptionMessage = "Meal plan data retrieval failed.";

            public const string CategoryDataRetrievalExceptionMessage = "Recipe Categories data retrieval failed.";

            public const string RecipeIngredientMeasuresDataRetrievalExceptionMessage = "Recipe Ingredient Measures data retrieval failed.";

            public const string RecipeIngredientSpecificationsDataRetrievalExceptionMessage = "Recipe Ingredient Specifications data retrieval failed.";

            public const string FavouriteRecipeDataRetrievalExceptionMessage = "User like for a recipe data retrieval failed.";

            public const string RecipeTotalLikesDataRetrievalExceptionMessage = "Recipe likes total count data retrieval failed.";

            public const string MealsTotalCountDataRetrievalExceptionMessage = "Meals total count data retrieval failed.";

        }

        public static class  UnauthorizedExceptionMessages 
        {
            // RECIPE:
            public const string RecipeEditAuthorizationExceptionMessage = "You do not have permission to edit this recipe.";
            public const string RecipeDeleteAuthorizationMessage = "You do not have permission to delete this recipe.";

            // USER:
            public const string UserNotLoggedInExceptionMessage = "You need to be logged in to proceed.";

            //MEAL PLAN:
            public const string MealplanEditAuthorizationExceptionMessage = "You do not have permission to edit this meal plan.";
            public const string MealplanDeleteAuthorizationMessage = "You do not have permission to delete this meal plan.";
        }

        public static class  RecordNotFoundExceptionMessages
        {
            // RECIPE:
            // For single Recipe
            public const string RecipeNotFoundExceptionMessage = "Recipe not found.";
            // For Recipe collections
            public const string NoRecipesFoundExceptionMessage = "No recipes found by this criteria.";

            // INGREDIENT:
            public const string IngredientNotFoundExceptionMessage = "Ingredient not found.";

            // FAV RECIPES:
            public const string FavouriteRecipeNotFoundExceptionMessage = "Favourite recipe not found.";

            // MEAL:
            public const string MealNotFoundExceptionMessage = "Meal not found.";

            // MEALPLAN:
            // For single MealPlan
            public const string MealplanNotFoundExceptionMessage = "Mealplan not found.";
            // For MealPlan collections:
            public const string NoMealplansFoundExceptionMessage = "No mealplans found by this criteria.";

            // USER:
            public const string UserNotFoundExceptionMessage = "No user found by this id";

            // CATEGORY:
            public const string CategoryNotFoundExceptionMessage = "No category found by this id.";
        }

        /// <summary>
        /// Exception Messages, returned when creating and editing entities in the database results in an error
        /// </summary>
        public static class InvalidOperationExceptionMessages
        {
            // RECIPE:
            public const string InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage = "This Recipe is included in active mealplans and cannot be deleted right now.";

            public const string RecipeUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe was not added to the Database.";

            public const string RecipeIngredientsUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe ingredients were not added to the Database.";

            public const string RecipeStepsUnsuccessfullyAddedExceptionMessage = "Something went wrong and the recipe steps were not added to the Database.";

            // INGREDIENT:
            public const string IngredientCannotBeDeletedExceptionMessage = "Ingredient is included in Recipes and cannot be deleted.";

            // MEALPLAN:
            public const string MealplanUnsuccessfullyAddedExceptionMessage = "Something went wrong and the mealplan was not added to the Database.";

            public const string MealplanUnsuccessfullyEditedExceptionMessage = "Something went wrong and the mealplan was not edited.";

            
            // USER:
            public const string UserUnsuccessfullyCreatedExceptionMessage = "User creation failed.";

            public const string UserUnsuccessfullyDeletedExceptionMessage = "User deletion failed.";

            public const string TokenGenerationUnsuccessfullExceptionMessage = "User Token unsuccessfully generated.";

            // CATEGORY:
            public const string CategoryUnsuccessfullyAddedExceptionMessage = "Something went wrong and the category was not added to the Database.";

            public const string CategoryUnsuccessfullyEditedExceptionMessage = "Something went wrong and the category was not edited.";

            public const string CategoryUnsuccessfullyDeletedExceptionMessage = "Something went wrong and the category was not deleted.";

            public const string CategoryCannoBeDeletedExceptionMessage = "Category has dependencies and cannot be deleted.";
        }

        public static class SmtpExceptionMessages
        {
            public const string EmailConfirmationUnsuccessfullySentToUser = "The token for email confirmation was unsuccessfully sent to the user.";
        }

        public static class InvalidCastExceptionMessages
        {
            public const string RecipeAddOrEditModelUnsuccessfullyCasted = "Failed to cast RecipeFormModel.";

            public const string IngredientAddOrEditModelUnsuccessfullyCasted = "Failed to cast IngredientFormModel.";
        }

        public static class ArgumentNullExceptionMessages
        {
            public const string TokenNullExceptionMessage = "The validation token is null.";

            public const string RecipeNullExceptionMessage = "The Recipe is null";

            public const string UserNullExceptionMessage = "The User is null.";
        }
    }
}
