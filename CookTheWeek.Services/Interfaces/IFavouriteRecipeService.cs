namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IFavouriteRecipeService
    {
        Task<bool> IsLikedByUserIdAsync(string id, string userId);
        Task LikeAsync(string id, string userId);
        Task UnlikeAsync(string id, string userId);
        Task<ICollection<RecipeAllViewModel>> AllLikedByUserIdAsync(string userId);
        Task<int?> LikesCountAsync(string recipeId);
    }
}
