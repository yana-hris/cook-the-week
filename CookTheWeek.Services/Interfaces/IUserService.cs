namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsMealplanOwnerByIdAsync(string id, string userId);
        Task<bool> IsRecipeOwnerByIdAsync(string recipeId, string userId);
        Task DeleteUserAsync(string userId);
        Task<UserProfileViewModel?> GetProfileDetailsAync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
    }
}
