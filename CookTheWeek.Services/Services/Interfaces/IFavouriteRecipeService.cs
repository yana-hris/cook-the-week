namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;

    public interface IFavouriteRecipeService
    {
        /// <summary>
        /// Rceives the service model, validates it and if all is valid, creates or deletes the user like (depending on its existence).
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>May throw the following exceptions due to usage of validation service:
        /// ArgumentNullException (if recipeId is null)
        /// RecordNotFoundException (if recipe does not exist)
        /// UnauthorizedUserException (if user is not logged in or if id-s do not match)</remarks>
        /// <returns>The result of the Operation</returns>
        Task<OperationResult> TryToggleLikesAsync(FavouriteRecipeServiceModel model);

        /// <summary>
        /// Gets a collection of the ids of all recipes, liked by the current user
        /// </summary>  
        /// <returns>A collection of FavouriteRecipe</returns>
        Task<ICollection<string>> GetAllRecipeIdsLikedByCurrentUserAsync();

        /// <summary>
        /// Returns true if the user has liked a specific recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>true or false</returns>
        Task<FavouriteRecipe?> GetRecipeLikeIfExistsAsync(Guid recipeId);

        /// <summary>
        /// Returns the total count of likes for a recipe by Id or 0
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> GetRecipeTotalLikesAsync(Guid recipeId);

       
        /// <summary>
        /// Sets all liked recipes isDeleted tag to true
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid recipeId);
    }
}
