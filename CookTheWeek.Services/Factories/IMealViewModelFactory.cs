namespace CookTheWeek.Services.Data.Factories
{
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
        Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId);


    }
}
