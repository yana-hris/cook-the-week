namespace CookTheWeek.Services.Data.Models.MealPlan
{
    using System.ComponentModel.DataAnnotations;

    public class MealServiceModel
    {
        [Required]
        public string RecipeId { get; set; } = null!;
    }
}
