namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;
    
    using CustomValidationAttributes;
    using static Common.EntityValidationConstants.RecipeIngredient;
    public class RecipeIngredientFormModel
    {
        
        public RecipeIngredientFormModel()
        {
            this.Measures = new HashSet<RecipeIngredientSelectMeasureViewModel>();
            this.Specifications = new HashSet<RecipeIngredientSelectSpecificationViewModel>();
        }
        
        [Required(ErrorMessage = RecipeIngredientNameRequiredErrorMessage)]
        [StringLength(RecipeIngredientNameMaxLength, MinimumLength = RecipeIngredientNameMinLength, ErrorMessage = RecipeIngredientNameRangeErrorMessage)]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = QtyRequiredErrorMessage)]
        [ValidateQty]
        public RecipeIngredientQtyFormModel Qty { get; set; } = null!;

        [Required(ErrorMessage = MeasureRequiredErrorMessage)]
        [Display(Name = "Unit")]
        public int? MeasureId { get; set; }

        [Display(Name = "Note")]
        public int? SpecificationId { get; set; }
        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
