namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;

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
        public async Task<string[]> AllUsersInRoleIdsAsync(string roleName)
        {
            var allUsers = await this.userManager.Users
                .ToListAsync();

            var usersInRoleStringIds = new List<string>();

            foreach (var user in allUsers)
            {
                if (await userManager.IsInRoleAsync(user, roleName))
                {
                    usersInRoleStringIds.Add(user.Id.ToString());
                }
            }

            return usersInRoleStringIds.ToArray();

        }

        public async Task<string[]> AllUsersNotInRoleIdsAsync(string roleName)
        {
            var allUsers = await this.userManager.Users
                .ToListAsync();

            var usersNotInRoleStringIds = new List<string>();

            foreach (var user in allUsers)
            {
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    usersNotInRoleStringIds.Add(user.Id.ToString());
                }
            }

            return usersNotInRoleStringIds.ToArray();
        }
    }
}
