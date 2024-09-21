namespace CookTheWeek.Services.Data.Interfaces
{
    
    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        Task<UserProfileViewModel?> GetProfileDetailsAync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
        Task DeleteUserAndUserDataAsync(string userId);
    }
}
