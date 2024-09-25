namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.MealPlan;
    using Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Services.Data.Models.MealPlan;

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
        /// Adds a meal plan to the database or throws an exception
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>mealPlan ID</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<string> AddAsync(string userId, MealPlanAddFormModel model);

        /// <summary>
        /// Edits a meal plan or throws an exception (if not found or user is not authorized). The edit includes updating all inclusive meals
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>mealPlan ID</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task EditAsync(string userId, MealPlanEditFormModel model);

        /// <summary>
        /// Returns a collection of all user`s mealplans MealPlanAllViewModel or throws an exception if no meal plans found (collection is empty)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A collection of MealPlanAllViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId);

        /// <summary>
        /// Returns the total amount of all user`s mealplans or 0 (not null)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int (default = 0)</returns>
        Task<int?> MineCountAsync(string userId);

        /// <summary>
        /// Returns a single meal plan for Details View or throws an exception if the mealplan is not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanDetailsViewModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id);

        /// <summary>
        /// Returns a single meal plan for Edit View or throws an exception if the mealplan is not found or the user is not the owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MealPlanEditFormModel</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        Task<MealPlanEditFormModel> GetForEditByIdAsync(string id, string userId);

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
        /// Finds and deleted a specific meal plan if it exists, deleting also all its nested meals. If a meal plan does not exists, throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task DeleteById(string id);

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
    }
}
