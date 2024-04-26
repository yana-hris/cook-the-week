namespace CookTheWeek.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.Admin.MealPlanAdmin;
    using Web.ViewModels.MealPlan;

    public interface IMealPlanService
    {
        Task AddAsync(string userId, MealPlanAddFormModel model);
        Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync();
        Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync();
        Task<int> AllActiveCountAsync();
    }
}
