namespace CookTheWeek.Web.ViewModels.MealPlan
{
    using System.ComponentModel.DataAnnotations;

    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;

    using static Common.EntityValidationConstants.MealPlan;

    public class MealPlanEditFormModel : IMealPlanFormModel
    {
        public MealPlanEditFormModel()
        {
            this.Meals = new List<MealAddFormModel>();
        }

        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public IList<MealAddFormModel> Meals { get; set; }


    }
    
}
