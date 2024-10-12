
namespace CookTheWeek.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;

    public class UserRepository : IUserRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public UserRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>   
        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await dbContext
                .Users
                .AnyAsync(user => user.Id == id);
        }
        
        /// <inheritdoc/>
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var user = await dbContext.Users
                .Where(user => user.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

            return user;
        }

        /// <inheritdoc/>       
        public async Task<ApplicationUser?> GetByUsernameAsync(string userName)
        {
            return await dbContext.Users
                .Where(user => user.UserName.ToLower() == userName.ToLower())
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == id);
        }

       
        /// <inheritdoc/>
        public async Task<int?> AllCountAsync()
        {
            int? users = await dbContext
                .Users
                .AsNoTracking()
                .CountAsync();

            return users;
        }

        /// <inheritdoc/>
        public IQueryable<ApplicationUser> GetAllQuery()
        {
            return this.dbContext
                .Users
                .AsQueryable();
                
        }
       
        
    }
}
