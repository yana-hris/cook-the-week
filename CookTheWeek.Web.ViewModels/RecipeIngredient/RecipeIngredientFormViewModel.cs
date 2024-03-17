namespace CookTheWeek.Web.ViewModels.RecipeIngredient
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidationConstants.RecipeIngredient;
    public class RecipeIngredientFormViewModel
    {
        public RecipeIngredientFormViewModel()
        {
            this.Measures = new HashSet<RecipeIngredientSelectMeasureViewModel>();
            this.Specifications = new HashSet<RecipeIngredientSelectSpecificationViewModel>();
        }
        
        [Required(ErrorMessage = "Please enter ingredient!")]
        [Display(Name = "Ingredient")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Qty")]
        [Range(QtyMinValue, QtyMaxValue , ErrorMessage = "The qty should be between {1} and {2}!")]
        public decimal Qty { get; set; }

        [Required]
        [Display(Name = "Unit")]
        [Range(1, int.MaxValue, ErrorMessage="Please choose a unit!")]
        public int MeasureId { get; set; }

        [Display(Name = "Options")]
        public int? SpecificationId { get; set; }
        public ICollection<RecipeIngredientSelectMeasureViewModel>? Measures { get; set; }

        public ICollection<RecipeIngredientSelectSpecificationViewModel>? Specifications { get; set; }
    }
}
