namespace CookTheWeek.Services.Data.Models.MealPlan
{
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealServiceModel
    {
        [Required]
        [StringLength(GuidLength)]
        public string RecipeId { get; set; } = null!;
    }
}
