﻿namespace CookTheWeek.Services.Data.Interfaces
{
    using CookTheWeek.Services.Data.Vlidation;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IValidationService
    {
        Task<ValidationResult> ValidateRecipeAsync(IRecipeFormModel model);

        Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model);

    }
}
