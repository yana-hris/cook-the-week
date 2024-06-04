namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Data.Interfaces;

    public class UserAdminService : IUserAdminService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserAdminService(CookTheWeekDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
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
