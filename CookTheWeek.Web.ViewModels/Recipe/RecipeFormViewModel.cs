namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using static CookTheWeek.Common.EntityValidationConstants.Recipe;

    public class RecipeFormViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        [Display(Name = "How to Cook")]
        public string Instructions { get; set; } = null!;

        [Required]
        [Range(ServingsMinValue, ServingsMaxValue)]
        public int Servings { get; set; }

        [Required]
        [Display(Name = "Minutes")]
        [Range(CookingTimeMinValue, CookingTimeMaxValue)]
        public int CookingTimeMinutes { get; set; }

        [Required]
        [Url]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Display(Name = "Meal Type")]
        public int RecipeCategoryId { get; set; }
        public RecipeIngredientFormViewModel? Ingredient { get; set; }
        public ICollection<RecipeCategorySelectViewModel>? Categories { get; set; }
        public IDictionary<int, int>? ServingsOptions { get; set; }
        public ICollection<RecipeIngredientFormViewModel>? Ingredients { get; set; }


    }
}
