namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ShoppingListViewModel
    {
        public ShoppingListViewModel()
        {
            this.ProductsByCategories = new List<ProductListViewModel>();
        }
        public string MealPlanId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public ICollection<ProductListViewModel> ProductsByCategories { get; set; } = null!;
    }
}
