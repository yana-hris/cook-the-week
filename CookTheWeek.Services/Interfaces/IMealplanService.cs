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
        Task<string> AddAsync(string userId, MealPlanAddFormModel model);
        Task EditAsync(string userId, MealPlanAddFormModel model);
        Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId);
        Task<MealPlanViewModel> GetByIdAsync(string id);
        Task<MealPlanAddFormModel> GetForEditByIdAsync(string id);
        Task<bool> ExistsByIdAsync(string id);
        Task<int> AllActiveCountAsync();

        // Helper methods for Meal Plan Details View
        Task<int> GetMealPlanTotalMinutesForDetailsAsync(string id);
        Task<int> GetIMealPlanIngredientsCountForDetailsAsync(string id);
    }
}
