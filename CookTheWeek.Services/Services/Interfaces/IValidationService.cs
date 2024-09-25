namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    public interface IValidationService
    {
        /// <summary>
        /// Custom validation for recipes upon adding and editing Recipe: checks if the selected category, ingredient, measure and specification exist in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateRecipeAsync(IRecipeFormModel model);

        /// <summary>
        /// Checks if an ingredient exists by name in the database returns a flag
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model);

        /// <summary>
        /// Validates if a user with the given email or username already exists and returns a Validation result, with a collection of all model errors (dictionary).
        /// </summary>
        /// <param name="model">The Register form model (user input fields)</param>
        /// <returns>ValidationResult</returns>
        Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model);

        /// <summary>
        /// Validates the service model data before creating a meal plan Add form model
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ValidationResult> ValidateMealPlanServiceModelAsync(MealPlanServiceModel serviceModel);

        /// <summary>
        /// Validates if the ids of a meal`s collection are valid recipes
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateMealPlanEditFormModelAsync(MealPlanEditFormModel model);

    }
}
