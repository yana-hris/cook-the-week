namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IUserRepository
    {
        /// <summary>
        /// Returns if a user by a specified id exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(Guid id);
        
        /// <summary>
        /// Gets the Application user if exists by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Application User or null</returns>
        Task<ApplicationUser?> GetByEmailAsync(string email);

        /// <summary>
        /// Gets the Application user if exists by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Application user or null</returns>
        Task<ApplicationUser?> GetByUsernameAsync(string userName);

        /// <summary>
        /// Returns the user, if existing or null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User or null</returns>
        Task<ApplicationUser?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the total count of all registered users in the database
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Returns an IQueryable collection of all users in the database, that can be filtered and sorted using LINQ. The collection can be materialized safely using the Async() methods
        /// </summary>
        /// <returns>An IQueryable collection of ApplicationUser</returns>
        IQueryable<ApplicationUser> GetAllQuery();
       
    }
}
