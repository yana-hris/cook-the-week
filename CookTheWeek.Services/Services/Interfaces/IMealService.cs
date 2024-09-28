namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CookTheWeek.Services.Data.Models.MealPlan;
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
        Task DeleteByRecipeIdAsync(string recipeId);

        /// <summary>
        /// Transforms a collection of MealAddFormModel into a collection of Meals and adds them to the database
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
        Task AddAllAsync(IList<MealAddFormModel> meals);

        /// <summary>
        /// Creates a MealAddFormModel from a service model. Throws an exception if the specific recipe is not found in the database
        /// </summary>
        /// <param name="meal"></param>
        /// <remarks>May throw a RecordNotFoundException due to usage of GetByIdAsync method.</remarks>
        /// <returns>MealAddFormModel</returns>
        Task<MealAddFormModel> CreateMealAddFormModelAsync(MealServiceModel meal);
    }
}
