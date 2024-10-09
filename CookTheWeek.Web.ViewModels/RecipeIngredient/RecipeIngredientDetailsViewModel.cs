namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class RecipeIngredientDetailsViewModel : ISupplyItemModel
    {
        public string Name { get; set; } = null!;
        public string Qty { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public string? Specification { get; set; }
    }
}
