namespace CookTheWeek.Services.Data.Interfaces
{
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
        Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId); // Ok

        /// <summary>
        /// Adds a Recipe to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>The newly created Recipe Id</returns>
        Task<string> AddAsync(RecipeAddFormModel model, string userId, bool isAdmin); // Ok

        /// <summary>
        /// Edits an existing Recipe
        /// </summary>
        Task EditAsync(RecipeEditFormModel model);

        /// <summary>
        /// Returns a Detailed Viewmodel for a specific recipe
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

        // TODO: move to recipeFactory
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin);

        
        /// <summary>
        /// Deletes a specific recipe and all its nested entities: recipe ingredients, recipe steps, meals and likes
        /// </summary>
        Task DeleteByIdAsync(string id, string userId, bool isAdmin);

        /// <summary>
        /// Returns a viewmodel collection of all recipes, added by a specific user
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
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
        /// 
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
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
    }
}
