namespace CookTheWeek.Web.ViewModels.MealPlan
{
    using CookTheWeek.Web.ViewModels.Meal;
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.MealPlan;

    public class MealPlanAddFormModel
    {
        public MealPlanAddFormModel()
        {
            this.Meals = new HashSet<MealAddFormModel>();
        }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The Meal Plan Name must be between {2} and {1}")]
        public string Name { get; set; } = null!;

        [Required]
        public ICollection<MealAddFormModel> Meals { get; set; }
    }
}
