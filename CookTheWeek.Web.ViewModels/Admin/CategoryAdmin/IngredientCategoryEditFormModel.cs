namespace CookTheWeek.Web.ViewModels.Admin.CategoryAdmin
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.IngredientCategory;
    public class IngredientCategoryEditFormModel : ICategoryFormModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Ingredient Category Name should be between {2} and {1} characters long")]
        [Display(Name = "Ingredient Category Name")]
        public string Name { get; set; } = null!;
    }
}
