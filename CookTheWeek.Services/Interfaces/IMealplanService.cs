namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.MealPlan;
    using System.Threading.Tasks;

    public interface IMealplanService
    {
        Task AddAsync(string userId, MealPlanAddFormModel model);
        Task<int> AllActiveCountAsync();
    }
}
