namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IFavouriteRecipeRepository
    {
        Task<ICollection<FavouriteRecipe>> GetAllByUserIdAsync(string userId);

        Task<int?> AllCountByRecipeIdAsync(string recipeId);

        Task<bool> GetByIdAsync(string userId, string recipeId);

        Task AddAsync(string userId, string recipeId);

        Task DeleteAsync(string userId, string recipeId);

        Task DeleteAllByUserIdAsync(string userId);

        Task DeleteAllByRecipeIdAsync(string recipeId);
    }
}
