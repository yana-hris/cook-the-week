namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Models.Interfaces;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.EntityValidationConstants;

    public class ValidationService : IValidationService
    {
        private readonly ICategoryService<RecipeCategory, 
            RecipeCategoryAddFormModel, 
            RecipeCategoryEditFormModel, 
            RecipeCategorySelectViewModel> recipeCategoryService;
        private readonly ICategoryService<IngredientCategory,
            IngredientCategoryAddFormModel,
            IngredientCategoryEditFormModel,
            IngredientCategorySelectViewModel> ingredientCategoryService;
        private readonly IRecipeRepository recipeRepository;
        private readonly IRecipeService recipeService;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;
        private readonly ILogger<ValidationService> logger;
        public ValidationService(ICategoryService<RecipeCategory, 
                                            RecipeCategoryAddFormModel, 
                                            RecipeCategoryEditFormModel, 
                                            RecipeCategorySelectViewModel> categoryService,
            ICategoryService<IngredientCategory,
                                            IngredientCategoryAddFormModel,
                                            IngredientCategoryEditFormModel,
                                            IngredientCategorySelectViewModel> ingredientCategoryService,
            IIngredientService ingredientService,
            IRecipeIngredientService recipeIngredientService,
            IRecipeRepository recipeRepository,
            IRecipeService recipeService,
            ILogger<ValidationService> logger,
            IUserRepository userRepository)
        {
            this.recipeCategoryService = categoryService;
            this.ingredientService = ingredientService;
            this.recipeIngredientService = recipeIngredientService;
            this.recipeRepository = recipeRepository;
            this.userRepository = userRepository;
            this.recipeService = recipeService;
            this.logger = logger;
        }

              
        /// <inheritdoc/>
        public async Task<bool> ValidateIngredientForRecipeIngredientAsync(RecipeIngredientFormModel model)
        {
            Ingredient ingredient = await ingredientService.GetByIdAsync(model.IngredientId!.Value);
            return ingredient.Id == model.IngredientId && ingredient.Name.ToLower() == model.Name.ToLower();
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateIngredientFormModelAsync(IIngredientFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await ingredientCategoryService.CategoryExistsByIdAsync(model.CategoryId);

            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.CategoryId), CategoryValidation.CategoryInvalidErrorMessage);
            }

            // Check if an ingredient with the same name already exists
            bool existingByName = await ingredientService.ExistsByNameAsync(model.Name);

            if (existingByName)
            {
                int existingIngredientId = await ingredientService.GetIdByNameAsync(model.Name);

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
        public async Task<bool> ValidateCategoryCanBeDeletedAsync(int id)
        {
            return await recipeRepository.GetAllQuery()
                .Where(r => r.RecipesIngredients.Any(ri => ri.IngredientId == id))
                .AnyAsync();
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRecipeWithIngredientsAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await recipeCategoryService.CategoryExistsByIdAsync(model.RecipeCategoryId!.Value);
            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), EntityValidationConstants.RecipeValidation.RecipeCategoryIdInvalidErrorMessage);
            }

            if (!model.RecipeIngredients.Any())
            {
                AddValidationError(result, nameof(model.RecipeIngredients), EntityValidationConstants.RecipeValidation.IngredientsRequiredErrorMessage);
            }

            if (!model.Steps.Any())
            {
                AddValidationError(result, nameof(model.Steps), EntityValidationConstants.RecipeValidation.StepsRequiredErrorMessage);
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

                bool measureExists = await recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), RecipeIngredientValidation.MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        AddValidationError(result, nameof(ingredient.SpecificationId), RecipeIngredientValidation.SpecificationRangeErrorMessage);

                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model)
        {
            var result = new ValidationResult();

            var userWithEmailExists = await userRepository.GetByEmailAsync(model.Email);
            var userWithUserNameExists = await userRepository.GetByUsernameAsync(model.Username);

            if (userWithEmailExists != null || userWithUserNameExists != null)
            {
                AddValidationError(result, string.Empty, EntityValidationConstants.ApplicationUserValidation.AlreadyHaveAccountErrorMessage);
            }

            if (userWithUserNameExists != null)
            {
                AddValidationError(result, nameof(model.Username), EntityValidationConstants.ApplicationUserValidation.UsernameAlreadyExistsErrorMessage);
            }

            if (userWithEmailExists != null)
            {
                AddValidationError(result, nameof(model.Email), EntityValidationConstants.ApplicationUserValidation.EmailAlreadyExistsErrorMessage);
            }

            return result;

        }


        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateMealPlanServiceModelAsync(MealPlanServiceModel serviceModel)
        {
            var result = new ValidationResult();

            string userId = serviceModel.UserId;
            var user = await userRepository.ExistsByIdAsync(userId);

            // Validate userId exists in the database;
            if (!user)
            {
                AddValidationError(result, nameof(serviceModel.UserId), EntityValidationConstants.ApplicationUserValidation.UserNotFoundErrorMessage);
            }

            // Validate the currently logged in user is the same
            string? currentUserId = userService.GetCurrentUserId();

            if (!string.IsNullOrEmpty(currentUserId) && userId.ToLower() != currentUserId.ToLower())
            {
                AddValidationError(result, nameof(serviceModel.UserId), EntityValidationConstants.ApplicationUserValidation.InvalidUserIdErrorMessage);
            }

            // Validate recipe id`s are valid recipes
            var meals = serviceModel.Meals;

            for (int i = 0; i < meals.Count; i++)
            {
                var meal = serviceModel.Meals.ElementAt(i);
                bool recipeIdIsValid = await recipeRepository.ExistsByIdAsync(meal.RecipeId);

                if (!recipeIdIsValid)
                {
                    string key = $"Meals[{i}].RecipeId"; // sub-entry
                    AddValidationError(result, key, EntityValidationConstants.RecipeValidation.InvalidRecipeIdErrorMessage);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateMealPlanEditFormModelAsync(MealPlanEditFormModel model)
        {
            var result = new ValidationResult();

            if (!model.Meals.Any())
            {
                AddValidationError(result, nameof(model.Meals), EntityValidationConstants.MealPlanValidation.MealsRequiredErrorMessage);
            }

            for (int i = 0; i < model.Meals.Count; i++)
            {
                var meal = model.Meals.ElementAt(i);
                bool recipeIdIsValid = await recipeRepository.ExistsByIdAsync(meal.RecipeId);

                if (!recipeIdIsValid)
                {
                    string key = $"Meals[{i}].RecipeId"; // sub-entry
                    AddValidationError(result, key, EntityValidationConstants.RecipeValidation.InvalidRecipeIdErrorMessage);
                }

                if (!ValidateMealDates(meal))
                {
                    AddValidationError(result, nameof(meal.Date), EntityValidationConstants.MealValidation.DateRangeErrorMessage);
                }
            }

            return result;
        }

        
       /// <inheritdoc/>       
        public async Task<ValidationResult> ValidateCategoryAsync<TCategoryFormModel>(TCategoryFormModel model,
                                                   Func<string, Task<int?>> getCategoryIdByNameFunc,
                                                   Func<int, Task<bool>> categoryExistsByIdFunc = null)
                           where TCategoryFormModel : ICategoryFormModel
        {
            var result = new ValidationResult();

            // Editing scenario
            if (model is ICategoryEditFormModel editModel && categoryExistsByIdFunc != null)
            {
                bool categoryExists = await categoryExistsByIdFunc(editModel.Id); // check if exists

                if (!categoryExists)
                {
                    logger.LogError($"Record not found: A category with ID {editModel.Id} was not found.");
                    throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
                }

                // Check if the category name exists in another category
                int? existingCategoryId = await getCategoryIdByNameFunc(editModel.Name);

                // If a category with the same name exists but it is not the current one being edited
                if (existingCategoryId.HasValue && existingCategoryId.Value != editModel.Id)
                {
                    AddValidationError(result, nameof(editModel.Name), CategoryValidation.CategoryExistsErrorMessage);
                }
            }
            else
            {
                // Adding scenario: Check if a category with the same name already exists
                int? existingCategoryId = await getCategoryIdByNameFunc(model.Name);
                if (existingCategoryId.HasValue)
                {
                    AddValidationError(result, nameof(model.Name), CategoryValidation.CategoryExistsErrorMessage);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> CanCategoryBeDeletedAsync<TCategory>(int categoryId, 
                                                            Func<int, Task<TCategory?>> getCategoryByIdFunc,
                                                            Func<int, Task<bool>> hasDependenciesFunc)
            where TCategory : ICategory
        {
            var category = await getCategoryByIdFunc(categoryId);
            
            if (category == null)
            {
                logger.LogError($"Record not found: A category with if {categoryId} was not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            // Check if the category has dependencies (e.g., recipes, ingredients)
            bool hasDependencies = await hasDependenciesFunc(category.Id);

            if (hasDependencies)
            {
                logger.LogError($"Category with id {categoryId} has dependencies and cannot be deleted.");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.CategoryCannoBeDeletedExceptionMessage);
            }

            return true;
        }

        // TODO: check!
        public async Task<ValidationResult> ValidateLikeOrUnlikeRecipe(string userId, string recipeId)
        {
            var result = new ValidationResult();
            string? currentUserId = userService.GetCurrentUserId();

           
            // Validate if recipe exists
            if (string.IsNullOrEmpty(recipeId))
            {
                logger.LogError($"Null reference error: RecipeId is null upon attempt of user with id {userId} to like it.");
                AddValidationError(result, nameof(recipeId), ArgumentNullExceptionMessages.RecipeNullExceptionMessage);
            }

            bool exists = await recipeRepository.ExistsByIdAsync(recipeId);

            // Validate user authorization
            if (!string.IsNullOrEmpty(currentUserId) &&
                !string.IsNullOrEmpty(userId) &&
                !GuidHelper.CompareTwoGuidStrings(currentUserId, userId))
            {
                logger.LogError($"Unauthorized access attempt: User {userId} tried to access {recipeId} but does not have the necessary permissions.");
                AddValidationError(result, nameof(userId), UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<bool> ValidateIngredientCanBeDeleted(int id)
        {
            return recipeRepository.GetAllQuery()
                .AnyAsync(r => r.RecipesIngredients.Any(ri => ri.IngredientId == id));
        }



        // PRIVATE METHODS:

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
