namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Meal;
    using System.Threading.Tasks;

    public interface IMealService
    {
        Task<MealDetailsViewModel> Details(int id);
    }
}
