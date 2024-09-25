namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IFavouriteRecipeRepository
    {
        /// <summary>
        ///  Gets all liked recipes by a given user id as a collection
        /// </summary>  
        /// <returns>A collection of FavouriteRecipe</returns>
        Task<ICollection<FavouriteRecipe>> GetAllByUserIdAsync(string userId);

        /// <summary>
        ///  Returns the total likes for a recipe by Id
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllCountByRecipeIdAsync(string recipeId);

        /// <summary>
        ///  Checks if a recipe is added to the user`s favourites and returns boolean
        /// </summary>
        /// <returns>true or false</returns>
        Task<bool> GetByIdAsync(string userId, string recipeId);

        /// <summary>
        ///  Adds a given recipe to a user`s favourites by ids
        /// </summary>     
        Task AddAsync(string userId, string recipeId);

        /// <summary>
        /// Removes a given recipe from a user`s favourites by ids or throws an exception
        /// </summary> 
        /// <exception cref="RecordNotFoundException"></exception>
        Task DeleteAsync(string userId, string recipeId);

        /// <summary>
        /// Deletes all likes for all recipes by a given user id from the favurite-recipe table.  If there are not likes - does nothing.
        /// </summary>
        Task DeleteAllByUserIdAsync(string userId);

        /// <summary>
        /// Deletes all likes for all users by a given recipe id from the favourite-recipe table. If there are not likes - does nothing.
        /// </summary>
        Task DeleteAllByRecipeIdAsync(string recipeId);
    }
}
