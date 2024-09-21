namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ProductViewModel
    {
        public string Name { get; set; } = null!;
        public string Qty { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public string? Specification { get; set; } 
    }
}
