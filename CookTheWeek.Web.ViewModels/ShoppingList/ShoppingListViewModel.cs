namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ShoppingListViewModel
    {
        public ShoppingListViewModel()
        {
            this.ProductsByCategories = new List<ProductListViewModel>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public ICollection<ProductListViewModel> ProductsByCategories { get; set; }
    }
}
