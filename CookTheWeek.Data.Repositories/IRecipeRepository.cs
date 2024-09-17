namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeRepository
    {
        Task<ICollection<Recipe>> GetAllAsync();
        Task<string> AddAsync(Recipe recipe);

        Task<Recipe> GetByIdAsync(string id);

        Task<bool> ExistsByIdAsync(string id);

        Task<Recipe> GetForEditByIdAsync(string id);
        Task UpdateAsync(Recipe recipe);

        Task DeleteByIdAsync(string id);

        Task<ICollection<Recipe>>? GetAllByUserIdAsync(string userId);

        Task<int> GetMineCountAsync(string userId);

        Task<int> GetAllCountAsync();

        Task<bool> IsIncludedInMealPlansAsync(string id);

        Task<Recipe> GetForMealByIdAsync(string recipeId);

        Task<ICollection<Recipe>> GetAllSiteAsync();

        Task<ICollection<Recipe>> GetAllUserRecipesAsync();
    }
}
