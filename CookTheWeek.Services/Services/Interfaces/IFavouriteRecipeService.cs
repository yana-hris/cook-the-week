namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;

    public interface IFavouriteRecipeService
    {

        /// <summary>
        /// Gets all liked recipes by a given user id as a collection of FavouriteRecipe
        /// </summary>  
        /// <returns>A collection of FavouriteRecipe</returns>
        Task<ICollection<FavouriteRecipe>> GetAllRecipesLikedByUserIdAsync(string userId);

        /// <summary>
        /// Returns true if the user has liked a specific recipe
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns>true or false</returns>
        Task<bool> HasUserByIdLikedRecipeById(string userId, string recipeId);

        /// <summary>
        /// Returns the total count of likes for a recipe by Id or 0
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> GetRecipeTotalLikesAsync(string recipeId);

        /// <summary>
        /// Deletes a user like for a specific recipe if it exists. Otherwise does nothing
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task DeleteLikeAsync(string userId, string recipeId);

        /// <summary>
        /// Adds a like by a given user id for a recipe with a given recipe id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task AddLikeAsync(string userId, string recipeId);

        /// <summary>
        /// Deletes all likes for all users by a given recipe id from the favourite-recipe table. If there are no likes - does nothing.
        /// </summary>
        Task DeleteAllRecipeLikesAsync(string recipeId);
    }
}
