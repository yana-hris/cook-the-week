namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;
    using CookTheWeek.Web.ViewModels.Category;
    using static Common.EntityValidationConstants.RecipeIngredient;

    public class IngredientEditFormModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(RecipeIngredientNameMaxLength, MinimumLength = RecipeIngredientNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<IngredientCategorySelectViewModel>? Categories { get; set; }
    }
}
