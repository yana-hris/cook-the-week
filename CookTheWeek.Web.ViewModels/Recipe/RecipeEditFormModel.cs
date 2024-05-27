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
            this.RecipeIngredients = new List<RecipeIngredientFormViewModel>();
        }
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title cannot be shorter than {2} characters")]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }        

        [Required(ErrorMessage = "Required")]
        [Range(ServingsMinValue, ServingsMaxValue, ErrorMessage = @"Servings cannot be < {2} and > {1}")]
        [Display(Name = "Serves")]
        public int Servings { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Minutes")]
        [Range(CookingTimeMinValue, CookingTimeMaxValue, ErrorMessage = "Cooking Time cannot be less than 10 minutes")]
        public int CookingTimeMinutes { get; set; }

        [Required(ErrorMessage = "Required")]
        [Url]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength, ErrorMessage = "Invalid URL link")]
        [Display(Name = "Image URL Link")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Meal Type")]
        public int RecipeCategoryId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Cooking Steps")]

        public List<StepFormModel> Steps { get; set; }

        public List<RecipeIngredientFormViewModel> RecipeIngredients { get; set; } = null!;

        public ICollection<int>? ServingsOptions { get; set; } = null!;

        public ICollection<RecipeCategorySelectViewModel>? Categories { get; set; } = null!;
    }
}
