namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IMealplanRepository
    {
        /// <summary>
        /// Returns a queryable collection of all mealplans in the database that can be filtered, sorted and materialized with any of the Async methods
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

        /// <summary>
        /// Updates an existing meal plan
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        Task UpdateAsync(MealPlan mealPlan);

        /// <summary>
        /// Deletes a single meal plan
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(MealPlan mealPlan);

        /// <summary>
        /// Deletes a collection of meal plans
        /// </summary>
        /// <param name="mealPlans"></param>
        /// <returns></returns>
        Task DeleteAllAsync(ICollection<MealPlan> mealPlans);

    }
}
