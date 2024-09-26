namespace CookTheWeek.Web.ViewModels.Admin.CategoryAdmin
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.RecipeCategory;

    public class RecipeCategoryEditFormModel : ICategoryFormModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Recipe Category Name should be between {2} and {1} characters long")]
        [Display(Name = "Recipe Category Name")]
        public string Name { get; set; } = null!;
    }
}
