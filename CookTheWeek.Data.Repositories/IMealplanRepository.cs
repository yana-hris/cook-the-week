namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IMealplanRepository
    {
        /// <summary>
        /// Gets a queryable collection of all mealplans in the database
        /// </summary>
        /// <returns>A queryable collection of MealPlan</returns>
        IQueryable<MealPlan> GetAllQuery();

        /// <summary>
        /// Gets a Mealplan by id, including all its meals, their recipes with their categories and recipeingredients
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single MealPlan or null</returns>
        Task<MealPlan?> GetByIdAsync(string id);

        /// <summary>
        /// Creates a new Mealplan in the database
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns>the newly created MealPlan Id as string</returns>
        Task<string> AddAsync(MealPlan mealPlan);

        Task UpdateAsync(MealPlan newMealPlan);

        Task DeleteByIdAsync(string id);

    }
}
