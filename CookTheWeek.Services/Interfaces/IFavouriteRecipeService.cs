namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IFavouriteRecipeService
    {
        Task<bool> ExistsByUserIdAsync(string id, string userId);
        Task AddByUserIdAsync(string id, string userId);
        Task RemoveByUserIdAsync(string id, string userId);
        Task<ICollection<RecipeAllViewModel>> AllByUserIdAsync(string userId);
    }
}
