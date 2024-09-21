namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeRepository
    {
        IQueryable<Recipe> GetAllQuery();
        Task<string> AddAsync(Recipe recipe);
        Task<Recipe> GetByIdAsync(string id);
        Task UpdateAsync(Recipe recipe);

        Task<ICollection<Recipe>>? GetAllByUserIdAsync(string userId);
    }
}
