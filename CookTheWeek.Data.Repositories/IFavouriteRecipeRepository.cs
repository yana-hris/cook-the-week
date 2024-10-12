namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IFavouriteRecipeRepository
    {
        /// <summary>
        /// Returns a queryable collection of all existing user likes 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A collection of FavouriteRecipe</returns>
        IQueryable<FavouriteRecipe> GetAllQuery();


        /// <summary>
        /// Returns the like (FavouriteRecipe) if existing or null
        /// </summary>
        /// <returns>FavouriteRecipe or null</returns>
        Task<FavouriteRecipe?> GetByIdAsync(Guid userId, Guid recipeId);

        /// <summary>
        ///  Adds a given recipe to a user`s favourites
        /// </summary>     
        Task AddAsync(FavouriteRecipe like);

        /// <summary>
        /// Updates a collection of user likes
        /// </summary>
        /// <param name="favourites"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(ICollection<FavouriteRecipe> favourites);

        /// <summary>
        /// Removes a given recipe from a user`s favourites 
        /// </summary> 
        Task DeleteAsync(FavouriteRecipe like);

        /// <summary>
        /// Deletes a collection of entries of type favourite recipe (user likes)
        /// </summary>
        /// <param name="userLikes"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(ICollection<FavouriteRecipe> userLikes);
        
    }
}
