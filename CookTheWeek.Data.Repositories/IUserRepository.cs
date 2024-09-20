namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserRepository
    {
        Task<bool> ExistsByIdAsync(string id);
        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<bool> HasPasswordAsync(ApplicationUser user);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IdentityResult> ChangePassAsync(ApplicationUser user, string currentPass, string newPass);

        Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password);

        Task DeleteAsync(ApplicationUser user);
    }
}
