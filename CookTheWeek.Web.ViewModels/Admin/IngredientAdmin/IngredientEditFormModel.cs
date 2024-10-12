namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.RecipeIngredientValidation;

    public class IngredientEditFormModel : IIngredientEditFormModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(RecipeIngredientNameMaxLength, MinimumLength = RecipeIngredientNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<IngredientCategorySelectViewModel> Categories { get; set; } = new List<IngredientCategorySelectViewModel>();
    }
}
