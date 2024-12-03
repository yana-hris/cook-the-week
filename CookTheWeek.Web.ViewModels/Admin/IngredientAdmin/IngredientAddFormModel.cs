namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;
    
    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.RecipeIngredientValidation;

    public class IngredientAddFormModel : IIngredientFormModel
    {
        public IngredientAddFormModel()
        {
            this.Categories = new HashSet<SelectViewModel>();
        }

        [Required(ErrorMessage = RecipeIngredientNameRequiredErrorMessage)]
        [StringLength(RecipeIngredientNameMaxLength, MinimumLength = RecipeIngredientNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<SelectViewModel> Categories { get; set; }


    }
}
