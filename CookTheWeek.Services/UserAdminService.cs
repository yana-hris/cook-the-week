namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using Data.Interfaces;

    public class UserAdminService : IUserAdminService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IRecipeService recipeService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserAdminService(CookTheWeekDbContext dbContext, IRecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
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
        
    }
}
