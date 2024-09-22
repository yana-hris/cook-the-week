namespace CookTheWeek.Services.Data.Interfaces
{
    
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Web.ViewModels.User;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Data.Models;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int?> AllCountAsync();
        Task<UserProfileViewModel> DetailsByIdAsync(string userId);
        Task<ApplicationUser> CreateUserAsync(RegisterFormModel model);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
        Task DeleteUserAndUserDataAsync(string userId);
        
    }
}
