namespace CookTheWeek.Web.ViewModels.ShoppingList
{
    
    public class ProductServiceModel
    {
        public string Name { get; set; } = null!;
        public decimal Qty { get; set; }
        public int MeasureId { get; set; }
        public int CategoryId { get; set; }
        public int? SpecificationId { get; set; }
    }
}
