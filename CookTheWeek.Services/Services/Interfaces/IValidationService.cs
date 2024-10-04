namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System;

    using CookTheWeek.Data.Models.Interfaces;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    public interface IValidationService
    {
        /// <summary>
        /// Custom validation for Add or Edit Ingredient Form models. Checks if the ingredient is no already added or if the category is valid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation result</returns>
        Task<ValidationResult> ValidateIngredientFormModelAsync(IIngredientFormModel model);

        /// <summary>
        /// Custom validation for recipes upon adding and editing Recipe: checks if the selected category and nested recipe ingredients are valid and exist in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation Result</returns>
        /// <remarks>Does not throw exceptions, returns the validation result and logs errors.</remarks>
        Task<ValidationResult> ValidateRecipeWithIngredientsAsync(IRecipeFormModel model);

        /// <summary>
        /// Checks if an ingredient is valid (has the same id and name in the database)
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        /// <remarks>May throw RecordNotFoundException if the ingredient does not exist</remarks>
        Task<bool> ValidateIngredientForRecipeIngredientAsync(RecipeIngredientFormModel model);

       
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
       /// Validates any MealPlanFormModel by checking the model type and performing respective validation. 
       /// In case of unexisting recipeId in meal plan model, throws exception. In case of unmatching or missing userId and meal plan ownerId, throws exception.
       /// </summary>
       /// <param name="model"></param>
       /// <returns>Validation Result</returns>
       /// <exception cref="RecordNotFoundException"></exception>
       /// <exception cref="UnauthorizedUserException"></exception>
        Task<ValidationResult> ValidateMealPlanFormModelAsync(IMealPlanFormModel model);

        /// <summary>
        /// A generic method to validate a given TCategory add or edit form model is valid (name does not exist, category for edit exists, etc.).
        /// If the category to edit does not exist, throws an exception.
        /// </summary>
        /// <typeparam name="TCategory">The type of category to check for (TCategory)</typeparam>
        /// <typeparam name="TCategoryAddFormModel">The form model for adding or editing a category</typeparam>
        /// <param name="model">The model</param>
        /// <returns>Validation Result, having isValid and Errors properties</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<ValidationResult> ValidateCategoryAsync<TCategoryFormModel>(TCategoryFormModel model,
                                                              Func<string, Task<int?>> getCategoryIdByNameFunc,
                                                              Func<int, Task<bool>> categoryExistsByIdFunc = null)
            where TCategoryFormModel : ICategoryFormModel;

        /// <summary>
        /// A validation method that checks if a category exists, then if it has any dependencies and if not, returns true. Otherwis, throws an exception.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="getCategoryByIdFunc"></param>
        /// <param name="hasDependenciesFunc"></param>
        /// <returns>true or throws an exception</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        Task<bool> CanCategoryBeDeletedAsync<TCategory>(int categoryId,
                                                Func<int, Task<TCategory?>> getCategoryByIdFunc,
                                                Func<int, Task<bool>> hasDependenciesFunc)
            where TCategory : ICategory;

        /// <summary>
        /// Validates if an Ingredient can be deleted by checking if it is included in any recipes or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> CanIngredientBeDeleted(int id);

        /// <summary>
        /// Takes a Service model of a recipe like and validates it. Checks for empty enttries, non-existing recipe or unmacthing userId. Throws exceptions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task<bool> ValidateUserLikeForRecipe(FavouriteRecipeServiceModel model);

        /// <summary>
        /// Validates if a user has the rights to edit or delete a given mealplan (by id)
        /// </summary>
        /// <param name="ownerId"></param>
        /// <exception cref="UnauthorizedUserException"></exception>
        void ValidateMealPlanUserAuthorizationAsync(Guid ownerId);
    }
}
