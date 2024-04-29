namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.Admin.UserAdmin;

    public class UserService : IUserService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IRecipeService recipeService;
        public UserService(CookTheWeekDbContext dbContext, IRecipeService recipeService)
        {
            this.dbContext = dbContext;
            this.recipeService = recipeService;
        }

        public async Task<ICollection<UserAllViewModel>> AllAsync()
        {
            ICollection<UserAllViewModel> allUsers = await this.dbContext
                .Users
                .Select(u => new UserAllViewModel()
                {
                    Id = u.Id.ToString(),
                    Username = u.UserName!,
                    Email = u.Email!,
                    TotalMealPlans = u.MealPlans.Count()
                })
                .ToListAsync();

            foreach (var user in allUsers)
            {
                user.TotalRecipes = await this.recipeService.MineCountAsync(user.Id);
            }

            return allUsers;

        }

        public async Task<int> AllCountAsync()
        {
            return await this.dbContext
                .Users
                .CountAsync();
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);
        }

        public async Task<bool> IsOwnerByRecipeId(string recipeId, string userId)
        {
            return await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == recipeId && r.OwnerId == userId)
                .AnyAsync();
        }

        public async Task<bool> IsOwnerByMealPlanId(string userId, string id)
        {
            return await this.dbContext
                .MealPlans
                .AnyAsync(mp => mp.Id.ToString() == id &&
                          mp.OwnerId.ToString() == userId);
        }
    }
}
