namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;

    using ValidationAttributes;

    using static Common.ValidationErrorMessages.RecipeIngredient;
    public class RecipeIngredientFormViewModel
    {
        public RecipeIngredientFormViewModel()
        {
            this.Measures = new HashSet<RecipeIngredientSelectMeasureViewModel>();
            this.Specifications = new HashSet<RecipeIngredientSelectSpecificationViewModel>();
        }
        
        [Required(ErrorMessage = NameErrorMessage)]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required]
        [ValidateQty(ErrorMessage = QtyErrorMessage)]
        public RecipeIngredientQtyFormModel Qty { get; set; } = null!;

        [Required]
        [Display(Name = "Unit")]
        [Range(1, int.MaxValue, ErrorMessage = MeasureErrorMessage)]
        public int MeasureId { get; set; }

        [Display(Name = "Note")]
        public int? SpecificationId { get; set; }
        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
