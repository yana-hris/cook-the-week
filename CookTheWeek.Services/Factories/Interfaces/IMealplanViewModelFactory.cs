namespace CookTheWeek.Services.Data.Factories.Interfaces
{
    using CookTheWeek.Web.ViewModels.Recipe;

    public interface IMealplanViewModelFactory
    {
        /// <summary>
        /// Generates a form model for adding a new meal to a mealplan 
        /// </summary>
        /// <returns>MealAddFormModel</returns>
        Task<RecipeAddFormModel> CreateRecipeAddFormModelAsync();
    }
}
