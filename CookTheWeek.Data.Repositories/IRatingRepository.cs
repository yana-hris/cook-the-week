namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;

    public interface IRatingRepository
    {
        /// <summary>
        /// Returns a queryable collection of all existing Recipe-User Ratings 
        /// </summary>
        /// <returns>A collection of FavouriteRecipe</returns>
        IQueryable<RecipeRating> GetAllQuery();


        /// <summary>
        /// Updates a collection of Recipe User Ratings
        /// </summary>
        /// <param name="allRatings"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(ICollection<RecipeRating> allRatings);

        /// <summary>
        /// Deletes a collection of entries of type Recipe User Ratings
        /// <param name="userLikes"></param>
        /// <returns></returns>
        Task DeleteRangeAsync(ICollection<RecipeRating> allRatings);
    }
}
