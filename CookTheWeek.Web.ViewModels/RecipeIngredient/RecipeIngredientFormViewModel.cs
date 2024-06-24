namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using CookTheWeek.Web.ViewModels.CustomValidationAttributes;
    using System.ComponentModel.DataAnnotations;

    using ValidationAttributes;

    using static Common.EntityValidationConstants.RecipeIngredient;
    public class RecipeIngredientFormViewModel
    {
        
        public RecipeIngredientFormViewModel()
        {
            this.Measures = new HashSet<RecipeIngredientSelectMeasureViewModel>();
            this.Specifications = new HashSet<RecipeIngredientSelectSpecificationViewModel>();
        }
        
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameRangeErrorMessage)]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = QtyRequiredErrorMessage)]
        [ValidateQty]
        public RecipeIngredientQtyFormModel Qty { get; set; } = null!;

        [Required(ErrorMessage = MeasureRequiredErrorMessage)]
        [Display(Name = "Unit")]
        [ValidateRangeBasedOnCollectionSize(nameof(Measures), MeasureRangeErrorMessage)]
        public int MeasureId { get; set; }

        [Display(Name = "Note")]
        [ValidateRangeBasedOnCollectionSize(nameof(Specifications), SpecificationRangeErrorMessage)]
        public int? SpecificationId { get; set; }
        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
