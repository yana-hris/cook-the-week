namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.MealPlan;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMealPlanService
    {
        Task AddAsync(string userId, MealPlanAddFormModel model);
        Task<ICollection<MealPlanAllViewModel>> AllActiveAsync();
        Task<int> AllActiveCountAsync();
    }
}
