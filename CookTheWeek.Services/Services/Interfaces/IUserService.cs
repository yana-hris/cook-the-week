namespace CookTheWeek.Services.Data.Services.Interfaces
{

    using Microsoft.AspNetCore.Identity;

    using CookTheWeek.Web.ViewModels.User;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Data.Models;
    using System.Security.Claims;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int?> AllCountAsync();
        Task<UserProfileViewModel> DetailsByIdAsync(string userId);
        Task<ApplicationUser> CreateUserAsync(RegisterFormModel model);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
        Task DeleteUserAndUserDataAsync(string userId);

        /// <summary>
        /// Generates a token for the user to confirm registration account via email. 
        /// If it fails, throws an InvalidOperationException. If the user is null, throws an ArgumentNullException
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task<string> GenerateTokenForEmailConfirmationAsync(ApplicationUser user);

        /// <summary>
        /// Deletes the user from the database. If the user is null, throws an ArgumentNullException. If the operation fails, throws an InvalidOperationException.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Task DeleteUserAsync(ApplicationUser user);

        /// <summary>
        /// Gets the currenly logged in user and returns his/her id
        /// </summary>
        /// <returns>string or null</returns>
        string? GetCurrentUserId();
    }
}
