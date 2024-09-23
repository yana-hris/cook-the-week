namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.MealPlan;
    using Web.ViewModels.Admin.MealPlanAdmin;

    public interface IMealPlanService
    {
        Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync();
        Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync();

        /// <summary>
        /// Adds a meal plan to the database or throws an exception
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns>mealPlan ID</returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<string> AddAsync(string userId, MealPlanAddFormModel model);
        Task EditAsync(string userId, MealPlanEditFormModel model);
        Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId);
        Task<int> MineCountAsync(string userId);
        Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id);
        Task<MealPlanEditFormModel> GetForEditByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<int> AllActiveCountAsync();

        /// <summary>
        /// Deletes the meals and mealplan, including them
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteById(string id);

        /// <summary>
        /// Deletes all meal plans and related meals, by a certain user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteAllByUserIdAsync(string userId);
    }
}
