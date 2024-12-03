namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;
    
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;

    public class RecipeAddFormModel : IRecipeFormModel
    {
        public RecipeAddFormModel()
        {
            this.Steps = new List<StepFormModel>();
            this.RecipeIngredients = new List<RecipeIngredientFormModel>();
            this.Categories = new HashSet<SelectViewModel>();   
            this.DifficultyLevels = new HashSet<SelectViewModel>();
            this.AvailableTags = new HashSet<SelectViewModel>();
            this.SelectedTagIds = new List<int>();
        }

        [Required(ErrorMessage = TitleRequiredErrorMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleMinLengthErrorMessage)]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = null!;


        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionRangeErrorMessage)]
        public string? Description { get; set; }


        [Required(ErrorMessage = ServingsRequiredErrorMessage)] 
        [Range(ServingsMinValue, ServingsMaxValue, ErrorMessage = ServingsRangeErrorMessage)]
        [Display(Name = "Serves")]
        public int? Servings { get; set; }


        [Required(ErrorMessage = CookingTimeRequiredErrorMessage)] 
        [Display(Name = "Ready for")]
        [Range(CookingTimeMinValue, CookingTimeMaxValue, ErrorMessage = CookingTimeRangeErrorMessage)]
        public int? CookingTimeMinutes { get; set; }


        [Required(ErrorMessage = ImageRequiredErrorMessage)]
        [RegularExpression(UrlPattern, ErrorMessage = ImageInvalidErrorMessage)]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength, ErrorMessage = ImageRangeErrorMessage)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;


        [Required(ErrorMessage = RecipeCategoryIdRequiredErrorMessage)]
        [Display(Name = "Meal Type")]
        public int? RecipeCategoryId { get; set; }


        [Display(Name = "Difficulty Level")]
        [Range(DifficultyLevelMinValue, DifficultyLevelMaxValue, ErrorMessage = DifficultyLevelErrorMessage)]
        public int? DifficultyLevelId { get; set; }


        [Display(Name = "Cooking Steps")]
        public List<StepFormModel> Steps { get; set; } = null!;


        [Display(Name = "Recipes Ingredients")]
        public List<RecipeIngredientFormModel> RecipeIngredients { get; set; } = null!;


        [Display(Name = "Recipe`s selected Tag IDs")]
        public List<int> SelectedTagIds { get; set; }


        // Select menu collections
        public ICollection<int>? ServingsOptions { get; set; } = null!;
        public ICollection<SelectViewModel>? Categories { get; set; } 
        public ICollection<SelectViewModel>? DifficultyLevels { get; set; }
        public ICollection<SelectViewModel>? AvailableTags { get; set; }



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
