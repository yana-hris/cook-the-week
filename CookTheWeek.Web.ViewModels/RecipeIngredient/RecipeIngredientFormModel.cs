namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;
    
    using CustomValidationAttributes;

    using static Common.EntityValidationConstants.RecipeIngredientValidation;
    public class RecipeIngredientFormModel
    {
        
        public RecipeIngredientFormModel()
        {
            this.Measures = new HashSet<SelectViewModel>();
            this.Specifications = new HashSet<SelectViewModel>();
        }

        [Required]
        public int? IngredientId { get; set; }

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
        public ICollection<SelectViewModel> Measures { get; set; }

        public ICollection<SelectViewModel> Specifications { get; set; }
    }
}
