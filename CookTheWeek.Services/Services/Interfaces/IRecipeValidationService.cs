namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;

    public interface IRecipeValidationService
    {
        /// <summary>
        /// Custom validation for recipes upon adding and editing Recipe: checks if the selected category and nested recipe ingredients are valid and exist in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation Result</returns>
        /// <remarks>Does not throw exceptions, returns the validation result and logs errors.</remarks>
        Task<ValidationResult> ValidateRecipeFormModelAsync(IRecipeFormModel model);


        /// <summary>
        /// Validates if a Recipe can be deleted by checking if it is included in any active MealPlans.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> CanRecipeBeDeletedAsync(Guid id);

        /// <summary>
        /// Validates if a user has the rights to access a given recipe for edit or delete by the Recipe Owner ID
        /// </summary>
        /// <param name="ownerId">the resource OwnerId</param>
        /// <exception cref="UnauthorizedUserException"></exception>
        void ValidateUserIsRecipeOwner(Guid ownerId);

        /// <summary>
        /// Validates the recipe exists in the database by a given ID
        /// Throws exceptions.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <remarks>Throws exception in case the recipe ID is null or does not exist</remarks>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RecordNotFoundException"></exception>
        Task ValidateRecipeExistsAsync(Guid recipeId);
    }
}
