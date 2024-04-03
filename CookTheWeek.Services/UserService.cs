namespace CookTheWeek.Services.Data
{
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly CookTheWeekDbContext dbContext;
        public UserService(CookTheWeekDbContext dbContext)
        {
             this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UserViewModel>> AllAsync()
        {
            var allUsers = await this.dbContext.Users
                .Select(async u => new
                {
                    User = u,
                    RecipeCountTask = MineCount(u.Id.ToString())
                })
                .ToListAsync();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in allUsers)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.User.Id.ToString(),
                    Username = user.User.UserName!,
                    TotalMealPlans = user.User.MealPlans.Count(),
                    TotalRecipes = await user.RecipeCountTask
                };

                userViewModels.Add(userViewModel);
            }

            return userViewModels;
        }

        public async Task<int> MineCount(string id)
        {
            ICollection<Recipe> recipes = await this.dbContext
                .Recipes
                .Where(r => r.OwnerId == id && r.IsDeleted == false)
                .ToListAsync();

            return recipes.Count();
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            bool exists = await this.dbContext.Users
                .AnyAsync(u => u.Id.ToString() == id);

            return exists;
        }
    }
}
