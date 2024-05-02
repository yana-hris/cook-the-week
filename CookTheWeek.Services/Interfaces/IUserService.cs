namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;

    public interface IUserService
    {
        Task<ICollection<UserAllViewModel>> AllAsync();
        Task<int> AllCountAsync();
        Task<bool> ExistsByIdAsync(string id);
        Task<bool> IsOwnerByMealPlanId(string id, string userId);
        Task<bool> IsOwnerByRecipeId(string recipeId, string userId);
    }
}
