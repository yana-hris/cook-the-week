namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System;

    using CookTheWeek.Data.Models.Interfaces;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
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
        /// Validates if a user with the given email or username already exists and returns a Validation result, with a collection of all model errors (dictionary).
        /// </summary>
        /// <param name="model">The Register form model (user input fields)</param>
        /// <returns>ValidationResult</returns>
        Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model);

        
      
        /// <summary>
        /// A generic method to validate a given TCategory add or edit form model.
        /// Ensures that the category name does not already exist, and that the category to edit exists.
        /// If the category to edit does not exist, an exception is thrown.
        /// </summary>
        /// <typeparam name="TCategory">The type of the category being validated.</typeparam>
        /// <typeparam name="TCategoryFormModel">The form model used for adding or editing the category.</typeparam>
        /// <param name="model">The category form model containing the data to validate.</param>
        /// <param name="categoryRepository">The repository used to access category data.</param>
        /// <returns>A <see cref="ValidationResult"/> containing the validation outcome, including the isValid flag and any errors.</returns>
        /// <exception cref="RecordNotFoundException">Thrown when the category to edit does not exist in the database.</exception>

        Task<ValidationResult> ValidateCategoryAsync<TCategory, TCategoryFormModel>(TCategoryFormModel model,
                                                             ICategoryRepository<TCategory> categoryRepository)
            where TCategory : class, ICategory, new()
            where TCategoryFormModel : ICategoryFormModel;

        /// <summary>
        /// A generic method to determine whether a category can be safely deleted.
        /// Checks if the category exists, and whether it has any dependencies (e.g., related entities).
        /// If the category does not exist, an exception is thrown. If the category has dependencies, deletion is not allowed.
        /// </summary>
        /// <typeparam name="TCategory">The type of the category being checked for deletion.</typeparam>
        /// <typeparam name="TDependency">The dependent entity type that may be linked to the category (e.g., Recipe, Ingredient).</typeparam>
        /// <param name="categoryId">The ID of the category to check for deletion.</param>
        /// <param name="categoryRepository">The repository used to access category data and check dependencies.</param>
        /// <returns>Returns <c>true</c> if the category can be deleted; otherwise, an exception is thrown.</returns>
        /// <exception cref="RecordNotFoundException">Thrown if the category with the specified ID does not exist in the database.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the category has dependencies and cannot be deleted.</exception>

        Task<bool> CanCategoryBeDeletedAsync<TCategory, TDependency>(int categoryId,
                                                ICategoryRepository<TCategory> categoryRepository)
            where TCategory : class, ICategory, new()
        where TDependency : class;

        /// <summary>
        /// Validates if an Ingredient can be deleted by checking if it is included in any recipes or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> CanIngredientBeDeletedAsync(int id);

        /// <summary>
        /// Takes a Service model of a recipe like and validates it. Checks for empty enttries, non-existing recipe or unmacthing userId. Throws exceptions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task ValidateUserLikeForRecipe(FavouriteRecipeServiceModel model);

        
        
    }
}
