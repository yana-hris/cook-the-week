namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
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
        public async Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model)
        {
            Ingredient ingredient = await ingredientService.GetByIdAsync(model.IngredientId!.Value);
            return ingredient.Id == model.IngredientId && ingredient.Name.ToLower() == model.Name.ToLower();
        }


        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRecipeWithIngredientsAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await recipeCategoryService.CategoryExistsByIdAsync(model.RecipeCategoryId!.Value);
            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), EntityValidationConstants.Recipe.RecipeCategoryIdInvalidErrorMessage);
            }

            if (!model.RecipeIngredients.Any())
            {
                AddValidationError(result, nameof(model.RecipeIngredients), EntityValidationConstants.Recipe.IngredientsRequiredErrorMessage);
            }

            if (!model.Steps.Any())
            {
                AddValidationError(result, nameof(model.Steps), EntityValidationConstants.Recipe.StepsRequiredErrorMessage);
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                try
                {
                    bool exists = await ValidateIngredientAsync(ingredient);
                }
                catch (RecordNotFoundException ex)
                {
                    logger.LogError($"Ingredient with name {ingredient.Name} and id {ingredient.Name} does not exist. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                    AddValidationError(result, nameof(ingredient.Name), EntityValidationConstants.RecipeIngredient.RecipeIngredientInvalidErrorMessage);
                }

                bool measureExists = await recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), EntityValidationConstants.RecipeIngredient.MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        AddValidationError(result, nameof(ingredient.SpecificationId), EntityValidationConstants.RecipeIngredient.SpecificationRangeErrorMessage);

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
                AddValidationError(result, string.Empty, EntityValidationConstants.ApplicationUser.AlreadyHaveAccountErrorMessage);
            }

            if (userWithUserNameExists != null)
            {
                AddValidationError(result, nameof(model.Username), EntityValidationConstants.ApplicationUser.UsernameAlreadyExistsErrorMessage);
            }

            if (userWithEmailExists != null)
            {
                AddValidationError(result, nameof(model.Email), EntityValidationConstants.ApplicationUser.EmailAlreadyExistsErrorMessage);
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
                AddValidationError(result, nameof(serviceModel.UserId), EntityValidationConstants.ApplicationUser.UserNotFoundErrorMessage);
            }

            // Validate the currently logged in user is the same
            string? currentUserId = userService.GetCurrentUserId();

            if (!string.IsNullOrEmpty(currentUserId) && userId.ToLower() != currentUserId.ToLower())
            {
                AddValidationError(result, nameof(serviceModel.UserId), EntityValidationConstants.ApplicationUser.InvalidUserIdErrorMessage);
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
                    AddValidationError(result, key, EntityValidationConstants.Recipe.InvalidRecipeIdErrorMessage);
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
                AddValidationError(result, nameof(model.Meals), EntityValidationConstants.MealPlan.MealsRequiredErrorMessage);
            }

            for (int i = 0; i < model.Meals.Count; i++)
            {
                var meal = model.Meals.ElementAt(i);
                bool recipeIdIsValid = await recipeRepository.ExistsByIdAsync(meal.RecipeId);

                if (!recipeIdIsValid)
                {
                    string key = $"Meals[{i}].RecipeId"; // sub-entry
                    AddValidationError(result, key, EntityValidationConstants.Recipe.InvalidRecipeIdErrorMessage);
                }

                if (!ValidateMealDates(meal))
                {
                    AddValidationError(result, nameof(meal.Date), EntityValidationConstants.Meal.DateRangeErrorMessage);
                }
            }

            return result;
        }

       
       /// <inheritdoc/>
        public async Task<ValidationResult> ValidateCategoryNameAsync<TCategoryFormModel>(TCategoryFormModel model,
                                                              Func<string, Task<int?>> getCategoryIdByNameFunc,
                                                              Func<int, Task<bool>> categoryExistsByIdFunc = null)
                                      where TCategoryFormModel : ICategoryFormModel
        {
            var result = new ValidationResult();

            // Editing scenario: check if the category name exists in another category
            if (model is ICategoryEditFormModel editModel && categoryExistsByIdFunc != null)
            {
                int? existingCategoryId = await getCategoryIdByNameFunc(editModel.Name);

                // If a category with the same name exists but it is not the current one being edited
                if (existingCategoryId.HasValue && existingCategoryId.Value != editModel.Id)
                {
                    AddValidationError(result, nameof(model.Name), EntityValidationConstants.Category.CategoryExistsErrorMessage);
                }
            }
            else
            {
                // Adding scenario: Check if a category with the same name already exists
                int? existingCategoryId = await getCategoryIdByNameFunc(model.Name);
                if (existingCategoryId.HasValue)
                {
                    AddValidationError(result, nameof(model.Name), EntityValidationConstants.Category.CategoryExistsErrorMessage);
                }
            }

            return result;

        }


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
