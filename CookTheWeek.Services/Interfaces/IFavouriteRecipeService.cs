namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IFavouriteRecipeService
    {
        Task<bool> IsFavouriteRecipeForUserByIdAsync(string id, string userId);
        Task AddToFavouritesByUserId(string id, string userId);
        Task RemoveFromFavouritesByUserId(string id, string userId);
        Task<ICollection<RecipeAllViewModel>> AllFavouritesByUserAsync(string userId);
    }
}
