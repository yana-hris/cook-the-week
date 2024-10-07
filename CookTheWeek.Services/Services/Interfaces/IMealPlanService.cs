namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.MealPlan;

    public interface IMealPlanService
    {
        /// <summary>
        /// Returns a collection of MealPlans, containing data for all users` active meal plans for the Admin Area
        /// </summary>
        /// <returns>A collection of MealPlans</returns>
        Task<ICollection<MealPlan>> GetAllActiveAsync();

        /// <summary>
        /// Returns a collection of MealPlans, containing data for all users` finished meal plans for the Admin Area
        /// </summary>
        /// <returns>A collection of MealPlans</returns>
        Task<ICollection<MealPlan>> GetAllFinishedAsync();

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
        /// Gets all current use meal plans as a collection or throws an exception in case of none
        /// </summary>
        /// <returns>A collection of MealPlan</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ICollection<CookTheWeek.Data.Models.MealPlan>> GetAllMineAsync();

        /// <summary>
        /// Returns the total amount of all user`s mealplans or 0 (not null)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int (default = 0)</returns>
        Task<int?> MineCountAsync();

        /// <summary>
        /// Validates the existence and authorization rights of a the current user to edit the given meal plan and returns it 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A MealPlan</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlan> GetForFormModelById(string id);
       
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
        /// Either returns the MealPlan or throws an exception in case it does not exist.
        /// The returned meal plan contains data for the meals, their recipes, the recipe categories, their recipeingredients and associated ingredients.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlan</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<CookTheWeek.Data.Models.MealPlan> GetByIdAsync(string id);

        /// <summary>
        /// Validates if a mealplan exists and can be deleted by the current user. If so, deletes it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>May throw RecordNotFoundException or UnauthorizedUserException</remarks>
        Task TryDeleteByIdAsync(string id);

    }
}
