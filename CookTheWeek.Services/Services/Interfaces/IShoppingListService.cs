namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using System.Threading.Tasks;

    public interface IShoppingListService
    {
        Task<ShoppingListViewModel> TryGetShoppingListDataByMealPlanIdAsync(string id);
    }
}
