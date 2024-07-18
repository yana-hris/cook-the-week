namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            this.Products = new List<ProductViewModel>();
        }
        public string Title { get; set; } = null!;

        public ICollection<ProductViewModel> Products { get; set; } = null!;
    }
}
