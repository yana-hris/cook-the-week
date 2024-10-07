namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IStepRepository
    {
        /// <summary>
        /// Adds a collection of steps to the database
        /// </summary>
        Task AddAllAsync(ICollection<Step> steps);
                
        /// <summary>
        /// Deletes a collection of Steps
        /// </summary>
        /// <param name="recipeId">recipeId</param>
        Task DeleteAllAsync(ICollection<Step> steps);

        /// <summary>
        /// Gets a queryable collection of all Steps, which can be filtered and materialized (awaited with any of the Async LINQ methods).
        /// </summary>
        /// <returns>A queryable collection of Step</returns>
        IQueryable<Step> GetAllQuery();

        /// <summary>
        /// Soft deletes a step by setting its IsDeleted flag to true
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        Task SoftDeleteAsync(Step step);
    }
}
