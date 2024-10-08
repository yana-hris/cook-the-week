namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IStepRepository
    {
        /// <summary>
        /// Gets a queryable collection of all Steps, which can be filtered and materialized (awaited with any of the Async LINQ methods).
        /// </summary>
        /// <returns>A queryable collection of Steps</returns>
        IQueryable<Step> GetAllQuery();

        /// <summary>
        /// Adds a collection of steps to the database
        /// </summary>
        Task AddRangeAsync(ICollection<Step> steps);

        /// <summary>
        /// Updates a collection of Steps in the database
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(ICollection<Step> steps);

        /// <summary>
        /// Deletes a collection of Steps
        /// </summary>
        /// <param name="recipeId">recipeId</param>
        Task DeleteRangeAsync(ICollection<Step> steps);

        
    }
}
