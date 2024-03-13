namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeIngredientFormViewModel
    {
        [Required]
        public int IngredientId { get; set; }

        [Required]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }

        [Required]
        [Display(Name = "Measure")]
        public int MeasureId { get; set; }

        [Display(Name = "Options")]
        public int? SpecificationId { get; set; }

        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
