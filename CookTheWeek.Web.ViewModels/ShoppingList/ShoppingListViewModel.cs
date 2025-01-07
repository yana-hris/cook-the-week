namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    public class ShoppingListViewModel
    {
        public ShoppingListViewModel()
        {
            this.ShopItemsByCategories = new List<ISupplyItemListModel<IngredientItemViewModel>>();
        }
        public string MealPlanId { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public IEnumerable<ISupplyItemListModel<IngredientItemViewModel>> ShopItemsByCategories { get; set; } = null!;
    }
}
