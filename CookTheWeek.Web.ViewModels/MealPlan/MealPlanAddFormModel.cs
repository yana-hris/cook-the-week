namespace CookTheWeek.Web.ViewModels.MealPlan;

using System.ComponentModel.DataAnnotations;

using CookTheWeek.Web.ViewModels.Meal;

using static Common.EntityValidationConstants.MealPlan;

public class MealPlanAddFormModel
{
    public MealPlanAddFormModel()
    {
        this.Meals = new List<MealAddFormModel>();
    }
    public string? Id { get; set; }
    
    public DateTime? StartDate { get; set; } 

    [Required(ErrorMessage = NameRequiredErrorMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthErrorMessage)]
    public string? Name { get; set; } 

    [Required]
    public IList<MealAddFormModel> Meals { get; set; } = null!;
    
}
