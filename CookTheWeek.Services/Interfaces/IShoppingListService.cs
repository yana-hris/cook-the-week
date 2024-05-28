namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using System.Threading.Tasks;

    public interface IShoppingListService
    {
        Task<ShoppingListViewModel> GetByMealPlanIdAsync(string id);
    }
}
