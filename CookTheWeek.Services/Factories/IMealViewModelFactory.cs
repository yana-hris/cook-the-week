namespace CookTheWeek.Services.Data.Factories
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;

    public interface IMealViewModelFactory
    {
        /// <summary>
        /// Generates a view model for a user`s meal by a given meal ID. May throw exceptions in case of missing data or database retrieval problem.
        /// </summary>
        /// <param name="mealId">Meal Id (int)</param>
        /// <returns>MealDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="DataRetrievalException"></exception>
        Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId, bool isMealPlanFinished);

        /// <summary>
        /// Creates a MealAddFormModel from a service model. Throws an exception if the specific recipe is not found in the database
        /// </summary>
        /// <param name="meal"></param>
        /// <remarks>May throw a RecordNotFoundException due to usage of GetByIdAsync method.</remarks>
        /// <returns>MealAddFormModel</returns>
        Task<MealFormModel> CreateMealAddFormModelAsync(MealServiceModel meal);

       

    }
}
