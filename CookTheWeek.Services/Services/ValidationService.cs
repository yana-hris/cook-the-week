namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using CookTheWeek.Common;
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
        private readonly IIngredientService ingredientService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IUserRepository userRepository;
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
            IUserRepository userRepository)
        {
            this.recipeCategoryService = categoryService;
            this.ingredientService = ingredientService;
            this.recipeIngredientService = recipeIngredientService;
            this.recipeRepository = recipeRepository;
            this.userRepository = userRepository;
        }

       
        /// <summary>
        /// Checks if an ingredient exists by name in the database returns a flag
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public async Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model)
        {
            bool exists = await ingredientService.ExistsByNameAsync(model.Name);
            // TODO: Implement logic to create ingredients which don`t exist!
            return exists;

        }

        /// <summary>
        /// Custom validation for recipes upon adding and editing Recipe: checks if the selected category, ingredient, measure and specification exist in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ValidationResult> ValidateRecipeAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            bool categoryExists = await recipeCategoryService.CategoryExistsByIdAsync(model.RecipeCategoryId.Value);

            if (!categoryExists)
            {
    
                AddValidationError(result, nameof(model.RecipeCategoryId), EntityValidationConstants.Recipe.RecipeCategoryIdInvalidErrorMessage);

            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                bool exists = await ValidateIngredientAsync(ingredient);

                if (!exists)
                {
                    AddValidationError(result, nameof(ingredient.Name), EntityValidationConstants.RecipeIngredient.RecipeIngredientInvalidErrorMessage);
                }

                bool measureExists = await recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), EntityValidationConstants.RecipeIngredient.MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        result.IsValid = false;
                        result.Errors.Add(nameof(ingredient.SpecificationId), EntityValidationConstants.RecipeIngredient.SpecificationRangeErrorMessage);
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Validates if a user with the given email or username already exists and returns a Validation result, with a collection of all model errors (dictionary).
        /// </summary>
        /// <param name="model">The Register form model (user input fields)</param>
        /// <returns>ValidationResult</returns>
        public async Task<ValidationResult> ValidateRegisterUserModelAsync(RegisterFormModel model)
        {
            var result = new ValidationResult();

            var userWithEmailExists = await userRepository.FindByEmailAsync(model.Email);
            var userWithUserNameExists = await userRepository.FindByNameAsync(model.Username);

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


        /// <summary>
        /// Validates the service model data before creating a meal plan Add form model
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns></returns>
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
            string? currentUserId = userRepository.GetCurrentUserId();

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

        /// <summary>
        /// Validates if the ids of a meal`s collection are valid recipes
        /// </summary>
        /// <param name="meals"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A generic method to validate if a given TCategory is not already existent in the database, which can lead to data duplication (by name)
        /// </summary>
        /// <typeparam name="TCategory">The type of category to check for</typeparam>
        /// <typeparam name="TCategoryAddFormModel">The form model for adding a category</typeparam>
        /// <param name="model">The model</param>
        /// <returns>Validation Result, having isValid and Errors properties</returns>
        /// <exception cref="InvalidCastException"></exception>
        public async Task<ValidationResult> ValidateCategoryByName<TCategoryAddFormModel>(TCategoryAddFormModel model)
        {
            var result = new ValidationResult();

            bool exists = false;

            if (model is RecipeCategoryAddFormModel recipeCategoryModel)
            {
                exists = await recipeCategoryService.CategoryExistsByNameAsync(recipeCategoryModel.Name);
            }
            else if (model is IngredientCategoryAddFormModel ingredientCategoryModel)
            {
                exists = await ingredientCategoryService.CategoryExistsByNameAsync(ingredientCategoryModel.Name);
            }
            else
            {
                throw new InvalidCastException(InvalidCastExceptionMessages.IngredientAddOrEditModelUnsuccessfullyCasted);
            }

            if (!exists)
            {
                AddValidationError(result, nameof(model.Name))

                ModelState.AddModelError(nameof(model.Name), $"Category with name {model.Name} already exists!");
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
