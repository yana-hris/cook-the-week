namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using static CookTheWeek.Common.EntityValidationConstants.Recipe;

    public class RecipeFormViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; set; } = null!;

        [Required]
        [Range(ServingsMinValue, ServingsMaxValue)]
        public int Servings { get; set; }

        [Required]
        [Range(CookingTimeMinValue, CookingTimeMaxValue)]
        public int CookingTimeMinutes { get; set; }

        [Required]
        [Url]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public int RecipeCategoryId { get; set; }

        public ICollection<RecipeCategorySelectViewModel>? Categories { get; set; }
    }
}
