namespace CookTheWeek.Web.ViewModels.Meal
{
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class MealIngredientDetailsViewModel : ISupplyItemModel
    {
        public string Name { get; set; } = null!;
        public string Qty { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public string? Specification { get; set; }
    }
}
