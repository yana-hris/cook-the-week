
namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;

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
        /// <returns>Meal or null</returns>
        Task<Meal?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a collection of meals to the database
        /// </summary>
        /// <param name="newMeals"></param>
        /// <returns></returns>
        Task AddRangeAsync(ICollection<Meal> meals);

        /// <summary>
        /// Updates a collection of meals in the database
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(ICollection<Meal> meals);

        /// <summary>
        /// Deletes all meals from a collection
        /// </summary>
        /// <param name="recipeId"></param>
        Task RemoveRangeAsync(ICollection<Meal> meals);

    }
}
