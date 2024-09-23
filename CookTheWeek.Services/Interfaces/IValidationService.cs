﻿namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Vlidation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    public interface IValidationService
    {
        Task<ValidationResult> ValidateRecipeAsync(IRecipeFormModel model);

        Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model);

        Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model);

        Task<ValidationResult> ValidateMealPlanServiceModelAsync(MealPlanServiceModel serviceModel);

    }
}
