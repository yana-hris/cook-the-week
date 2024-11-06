namespace CookTheWeek.Services.Data.Factories
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;

    public interface IMealPlanViewModelFactory
    {
        /// <summary>
        /// Creates the model for meal plan Add View from the received Service model. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>MealPlanAddFormModel</returns>
        /// <remarks>May throw a RecordNotFound exception from the GetRecipeById method.</remarks>
        Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel model);

        /// <summary>
        /// Returns a single meal plan for Details View or throws an exception if the mealplan is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanDetailsViewModel> CreateMealPlanDetailsViewModelAsync(Guid mealplanId);

        /// <summary>
        /// Returns a collection of all user`s mealplans MealPlanAllViewModel or throws an exception if no meal plans found (collection is empty)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A collection of MealPlanAllViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ICollection<MealPlanAllViewModel>> CreateMealPlansMineViewModelAsync();

        /// <summary>
        /// Retrieves all active meal plans and maps them to a collection of MealPlanAllAdminViewModel objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a collection of MealPlanAllAdminViewModel objects for active meal plans.</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> CreateAllActiveMealPlansAdminViewModelAsync();

        /// <summary>
        /// Returns a Viewmodel collection for Admin view of all finished user meal plans at the moment
        /// </summary>
        /// <returns>A collection of MealPlanAllAdminViewModel</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> CreateAllFinishedMealPlansAdminViewModelAsync();

        /// <summary>
        /// A generic method which creates either MealPlanAddFormModel (for copying an existing meal plan) or MealPlanEditFormModel (for editing purposes)
        /// </summary>
        /// <typeparam name="TFormModel"></typeparam>
        /// <param name="id">an existing meal plan`s ID</param>
        /// <returns>MealPlanAddFormModel or MealPlanEditFormModel</returns>
        Task<TFormModel> CreateMealPlanFormModelAsync<TFormModel>(Guid id)
            where TFormModel : IMealPlanFormModel, new();

        /// <summary>
        /// Creates a MealAddFormModel from a service model. Throws an exception if the specific recipe is not found in the database
        /// </summary>
        /// <param name="meal"></param>
        /// <remarks>May throw a RecordNotFoundException due to usage of GetByIdAsync method.</remarks>
        /// <returns>MealAddFormModel</returns>
        Task<MealFormModel> CreateMealAddFormModelAsync(MealServiceModel meal);

        /// <summary>
        /// Adds Select Dates for cooking meals in all instances of IMealPlanFormModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns>the model itself</returns>
        T AddMealCookSelectDates<T>(T model) where T : IMealPlanFormModel;
    }
}
