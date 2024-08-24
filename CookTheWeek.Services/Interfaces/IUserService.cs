namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;

    public interface IUserService
    {
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwnerByMealPlanIdAsync(string id, string userId);
        Task<bool> IsOwnerByRecipeIdAsync(string recipeId, string userId);
        Task DeleteUserAsync(string userId);
    }
}
