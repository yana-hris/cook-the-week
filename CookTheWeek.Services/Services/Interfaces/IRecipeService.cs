namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {
        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters
        /// </summary>
        /// <param name="queryModel">The user input as view model</param>
        /// <param name="userId">The current user if any (may be null)</param>
        /// <returns>A collection of the sorted and filtered recipes</returns>
        Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId); 

        /// <summary>
        /// Takes the model and validates further for any errors, if the model is correct, persists the Recipe in the database
        /// </summary>
        /// <param name="model">The Recipe form model to create a new recipe from</param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>The Result of the Operation: Success/Failure (and the recipeId if Success)</returns>
        /// <remarks>May throw RecordNotFoundException due to underlying usage of GetById method</remarks>
        Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model, string userId, bool isAdmin); 

        /// <summary>
        /// Edits an existing Recipe
        /// </summary>
        Task EditAsync(RecipeEditFormModel model);

        /// <summary>
        /// Creates a Detailed Viewmodel for a specific recipe
        /// </summary>
        /// <param name="id">Recipe Id</param>
        /// <returns>RecipeDetailsViewModel</returns>
        Task<RecipeDetailsViewModel> DetailsByIdAsync(string id); // Ok

        /// <summary>
        /// Returns true if the user has liked a specific recipe
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsLikedByUserAsync(string userId, string recipeId); //Ok

        /// <summary>
        /// Creates a RecipeEditFormModel or throws xceptions if recipe is not found or is not owned by the current user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="RecordNotFoundException">rethrown</exception>
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin);


        /// <summary>
        /// Deletes a specific (soft delete) recipe and all its nested nested and connected entities (hard delete): recipe ingredients, recipe steps, meals and likes. Throws an exception if the recipe is included in Meal Plans.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task DeleteByIdAsync(string id, string userId, bool isAdmin);

        /// <summary>
        /// Returns a viewmodel collection of all recipes, added by a specific user
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="DataRetrievalException"></exception>
        Task<ICollection<RecipeAllViewModel>> AllAddedByUserIdAsync(string userId); // Ok

        /// <summary>
        /// Returns the number of recipes added by a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int or 0</returns>
        Task<int?> MineCountAsync(string userId);

        /// <summary>
        /// Returns the count of all recipes
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Returns a flag indicating if a recipe is included in any meal plans or not
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsIncludedInMealPlansAsync(string recipeId);

        /// <summary>
        /// Returns a collection of all Site Recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> AllSiteAsync();

        /// <summary>
        /// Returns a collection of all users` recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> AllUserRecipesAsync();

        /// <summary>
        /// Returns all user-liked recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> AllLikedByUserAsync(string userId);

        /// <summary>
        /// Gets the total amount of likes for a recipe
        /// </summary>
        /// <returns>int</returns>
        Task<int?> GetAllRecipeLikesAsync(string recipeId);

        /// <summary>
        /// Gets the total amount of meals, cooked using a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>int?</returns>
        Task<int?> GetAllRecipeMealsCountAsync(string recipeId);

        /// <summary>
        /// Deletes all recipes that a user has added
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteAllByUserIdAsync(string userId);

        /// <summary>
        /// Checks if a user has liked a given recipe and if yes, unlikes it. If no, likes it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns>Task</returns>
        /// <remarks>May throw RecordNotFoundException</remarks>
        Task ToggleLike(string userId, string recipeId);
    }
}
