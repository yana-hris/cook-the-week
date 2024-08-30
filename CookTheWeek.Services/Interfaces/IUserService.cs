namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwnerByMealPlanIdAsync(string id, string userId);
        Task<bool> IsOwnerByRecipeIdAsync(string recipeId, string userId);
        Task DeleteUserAsync(string userId);
        Task<UserProfileViewModel?> GetProfile(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model);
        Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model);
    }
}
