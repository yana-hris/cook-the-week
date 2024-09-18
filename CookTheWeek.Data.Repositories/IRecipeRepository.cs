namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeRepository
    {
        IQueryable<Recipe> GetAllQuery();
        Task<string> AddAsync(Recipe recipe);

        Task<Recipe?> GetByIdAsync(string id);
        Task UpdateAsync(Recipe recipe);

        Task DeleteByIdAsync(string id);

        Task<ICollection<Recipe>>? GetAllByUserIdAsync(string userId);

        Task<int> GetMineCountAsync(string userId);

        Task<bool> IsIncludedInMealPlansAsync(string id);

        Task<Recipe> GetForMealByIdAsync(string recipeId);
    }
}
