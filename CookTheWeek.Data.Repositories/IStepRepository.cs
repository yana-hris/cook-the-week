namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IStepRepository
    {
        /// <summary>
        /// Gets a tracked queryable collection of all Steps, which can be filtered and materialized (awaited with any of the Async LINQ methods).
        /// </summary>
        /// <returns>A queryable collection of Steps</returns>
        IQueryable<Step> GetAllTrackedQuery();

        
        /// <summary>
        /// Persists the changes in the Database
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
