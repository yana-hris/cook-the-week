
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IMealRepository
    {
        Task<Meal> GetByIdAsync(int id);

        Task<int?> GetAllCountByRecipeIdAsync(string recipeId);

        Task DeleteAllByRecipeIdAsync(string recipeId);
    }
}
