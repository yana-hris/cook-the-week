namespace CookTheWeek.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.IngredientCategory;

    public class IngredientCategoryAddFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = @"Ingredient Category Name should be between {2} and {1} characters long")]
        public string Name { get; set; } = null!;
    }
}
