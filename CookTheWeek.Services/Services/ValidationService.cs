namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Models.Interfaces;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.FavouriteRecipe;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    using static CookTheWeek.Common.EntityValidationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using System.Runtime.ExceptionServices;

    public class ValidationService : IValidationService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IUserRepository userRepository;
        private readonly IMealplanRepository mealplanReposiroty;
        private readonly IIngredientRepository ingredientRepository;
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly ICategoryRepository<RecipeCategory> recipeCategoryRepository;
        private readonly ICategoryRepository<IngredientCategory> ingredientCategoryRepository;
        private readonly ILogger<ValidationService> logger;

        private readonly string? userId;

        public ValidationService(
            IRecipeRepository recipeRepository,
            IUserRepository userRepository,
            IMealplanRepository mealplanReposiroty,
            IIngredientRepository ingredientRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            ICategoryRepository<RecipeCategory> recipeCategoryRepository,
            ICategoryRepository<IngredientCategory> ingredientCategoryRepository,
            IUserContext userContext,
            ILogger<ValidationService> logger)
        {
            this.recipeRepository = recipeRepository;
            this.userRepository = userRepository;
            this.mealplanReposiroty = mealplanReposiroty;
            this.ingredientRepository = ingredientRepository;
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.recipeCategoryRepository = recipeCategoryRepository;
            this.ingredientCategoryRepository = ingredientCategoryRepository;
            this.logger = logger;
            this.userId = userContext.UserId ?? string.Empty;
        }

        // RECIPE:              
       
        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRecipeFormModelAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await recipeCategoryRepository.ExistsByIdAsync(model.RecipeCategoryId!.Value);
            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), RecipeValidation.RecipeCategoryIdInvalidErrorMessage);
            }

            if (!model.RecipeIngredients.Any())
            {
                AddValidationError(result, nameof(model.RecipeIngredients), RecipeValidation.IngredientsRequiredErrorMessage);
            }

            if (!model.Steps.Any())
            {
                AddValidationError(result, nameof(model.Steps), RecipeValidation.StepsRequiredErrorMessage);
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                try
                {
                    bool exists = await ValidateIngredientForRecipeIngredientAsync(ingredient);
                }
                catch (RecordNotFoundException ex)
                {
                    logger.LogError($"Ingredient with name {ingredient.Name} and id {ingredient.Name} does not exist. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                    AddValidationError(result, nameof(ingredient.Name), RecipeIngredientValidation.RecipeIngredientInvalidErrorMessage);
                }

                bool measureExists = await recipeIngredientRepository.MeasureExistsByIdAsync(ingredient.MeasureId!.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), RecipeIngredientValidation.MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await recipeIngredientRepository.SpecificationExistsByIdAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        AddValidationError(result, nameof(ingredient.SpecificationId), RecipeIngredientValidation.SpecificationRangeErrorMessage);

                    }
                }
            }

            return result;
        }



        // INGREDIENT:
        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateIngredientFormModelAsync(IIngredientFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await ingredientCategoryRepository.ExistsByIdAsync(model.CategoryId);

            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.CategoryId), CategoryValidation.CategoryInvalidErrorMessage);
            }

            // Check if an ingredient with the same name already exists
            bool existingByName = await ingredientRepository.ExistsByNameAsync(model.Name);

            if (existingByName)
            {
                int existingIngredientId = await ingredientRepository.GetAllQuery()
                        .Where(i => i.Name.ToLower() == model.Name.ToLower())
                        .Select(i => i.Id)
                        .FirstAsync();

                if (model is IIngredientEditFormModel editModel)
                {
                    if (existingIngredientId != editModel.Id)
                    {
                        AddValidationError(result, nameof(editModel.Name), IngredientValidation.IngredientNameErrorMessage);
                    }
                }
                else
                {
                    // In the add scenario, any matching name is a conflict
                    AddValidationError(result, nameof(model.Name), IngredientValidation.IngredientNameErrorMessage);
                }
            }

            return result;
        }
        

        /// <inheritdoc/>
        public Task<bool> CanIngredientBeDeleted(int id)
        {
            return recipeRepository.GetAllQuery()
                .AnyAsync(r => r.RecipesIngredients.Any(ri => ri.IngredientId == id));
        }



        // USER:
        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model)
        {
            var result = new ValidationResult();

            var userWithEmailExists = await userRepository.GetByEmailAsync(model.Email);
            var userWithUserNameExists = await userRepository.GetByUsernameAsync(model.Username);

            if (userWithEmailExists != null || userWithUserNameExists != null)
            {
                AddValidationError(result, string.Empty, ApplicationUserValidation.AlreadyHaveAccountErrorMessage);
            }

            if (userWithUserNameExists != null)
            {
                AddValidationError(result, nameof(model.Username), ApplicationUserValidation.UsernameAlreadyExistsErrorMessage);
            }

            if (userWithEmailExists != null)
            {
                AddValidationError(result, nameof(model.Email), ApplicationUserValidation.EmailAlreadyExistsErrorMessage);
            }

            return result;

        }



        // MEALPLAN:
        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateMealPlanServiceModelAsync(MealPlanServiceModel serviceModel)
        {
            var result = new ValidationResult();

            string serviceUserId = serviceModel.UserId;
            var user = await userRepository.ExistsByIdAsync(serviceUserId);

            // Validate userId exists in the database;
            if (!user)
            {
                logger.LogError($"Creating MealPlan model failed. User with id {serviceUserId} does not exist.");
                AddValidationError(result, nameof(serviceModel.UserId), ApplicationUserValidation.UserNotFoundErrorMessage);
            }

            // Validate the currently logged in user is the same
            
            if (!string.IsNullOrEmpty(userId) && serviceUserId.ToLower() != userId.ToLower())
            {
                logger.LogError($"$Unauthorized attempt to create mealplan. User Id from Service Model {serviceUserId} is different form currenly logged in user: {userId}.");
                AddValidationError(result, nameof(serviceModel.UserId), ApplicationUserValidation.InvalidUserIdErrorMessage);
            }

            // Validate recipe id`s are valid recipes
            var meals = serviceModel.Meals;

            for (int i = 0; i < meals.Count; i++)
            {
                var meal = serviceModel.Meals.ElementAt(i);

                try
                {
                    await ValidateRecipeExistsAsync(meal.RecipeId);
                }
                catch (Exception ex) when (ex is RecordNotFoundException || ex is ArgumentNullException)
                {
                    string key = $"Meals[{i}].RecipeId"; // sub-entry
                    AddValidationError(result, key, RecipeValidation.InvalidRecipeIdErrorMessage);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public void ValidateUserIsResourceOwnerAsync(Guid ownerId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                logger.LogError($"Unauthorized attempt of a user to access meal plan data.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            if (!GuidHelper.CompareGuidStringWithGuid(userId!, ownerId))
            {
                logger.LogError($"User with id {userId} is not the resource owner. Authentication not allowed.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.MealplanEditAuthorizationExceptionMessage);
            }
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateMealPlanMealsAsync(IMealPlanFormModel model)
        {
            var result = new ValidationResult();

            if (!model.Meals.Any())
            {
                AddValidationError(result, string.Empty, MealPlanValidation.MealsRequiredErrorMessage);
            }

            // Validate Meals
            await ValidateMealsAsync(model.Meals, result);
            
            return result;
        }

        
        // CATEGORY:
        /// <inheritdoc/>       
        public async Task<ValidationResult> ValidateCategoryAsync<TCategory, TCategoryFormModel>(TCategoryFormModel model,
                                                   ICategoryRepository<TCategory> categoryRepository)
            where TCategory : class, ICategory, new()
            where TCategoryFormModel : ICategoryFormModel
        {
            var result = new ValidationResult();

            // Editing scenario
            if (model is ICategoryEditFormModel editModel)
            {
                bool categoryExists = await categoryRepository.ExistsByIdAsync(editModel.Id); 

                if (!categoryExists)
                {
                    logger.LogError($"Record not found: A category with ID {editModel.Id} was not found.");
                    throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
                }

                // Check if the category name exists in another category
                int? existingCategoryId = await categoryRepository.GetIdByNameAsync(editModel.Name);

                // If a category with the same name exists but it is not the current one being edited
                if (existingCategoryId.HasValue && existingCategoryId.Value != editModel.Id)
                {
                    AddValidationError(result, nameof(editModel.Name), CategoryValidation.CategoryExistsErrorMessage);
                }
            }
            else
            {
                // Adding scenario: Check if a category with the same name already exists
                int? existingCategoryId = await categoryRepository.GetIdByNameAsync(model.Name);
                if (existingCategoryId.HasValue)
                {
                    AddValidationError(result, nameof(model.Name), CategoryValidation.CategoryExistsErrorMessage);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> CanCategoryBeDeletedAsync<TCategory, TDependency>(
        int categoryId,
        ICategoryRepository<TCategory> categoryRepository)
        where TCategory : class, ICategory, new()
        where TDependency : class
        {
            // Step 1: Check if the category exists
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                logger.LogError($"Category of type {typeof(TCategory).Name} with ID {categoryId} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            // Step 2: Check if the category has dependencies using the generic HasDependenciesAsync method
            bool hasDependencies = await categoryRepository.HasDependenciesAsync<TDependency>(categoryId);

            if (hasDependencies)
            {
                logger.LogError($"Category of type {typeof(TCategory).Name} with ID {categoryId} has dependencies and cannot be deleted.");
                throw new InvalidOperationException($"Category of type {typeof(TCategory).Name} cannot be deleted because it has dependencies.");
            }

            return true;  // Category can be deleted
        }




        // TODO: check!
        // FAVOURITE RECIPE (LIKES)
        /// <inheritdoc/>   
        public async Task ValidateUserLikeForRecipe(FavouriteRecipeServiceModel model)
        {
            string serviceUserId = model.UserId;
            string recipeId = model.RecipeId;

            await ValidateRecipeExistsAsync(recipeId);

            // Validate user authorization
            if (!string.IsNullOrEmpty(userId) &&
                !string.IsNullOrEmpty(serviceUserId) &&
                !GuidHelper.CompareTwoGuidStrings(userId, serviceUserId))
            {
                logger.LogError($"Unauthorized access attempt: User {userId} attempted to like/unlike recipe {recipeId} without necessary permissions.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

        }

        





        // PRIVATE METHODS:

        /// <summary>
        /// A helper method that checks if a recipe-ingredient is a valid and existing ingredient in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false or throws RecordNotFound</returns>
        /// <remarks>May throw RecordNotFoundException if the ingredient does not exist</remarks>
        private async Task<bool> ValidateIngredientForRecipeIngredientAsync(RecipeIngredientFormModel model)
        {
            Ingredient ingredient = await ingredientRepository.GetByIdAsync(model.IngredientId!.Value);
            return ingredient.Id == model.IngredientId && ingredient.Name.ToLower() == model.Name.ToLower();
        }


        /// <summary>
        /// A helper method which validates the meals in a mealplan form model. Takes the mealplan result and processes it or throws an exception
        /// </summary>
        /// <param name="meals"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task ValidateMealsAsync(ICollection<MealAddFormModel> meals, ValidationResult result)
        {

            for (int i = 0; i < meals.Count; i++)
            {
                var meal = meals.ElementAt(i);

                try
                {
                    await ValidateRecipeExistsAsync(meal.RecipeId);
                }
                catch (Exception ex) when (ex is RecordNotFoundException || ex is ArgumentNullException)
                {
                    logger.LogError($"Meal plan model invalid: recipe with id {meal.RecipeId} for meal at index {i} not found or is null.");
                    throw;
                }
                
                if (!ValidateMealDates(meal))
                {
                    logger.LogError($"Invalid meal date: {meal.Date} for meal with recipeId {meal.RecipeId}");
                    AddValidationError(result, nameof(meal.Date), MealValidation.DateRangeErrorMessage);
                }
            }
        }

        /// <summary>
        /// A helper method which verifies the recipeId is not null or if exists in the database. 
        /// Throws exceptions.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RecordNotFoundException"></exception>
        private async Task ValidateRecipeExistsAsync(string recipeId)
        {
            // Validate if recipeId is provided
            if (string.IsNullOrEmpty(recipeId))
            {
                logger.LogError($"Validation failed: RecipeId is null or empty.");
                throw new ArgumentNullException(ArgumentNullExceptionMessages.RecipeNullExceptionMessage);
            }

            // Check if the recipe exists
            bool recipeExists = await recipeRepository.ExistsByIdAsync(recipeId);
            if (!recipeExists)
            {
                logger.LogError($"Validation failed: Recipe with id {recipeId} does not exist.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }
        }

        /// <summary>
        /// Checks if a meal`s selected date is valid (being present in the select-dates and if can be parsed correctly).
        /// </summary>
        /// <param name="meal">the meal form model to check for validation errors</param>
        /// <returns>true or false</returns>
        private bool ValidateMealDates(MealAddFormModel meal)
        {
            if (!meal.SelectDates.Contains(meal.Date))
            {
                return false;
            }

            if (DateTime.TryParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
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
