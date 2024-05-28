namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.Meal;
    using System.Threading.Tasks;

    public interface IMealService
    {
        Task<MealDetailsViewModel> DetailsByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
    }
}
