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

        /// <summary>
        /// Marks a meal plan as expired if it has reached the expiration threshold of 7 full days 
        /// since its start date. This method updates the meal plan status to finished and adjusts 
        /// the user's active meal plan claim accordingly.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to check and potentially expire.</param>
        /// <param name="userId">The unique identifier of the user who owns the meal plan. Used to update the user’s meal plan claim.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method checks if 7 full days have passed since the <c>StartDate</c> of the meal plan 
        /// before marking it as expired. If the meal plan is expired, it updates the <c>IsFinished</c> 
        /// status and calls <see cref="IUserService.UpdateMealPlanClaimAsync"/> to ensure the user's claim 
        /// reflects the expired status. The method operates in UTC to ensure consistency across time zones.
        /// </remarks>
        /// <example>
        /// <code>
        /// await mealPlanService.ExpireMealPlanAsync(mealPlanId, userId);
        /// </code>
        /// </example>

        Task ExpireMealPlanAsync(Guid id, Guid userId);

        /// <summary>
        /// Returns the active MealPlan of the currently logged in user or throws an exception if not found
        /// </summary>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <returns>A single MealPlan</returns>
        Task<MealPlan> GetActiveAsync();

        /// <summary>
        /// Returns the ID of the currently logged in user`s active MealPlan
        /// </summary>
        /// <returns></returns>
        Task<Guid> GetActiveIdAsync();

        /// <summary>
        /// Gets a single Meal Plan including its deleted recipes (if any) are returns it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Includes deleted recipes as reference for the user.</remarks>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlan> GetUnfilteredByIdAsync(Guid id);

        /// <summary>
        /// Gets a single Meal Plan or throws an exception 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Does not include deleted recipes</remarks>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlan> GetByIdAsync(Guid id);
    }
}
