namespace CookTheWeek.Web.ViewModels.Admin.IngredientAdmin
{
    using System.ComponentModel.DataAnnotations;
    using CookTheWeek.Web.ViewModels.Category;
    using static Common.EntityValidationConstants.Ingredient;

    public class IngredientAddFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public ICollection<IngredientCategorySelectViewModel>? IngredientCategories { get; set; }


    }
}
