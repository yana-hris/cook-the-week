namespace CookTheWeek.Services.Data.Services.Interfaces
{

    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Common;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int?> AllCountAsync();
        Task<UserProfileViewModel> GetDetailsModelByIdAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
        Task DeleteUserAndUserDataAsync(string userId);
       
        /// <summary>
        /// Gets the currently logged in user and returns his/her id
        /// </summary>
        /// <returns>string or null</returns>
        string? GetCurrentUserId();

        
        Task<OperationResult> TryRegisterUserAsync(RegisterFormModel model);
        Task<OperationResult> TyrConfirmEmailAsync(string userId, string code);
        Task<OperationResult> TryLoginUserAsync(LoginFormModel model);
    }
}
