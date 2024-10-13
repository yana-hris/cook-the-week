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
        /// Adds a collection of steps to the entity withour Saving changes
        /// </summary>
        /// <remarks>A consequent SaveChangesAsync() required to persist the changes in the database</remarks>
        void AddRange(ICollection<Step> steps);
        
        /// <summary>
        /// Marks a collection of Steps for deletion withou persisting changes in the database
        /// </summary>
        /// <param name="recipeId">recipeId</param>
        ///<remarks>A consequent SaveChangesAsync() required to persist the changes in the database</remarks>
        void DeleteRange(ICollection<Step> steps);

        /// <summary>
        /// Persists the changes in the Database
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
