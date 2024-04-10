namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.User;

    public class UserService : IUserService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IRecipeService recipeService;
        public UserService(CookTheWeekDbContext dbContext, IRecipeService recipeService)
        {
            this.dbContext = dbContext;
            this.recipeService = recipeService;
        }

        public async Task<ICollection<UserViewModel>> AllAsync()
        {
            ICollection<UserViewModel> allUsers = await this.dbContext
                .Users
                .Select(u => new UserViewModel()
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

        public async Task<int> AllUsersCountAsync()
        {
            return await this.dbContext
                .Users
                .CountAsync();
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            bool exists = await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);

            return exists;
        }

        public async Task<bool> IsOwner(string id, string ownerId)
        {
            bool isOwner = await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == id && r.OwnerId == ownerId)
                .AnyAsync();

            return isOwner;
        }
    }
}
