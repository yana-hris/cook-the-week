namespace CookTheWeek.Services.Data.Models.RecipeIngredient
{
    public class RecipeIngredientServiceModel
    {
        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        public decimal Qty { get; set; }

        public int MeasureId { get; set; }

        public int? SpecificationId { get; set; }
    }
}
