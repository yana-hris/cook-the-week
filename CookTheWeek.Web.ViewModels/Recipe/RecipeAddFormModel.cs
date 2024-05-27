namespace CookTheWeek.Web.ViewModels.Recipe
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.Recipe;

    public class RecipeAddFormModel
    {
        public RecipeAddFormModel()
        {
            this.Steps = new List<StepFormModel>();
            this.RecipeIngredients = new List<RecipeIngredientFormViewModel>();
        }

        [Required(ErrorMessage = @"{0} required!")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        [Display(Name = "Recipe Title")]
        public string Title { get; set; } = null!;
        
        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }        

        [Required(ErrorMessage = @"{0} required!")]
        [Range(ServingsMinValue, ServingsMaxValue)]
        public int Servings { get; set; }

        [Required]
        [Display(Name = "Minutes")]
        [Range(CookingTimeMinValue, CookingTimeMaxValue, ErrorMessage = "Please enter a value between {1} and {2} minutes!")]
        public int CookingTimeMinutes { get; set; }

        [Required(ErrorMessage = @"{0} required!")]
        [Url]
        [StringLength(ImageUlrMaxLength, MinimumLength = ImageUlrMinLength)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = @"{0} required!")]
        [Display(Name = "Meal Type")]
        public int RecipeCategoryId { get; set; }

        public List<StepFormModel> Steps { get; set; } = null!;

        public List<RecipeIngredientFormViewModel> RecipeIngredients { get; set; } = null!;

        public ICollection<int>? ServingsOptions { get; set; } = null!;

        public ICollection<RecipeCategorySelectViewModel>? Categories { get; set; } = null!;


    }
}
