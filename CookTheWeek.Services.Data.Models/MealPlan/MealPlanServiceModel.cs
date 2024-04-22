namespace CookTheWeek.Services.Data.Models.MealPlan
{
    using System.ComponentModel.DataAnnotations;

    public class MealPlanServiceModel
    {
        public MealPlanServiceModel()
        {
            this.Meals = new HashSet<MealServiceModel>();
        }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public ICollection<MealServiceModel> Meals { get; set; }
    }
}
