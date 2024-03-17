namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{    
    public class RecipeIngredientDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public decimal Qty { get; set; }
        public string Measure { get; set; } = null!;
        public string? Specification { get; set; }
    }
}
