namespace CookTheWeek.Web.ViewModels.Ingredient
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;

    using static Common.EntityValidationConstants.Ingredient;

    public class IngredientEditFormModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<IngredientCategorySelectViewModel>? Categories { get; set; }
    }
}
