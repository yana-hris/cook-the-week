namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.MealPlan;
    using Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;

    public interface IMealPlanService
    {
        /// <summary>
        /// Returns a collection of MealPlanAllAdminViewModel, containing data for all users` active meal plans for the Admin Area
        /// </summary>
        /// <returns>A collection of MealPlanAllAdminViewModel</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync();

        /// <summary>
        /// Returns a collection of MealPlanAllAdminViewModel, containing data for all users` finished meal plans for the Admin Area
        /// </summary>
        /// <returns>A collection of MealPlanAllAdminViewModel</returns>
        Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync();

        /// <summary>
        /// Validates the mealplan model and if valid adds it to the database.
        /// If the OperationResult is Success, newly created mealplan ID is returned as Value.
        /// If the OperationResult is Failed, returns a dictionary with model Errors.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The result of the operation with Value(id) or Errors</returns>
        Task<OperationResult<string>> TryAddMealPlanAsync(MealPlanAddFormModel model);

        /// <summary>
        /// Validates and edits a mealplan or throws exceptions
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>Operation Result</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task<OperationResult> TryEditMealPlanAsync(MealPlanEditFormModel model);

        /// <summary>
        /// Returns a collection of all user`s mealplans MealPlanAllViewModel or throws an exception if no meal plans found (collection is empty)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A collection of MealPlanAllViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ICollection<MealPlanAllViewModel>> MineAsync();

        /// <summary>
        /// Returns the total amount of all user`s mealplans or 0 (not null)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int (default = 0)</returns>
        Task<int?> MineCountAsync();

        /// <summary>
        /// Returns a single meal plan for Details View or throws an exception if the mealplan is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id);

        /// <summary>
        /// Returns a single meal plan for Edit View or throws an exception if the mealplan is not found 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanEditFormModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanEditFormModel> GetForEditByIdAsync(string id);

        /// <summary>
        /// Returns a flag if a specific meal plan exists or not. Does not throw any exceptions
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(string id);

        /// <summary>
        /// Returns the total count of all Active meal plans. 
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllActiveCountAsync();

        /// <summary>
        /// Bulk delete for deleting all meal plans of a specific user, including deleting all nested meals. Does not throw any exceptions. If meal plans don`t exists, does nothing.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteAllByUserIdAsync(string userId);

        /// <summary>
        /// Creates the model for meal plan Add View from the received Service model. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>MealPlanAddFormModel</returns>
        /// <remarks>May throw a RecordNotFound exception from the GetRecipeById method.</remarks>
        Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel model);

        /// <summary>
        /// Either returns the MealPlan or throws an exception in case it does not exist.
        /// The returned meal plan contains data for the meals, their recipes, the recipe categories, their recipeingredients and associated ingredients.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlan</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlan> GetByIdAsync(string id);

        /// <summary>
        /// Validates if a mealplan exists and can be deleted by the current user. If so, deletes it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>May throw RecordNotFoundException or UnauthorizedUserException</remarks>
        Task TryDeleteByIdAsync(string id);

        /// <summary>
        /// Populates an empty MealPlanAddFormModel with meals data from an existing mealplan. If unsuccessful, throws an exception
        /// </summary>
        /// <param name="mealPlanId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<MealPlanAddFormModel> TryCopyMealPlanByIdAsync(string mealPlanId);
    }
}
