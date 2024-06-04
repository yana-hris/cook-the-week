namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int> AllCountAsync();       
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwnerByMealPlanIdAsync(string id, string userId);
        Task<bool> IsOwnerByRecipeIdAsync(string recipeId, string userId);
    }
}
