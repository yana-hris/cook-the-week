namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IFavouriteRecipeRepository
    {
        /// <summary>
        /// Returns a queryable collection of all existing user likes 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<FavouriteRecipe> GetAllQuery();


        /// <summary>
        /// Returns the like if existing or throws an exception
        /// </summary>
        /// <returns>FavouriteRecipe</returns>
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

    }
}
