namespace CookTheWeek.Web.ViewModels.Meal
{
    using System.ComponentModel.DataAnnotations;
    
    using static Common.EntityValidationConstants.MealValidation;
    using static Common.EntityValidationConstants.RecipeValidation;

    public class MealFormModel
    {
        public MealFormModel()
        {
            this.SelectDates = new string[7];
            this.SelectServingOptions = ServingsOptions;
        }

        public int? Id { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = MealServingsRequiredErrorMessage)]
        [Range(MinServingSize, MaxServingSize, ErrorMessage = MealServingsRangeErrorMessage)]
        public int Servings { get; set; }

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
