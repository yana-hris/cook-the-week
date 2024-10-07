
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
        Task AddAllAsync(ICollection<Meal> meals);

        /// <summary>
        /// Deletes all meals from a collection
        /// </summary>
        /// <param name="recipeId"></param>
        Task DeleteAll(ICollection<Meal> meals);

        /// <summary>
        /// Soft deletes a meal by setting its IsDeleted flag to true
        /// </summary>
        /// <param name="meal"></param>
        /// <returns></returns>
        Task SoftDeleteAsync(Meal meal);

    }
}
