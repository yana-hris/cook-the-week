
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IMealRepository
    {
        /// <summary>
        /// Gets a meal by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Meal</returns>
        Task<Meal> GetByIdAsync(int id);

        /// <summary>
        /// Gets the count of all meals, cooked by a certain recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>int</returns>
        Task<int> GetAllCountByRecipeIdAsync(string recipeId);

        /// <summary>
        /// Deletes all meals, cooked by a certain recipe
        /// </summary>
        /// <param name="recipeId"></param>
        Task DeleteAllByRecipeIdAsync(string recipeId);
    }
}
