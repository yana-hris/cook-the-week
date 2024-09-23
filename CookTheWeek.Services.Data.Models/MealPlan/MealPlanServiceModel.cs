namespace CookTheWeek.Services.Data.Models.MealPlan
{
    using System.ComponentModel.DataAnnotations;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealPlanServiceModel
    {
        public MealPlanServiceModel()
        {
            this.Meals = new HashSet<MealServiceModel>();
        }

        [Required]
        [StringLength(GuidLength)]
        public string UserId { get; set; } = null!;
        public ICollection<MealServiceModel> Meals { get; set; }
    }
}
