
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IMealRepository
    {
        /// <summary>
        /// Gets a queryable collection of all meals in the database, that can be filtered using LINQ
        /// </summary>
        /// <returns>A queryable collection of Meals</returns>
        IQueryable<Meal> GetAllQuery();
        /// <summary>
        /// Gets a meal by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Meal</returns>
        Task<Meal> GetByIdAsync(int id);

        /// <summary>
        /// Deletes all meals, cooked by a certain recipe
        /// </summary>
        /// <param name="recipeId"></param>
        Task DeleteAllByRecipeIdAsync(string recipeId);
       
    }
}
