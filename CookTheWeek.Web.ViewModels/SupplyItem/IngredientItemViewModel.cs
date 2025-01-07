namespace CookTheWeek.Web.ViewModels.SupplyItem
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class IngredientItemViewModel : ISupplyItemModel
    {
        public string Name { get; set; } = null!;
        public string Qty { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public string? Note { get; set; }

        public List<ISupplyItemModel>? ChildItems { get; set; }
    }
}
