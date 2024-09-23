namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Web.ViewModels.Meal;

    public interface IMealService
    {
        /// <summary>
        /// Returns a detailed view model for a meal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException">if the meal or recipe is not found</exception>
        Task<MealDetailsViewModel> GetForDetailsAsync(int id);

        /// <summary>
        /// Deletes all meals, included in a specific meal plan (by id)
        /// </summary>
        /// <param name="id">Meal Plan Id</param>
        /// <returns></returns>
        Task DeleteAllByMealPlanIdAsync(string mealplanId);

        /// <summary>
        /// Deletes all meals, cooked by a specific recipe Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAllByRecipeIdAsync(string recipeId);

        /// <summary>
        /// Transforms a collection of MealAddFormModel into a collection of Meals and adds them to the database
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
        Task AddAllAsync(IList<MealAddFormModel> meals);
    }
}
