namespace CookTheWeek.Web.ViewModels.MealPlan;

using System.ComponentModel.DataAnnotations;

using CookTheWeek.Web.ViewModels.Meal;

using static Common.EntityValidationConstants.MealPlan;

public class MealPlanAddFormModel
{
    public string? Id { get; set; }
    
    public DateTime? StartDate { get; set; } 

    [Required]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Invalid Meal Plan Name")]
    public string Name { get; set; } = null!;

    [Required]
    public IList<MealAddFormModel> Meals { get; set; } = new List<MealAddFormModel>();
    
}
