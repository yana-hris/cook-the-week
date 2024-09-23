﻿namespace CookTheWeek.Web.ViewModels.MealPlan;

using System.ComponentModel.DataAnnotations;

using CookTheWeek.Web.ViewModels.Interfaces;
using CookTheWeek.Web.ViewModels.Meal;

using static Common.EntityValidationConstants.MealPlan;

public class MealPlanAddFormModel : IMealPlanFormModel
{
    public MealPlanAddFormModel()
    {
        this.Meals = new List<MealAddFormModel>();
        this.StartDate = DateTime.Now;  
    }
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = NameRequiredErrorMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthErrorMessage)]
    public string Name { get; set; } = null!;

    [Required]
    public IList<MealAddFormModel> Meals { get; set; } = null!;
    
}
