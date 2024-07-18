namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static Common.EntityValidationConstants.Recipe;

    public class RecipeEditFormModel
    {
        public RecipeEditFormModel()
        {
            this.Steps = new List<StepFormModel>();
            this.RecipeIngredients = new List<RecipeIngredientFormModel>();
        }
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleMinLengthErrorMessage)]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionRangeErrorMessage)]
        public string? Description { get; set; }        

        [Required(ErrorMessage = ServingsRequiredErrorMessage)]
        [Range(ServingsMinValue, ServingsMaxValue, ErrorMessage = ServingsRangeErrorMessage)]
        [Display(Name = "Serves")]
        public int Servings { get; set; }

        [Required(ErrorMessage = CookingTimeRequiredErrorMessage)]
        [Display(Name = "Ready for")]
        [Range(CookingTimeMinValue, CookingTimeMaxValue, ErrorMessage = CookingTimeRangeErrorMessage)]
        public int CookingTimeMinutes { get; set; }

        [Required(ErrorMessage = ImageRequiredErrorMessage)]
        [RegularExpression(UrlPattern, ErrorMessage = ImageInvalidErrorMessage)]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength, ErrorMessage = ImageRangeErrorMessage)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = RecipeCategoryIdRequiredErrorMessage)]
        [Display(Name = "Meal Type")]
        public int RecipeCategoryId { get; set; }
        
        [Display(Name = "Cooking Steps")]
        public List<StepFormModel> Steps { get; set; } = null!;
        public List<RecipeIngredientFormModel> RecipeIngredients { get; set; } = null!;

        public ICollection<int>? ServingsOptions { get; set; } = null!;

        public ICollection<RecipeCategorySelectViewModel>? Categories { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Steps == null || Steps.Count == 0)
            {
                yield return new ValidationResult(StepsRequiredErrorMessage, new[] { nameof(Steps) });
            }

            if (RecipeIngredients == null || RecipeIngredients.Count == 0)
            {
                yield return new ValidationResult(IngredientsRequiredErrorMessage, new[] { nameof(RecipeIngredients) });
            }
        }
    }
}
