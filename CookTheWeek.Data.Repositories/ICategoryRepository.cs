
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models.Interfaces;

    public interface ICategoryRepository<TCategory> 
        where TCategory : class, ICategory, new()
    {
        /// <summary>
        /// Gets a queryable collection of all categories of type T, which can be used for filtering and sorting and can be materialized with any of the Async methods
        /// </summary>
        /// <returns>A collection of IQueryable of TCategory</returns>
        IQueryable<TCategory> GetAllQuery();

        /// <summary>
        /// Adds a TCategory to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TCategory entity);

        /// <summary>
        /// Edits the current TCategory and persists changes in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TCategory entity);

        /// <summary>
        /// Finds a TCategory by id and deletes it from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(int id);

        /// <summary>
        /// Gets a given TCategory by id or returns null if it does not exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TCategory or null</returns>
        Task<TCategory?> GetByIdAsync(int id);

        /// <summary>
        /// Checks if a given category exists by its id and returns a flag, indicating so
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Gets the category id by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int or null</returns>
        Task<int?> GetIdByNameAsync(string name);

        /// <summary>
        /// Checks if any of the TDependecy entity table has a foreign key categoryId. If true => there are existing dependecies
        /// </summary>
        /// <typeparam name="TDependency"></typeparam>
        /// <param name="categoryId"></param>
        /// <returns>true or false</returns>
        Task<bool> HasDependenciesAsync<TDependency>(int categoryId) where TDependency : class;
    }
}
