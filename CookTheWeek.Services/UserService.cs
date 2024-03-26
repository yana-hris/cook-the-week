namespace CookTheWeek.Services.Data
{
    using CookTheWeek.Data;
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly CookTheWeekDbContext dbContext;
        public UserService(CookTheWeekDbContext dbContext)
        {
             this.dbContext = dbContext;
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            bool exists = await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);

            return exists;
        }
    }
}
