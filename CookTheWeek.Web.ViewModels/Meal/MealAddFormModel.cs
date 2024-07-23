namespace CookTheWeek.Web.ViewModels.Meal
{
    using System.ComponentModel.DataAnnotations;

    using Common.HelperMethods;
    using static Common.EntityValidationConstants.Meal;
    using static Common.EntityValidationConstants.Recipe;

    public class MealAddFormModel
    {
        public MealAddFormModel()
        {
            this.SelectDates = DateGenerator.GenerateNext7Days();
            this.SelectServingOptions = ServingsOptions;
        }

        public string? Id { get; set; }

        [Required]
        public string RecipeId { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = MealServingsRequiredErrorMessage)]
        [Range(MinServingSize, MaxServingSize, ErrorMessage = MealServingsRangeErrorMessage)]
        public int? Servings { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = DateRequiredErrorMessage)]
        public string Date { get; set; } = null!;

        public int[] SelectServingOptions { get; set; }

        public string[] SelectDates { get; set; }
    }
}
