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
        Task<FavouriteRecipe?> GetByIdAsync(string userId, string recipeId);

        /// <summary>
        ///  Adds a given recipe to a user`s favourites
        /// </summary>     
        Task AddAsync(FavouriteRecipe like);

        /// <summary>
        /// Removes a given recipe from a user`s favourites 
        /// </summary> 
        Task DeleteAsync(FavouriteRecipe like);

        /// <summary>
        /// Deletes a collection of entries of type favourite recipe (user likes)
        /// </summary>
        /// <param name="userLikes"></param>
        /// <returns></returns>
        Task DeleteAllAsync(ICollection<FavouriteRecipe> userLikes);

        /// <summary>
        /// Sets the IsDeleted flag of the like as true
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        Task SoftDeleteAsync(FavouriteRecipe like);

    }
}
