namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System;

    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    public interface IValidationService
    {
        /// <summary>
        /// Custom validation for recipes upon adding and editing Recipe: checks if the selected category and nested recipe ingredients are valid and exist in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation Result</returns>
        /// <remarks>May throw RecordNotFoundException message upon GetById (ingredient)</remarks>
        Task<ValidationResult> ValidateRecipeWithIngredientsAsync(IRecipeFormModel model);

        /// <summary>
        /// Checks if an ingredient is valid (has the same id and name in the database)
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        /// <remarks>May throw RecordNotFoundException if the ingredient does not exist</remarks>
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

        /// <summary>
        /// A generic method to validate if a given TCategory is not already existent in the database, which can lead to data duplication (by name)
        /// </summary>
        /// <typeparam name="TCategory">The type of category to check for (TCategory)</typeparam>
        /// <typeparam name="TCategoryAddFormModel">The form model for adding or editing a category</typeparam>
        /// <param name="model">The model</param>
        /// <returns>Validation Result, having isValid and Errors properties</returns>
        Task<ValidationResult> ValidateCategoryNameAsync<TCategoryFormModel>(TCategoryFormModel model,
                                                              Func<string, Task<int?>> getCategoryIdByNameFunc,
                                                              Func<int, Task<bool>> categoryExistsByIdFunc = null)
            where TCategoryFormModel : ICategoryFormModel;
    }
}
