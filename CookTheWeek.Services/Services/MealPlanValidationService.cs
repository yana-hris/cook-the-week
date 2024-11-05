namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.EntityValidationConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealPlanValidationService : IMealPlanValidationService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<MealPlanValidationService> logger;
        private readonly IRecipeValidationService recipeValidator;

        private readonly Guid userId;
        private readonly bool isAdmin;
        public MealPlanValidationService(ILogger<MealPlanValidationService> logger,
            IRecipeValidationService recipeValidator,
            IUserRepository userRepository, IUserContext userContext)
        {
            this.userRepository = userRepository;
            this.recipeValidator = recipeValidator;
            this.logger = logger;

            userId = userContext.UserId;
            isAdmin = userContext.IsAdmin;
        }

        /// <inheritdoc/>
        public async Task<MealPlanServiceModel> CleanseAndValidateServiceModelAsync(MealPlanServiceModel serviceModel)
        {

            if (string.IsNullOrEmpty(serviceModel.UserId))
            {
                logger.LogError($"Meal plan model creation from service model failed. The received user ID is null.");
                throw new ArgumentNullException(ArgumentNullExceptionMessages.UserIdNullExceptionMessage);
            }

            if (Guid.TryParse(serviceModel.UserId, out Guid serviceUserId))
            {
                var user = await userRepository.ExistsByIdAsync(serviceUserId);

                // Validate userId exists in the database;
                if (!user)
                {
                    logger.LogError($"Meal plan model creation from service model failed. User with id {serviceUserId} does not exist.");
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.MealPlanModelUnsuccessfullyCreatedExceptionMessage);
                }

                // Validate the currently logged in user is the same

                if (userId != default && serviceUserId != userId)
                {
                    logger.LogError($"$Meal plan model creation from service model failed. User Id from service model {serviceUserId} is different form currenly logged in user: {userId}.");
                    throw new UnauthorizedUserException(UnauthorizedExceptionMessages.MealPlanCreationAuthorizationExceptionMessage);
                }
            }
            
            // Validate recipe id`s are valid guids
            var meals = serviceModel.Meals;

            foreach (var meal in meals)
            {
                if (!Guid.TryParse(meal.RecipeId, out Guid validGuid))
                {
                    logger.LogWarning($"Meal plan model creation non-fatal error. Received recipe Id {meal.RecipeId} is not a valid Guid and will be removed.");
                    meals.Remove(meal);
                }

                try
                {
                    await recipeValidator.ValidateRecipeExistsAsync(validGuid);
                }
                catch (RecordNotFoundException)
                {
                    logger.LogWarning($"Meal plan model creation non-fatal error. Received recipe Id {meal.RecipeId} does not exist in the database and will be removed.");
                    meals.Remove(meal);
                }
            }

            if (meals.Count == 0)
            {
                logger.LogError($"Meal plan model creation from service model failed. The received Meals array is null or empty.");
                throw new ArgumentNullException(ArgumentNullExceptionMessages.MealsArrayNullExceptionMessage);
            }

            return serviceModel;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateMealPlanFormModelAsync(IMealPlanFormModel model)
        {
            var result = new ValidationResult();

            var meals = model.Meals;

            if (meals.Count == 0)
            {
                logger.LogError($"Meal plan form model add/edit failed. The Meals collection is mepty.");
                AddValidationError(result, string.Empty, MealPlanValidation.MealsRequiredErrorMessage);
            }

            for (int i = 0; i < meals.Count; i++)
            {
                var meal = meals.ElementAt(i);

                try
                {
                    await recipeValidator.ValidateRecipeExistsAsync(meal.RecipeId);
                }
                catch(ArgumentNullException) 
                {
                    logger.LogError($"Meal plan form model add/edit failed. Received null recipeId for meal at index {i}.");
                    throw;
                }
                catch (RecordNotFoundException)
                {
                    logger.LogError($"Meal plan form model add/edit failed. Recipe with id {meal.RecipeId} for meal at index {i} does not exist.");

                    if (model is MealPlanEditFormModel editModel)
                    {
                        logger.LogError($"Recipe with id {meal.RecipeId} is invalid.");
                        AddValidationError(result, $"Meals[{i}].{nameof(meal.RecipeId)}", RecipeValidation.InvalidRecipeIdErrorMessage); // to show the error in the UI
                    }
                    else
                    {
                        throw; // process the error in add controller action
                    }
                }

                if (!ValidateMealDates(meal, meals.First().SelectDates))
                {
                    logger.LogError($"Meal plan form model add/edit failed. Invalid meal date: {meal.Date} for meal with recipeId {meal.RecipeId}");
                    AddValidationError(result, $"Meals[{i}].{nameof(meal.Date)}", MealValidation.DateRangeErrorMessage);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public void ValidateUserIsMealPlanOwner(Guid ownerId)
        {
            if (userId == default)
            {
                logger.LogError($"Unauthorized attempt of a user to access resource. User not logged in or userId is null.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            if (userId != ownerId && !isAdmin)
            {
                logger.LogError($"User with id {userId} is not the resource owner.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.MealplanEditAuthorizationExceptionMessage);
            }
        }

        /// <inheritdoc/>
        public bool ValidateIfRecipesWereDeleted(MealPlanAddFormModel copiedModel)
        {
            if (copiedModel.Meals.Any(m => m.RecipeId == DeletedRecipeId))
            {
                logger.LogWarning($"Non-fatal error: Meal Plan copy model contains deleted recipeIds.");
                return true;
            }
            return false;
        }

        // HELPER METHODS:

        /// <summary>
        /// Checks if a meal`s selected date is valid (being present in the select-dates and if can be parsed correctly).
        /// </summary>
        /// <param name="meal">the meal form model to check for validation errors</param>
        /// <returns>true or false</returns>
        private static bool ValidateMealDates(MealFormModel meal, string[] selectDates)
        {
            if (!selectDates.Contains(meal.Date))
            {
                return false;
            }

            if (!DateTime.TryParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Set a validation error with a message and make validation result false. If the validation error key alreday exists, the message will not be overwritten
        /// </summary>
        /// <param name="result">The overall validation result</param>
        /// <param name="key">The validation error key</param>
        /// <param name="errorMessage">The validation error message</param>
        private static void AddValidationError(ValidationResult result, string key, string errorMessage)
        {
            result.IsValid = false;
            if (!result.Errors.ContainsKey(key))
            {
                result.Errors.Add(key, errorMessage);
            }
        }


        
        
    }
}
