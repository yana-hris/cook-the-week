namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;

    public interface IMealPlanValidationService
    {
        /// <summary>
        /// Validates the provided <see cref="MealPlanServiceModel"/> by ensuring the user ID and recipe IDs are valid. 
        /// Removes any invalid or non-existing recipe IDs from the meal plan. Throws exceptions if critical validation fails.
        /// </summary>
        /// <param name="serviceModel">The meal plan service model to validate.</param>
        /// <returns>
        /// Returns the validated <see cref="MealPlanServiceModel"/> with invalid or non-existing recipe IDs removed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the user ID is null or the meals array is null or empty after validation.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the user associated with the service model does not exist in the database.
        /// </exception>
        /// <exception cref="UnauthorizedUserException">
        /// Thrown when the user associated with the service model is different from the currently logged-in user.
        /// </exception>
        /// <remarks>
        /// Non-fatal warnings are logged if invalid or non-existing recipe IDs are found, and they are removed from the meal plan.
        /// </remarks>

        Task<MealPlanServiceModel> CleanseAndValidateServiceModelAsync(MealPlanServiceModel mealPlanServiceModel);

        /// <summary>
        /// Validates MealPlanAddFormModel and MealPlanEditFormModel.
        /// For both models checks if Meal collection is empty, if meals are valid recipes. In case of wrong recipeIds throws RecordNotFoundException.
        /// In case of invalid selected Date (for meal) returns Validation Error.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation Result</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ValidationResult> ValidateMealPlanFormModelAsync(IMealPlanFormModel model);

        /// <summary>
        /// Validates if a user has the rights to access a given resource by the owner ID
        /// </summary>
        /// <param name="mealplanOwnerId">the resource OwnerId</param>
        /// <exception cref="UnauthorizedUserException"></exception>
        void ValidateUserIsMealPlanOwner(Guid mealplanOwnerId);

        
        /// <summary>
        /// Checks if any of the old meal plan`s meals are not referencing already deleted recipes.
        /// </summary>
        /// <param name="copiedModel"></param>
        /// <returns>true or false</returns>
        bool ValidateIfRecipesWereDeleted(MealPlanAddFormModel copiedModel);


    }
}
