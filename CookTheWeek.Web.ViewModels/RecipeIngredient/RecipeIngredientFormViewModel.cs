namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    using ValidationAttributes;
    public class RecipeIngredientFormViewModel
    {
        public RecipeIngredientFormViewModel()
        {
            this.Measures = new HashSet<RecipeIngredientSelectMeasureViewModel>();
            this.Specifications = new HashSet<RecipeIngredientSelectSpecificationViewModel>();
        }
        
        [Required(ErrorMessage = "Please enter ingredient.")]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required]
        [ValidateQty(ErrorMessage = "Invalid qty.")]
        public RecipeIngredientQtyFormModel Qty { get; set; }

        [Required]
        [Display(Name = "Unit")]
        [Range(1, int.MaxValue, ErrorMessage="Please choose a unit.")]
        public int MeasureId { get; set; }

        [Display(Name = "Note")]
        public int? SpecificationId { get; set; }
        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
