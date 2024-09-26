namespace CookTheWeek.Web.ViewModels.Admin.CategoryAdmin
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;

    using static Common.EntityValidationConstants.Category;

    public class IngredientCategoryAddFormModel : ICategoryFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Ingredient Category Name should be between {2} and {1} characters long")]
        public string Name { get; set; } = null!;
    }
}
