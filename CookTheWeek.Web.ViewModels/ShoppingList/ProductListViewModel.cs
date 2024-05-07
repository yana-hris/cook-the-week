namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ProductListViewModel
    {
        public string Title { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }
    }
}
