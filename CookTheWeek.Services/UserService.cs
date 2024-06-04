namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;

    public class UserService : IUserService
    {
        private readonly CookTheWeekDbContext dbContext;
        
        public UserService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);
        }

        public async Task<bool> IsOwnerByRecipeIdAsync(string recipeId, string userId)
        {
            return await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == recipeId && r.OwnerId.ToString() == userId)
                .AnyAsync();
        }

        public async Task<bool> IsOwnerByMealPlanIdAsync(string id, string userId)
        {
            return await this.dbContext
                .MealPlans
                .AnyAsync(mp => mp.Id.ToString() == id &&
                          mp.OwnerId.ToString() == userId);
        }
               
    }
}
