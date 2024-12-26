namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class ShoppingListViewModel
    {
        public ShoppingListViewModel()
        {
            this.ShopItemsByCategories = new List<ISupplyItemListModel<ShopItemViewModel>>();
        }
        public string MealPlanId { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public IEnumerable<ISupplyItemListModel<ShopItemViewModel>> ShopItemsByCategories { get; set; } = null!;
    }
}
