namespace CookTheWeek.Web.ViewModels
{
    using CookTheWeek.Web.ViewModels.Interfaces;
    
    public class ShopItemViewModel : ISupplyItemModel
    {
        public string Name { get; set; } = null!;
        public string Qty { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public string? Specification { get; set; } 
    }
}
