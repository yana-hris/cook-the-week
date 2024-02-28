namespace CookTheWeek.Web.ViewModels.Ingredient
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using static Common.EntityValidationConstants.Ingredient;

    public class IngredientFormViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int IngredientCategoryId { get; set; }

        public ICollection<IngredientCategorySelectViewModel>? IngredientCategories { get; set; }


    }
}
