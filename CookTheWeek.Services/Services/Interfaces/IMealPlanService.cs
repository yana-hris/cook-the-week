namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
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
        Task<ICollection<MealPlan>> GetAllMineAsync();

        /// <summary>
        /// Returns the total amount of all user`s mealplans or 0 (not null)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int (default = 0)</returns>
        Task<int?> MineCountAsync();

        /// <summary>
        /// Validates the existence of a meal plan by its id and authorization rights of the current user to access it. If not, throws exceptions
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A MealPlan</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task<MealPlan> TryGetAsync(Guid id);
       
        /// <summary>
        /// Returns the total count of all Active meal plans. 
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> AllActiveCountAsync();
        
       
        /// <summary>
        /// Validates if a mealplan exists and can be deleted by the current user. If so, deletes it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>May throw RecordNotFoundException or UnauthorizedUserException</remarks>
        Task TryDeleteByIdAsync(Guid id);

        /// <summary>
        /// Asynchronously updates the status of all active meal plans. 
        /// It marks meal plans as finished if they are older than 6 days and 
        /// sets all associated meals as cooked. 
        /// </summary>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the asynchronous operation.
        /// If cancellation is requested, the operation will stop before completion, 
        /// and an <see cref="OperationCanceledException"/> will be thrown.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous update operation.
        /// </returns>
        /// <remarks>
        /// The method retrieves all meal plans that are either unfinished or contain uncooked meals,
        /// iterates through them, and updates their status based on the business rule that meal plans
        /// older than 6 days are considered finished.
        /// </remarks>
        Task UpdateMealPlansStatusAsync(CancellationToken stoppingToken);
    }
}
