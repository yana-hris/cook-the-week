namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.User;

    using static CookTheWeek.Common.EntityValidationConstants.ApplicationUser;
    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.EntityValidationConstants.RecipeIngredient;

    public class ValidationService : IValidationService
    {
        private readonly ICategoryService categoryService;
        private readonly IRecipeRepository recipeRepository;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IUserRepository userRepository;
        public ValidationService(ICategoryService categoryService,
            IIngredientService ingredientService,
            IRecipeIngredientService recipeIngredientService,
            IRecipeRepository recipeRepository,
            IUserRepository userRepository)
        {
            this.categoryService = categoryService;
            this.ingredientService = ingredientService;
            this.recipeIngredientService = recipeIngredientService;
            this.recipeRepository = recipeRepository;
            this.userRepository = userRepository;
        }

        // TODO: Implement logic to create ingredients which don`t exist!
        public async Task<bool> ValidateIngredientAsync(RecipeIngredientFormModel model)
        {
            bool exists = await ingredientService.ExistsByNameAsync(model.Name);
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

            bool categoryExists = await categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId.Value);

            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), RecipeCategoryIdInvalidErrorMessage);

            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                bool exists = await ValidateIngredientAsync(ingredient);

                if (!exists)
                {
                    AddValidationError(result, nameof(ingredient.Name), RecipeIngredientInvalidErrorMessage);
                }

                bool measureExists = await recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId.Value);

                if (!measureExists)
                {
                    AddValidationError(result, nameof(ingredient.MeasureId), MeasureRangeErrorMessage);
                }

                if (ingredient.SpecificationId != null && ingredient.SpecificationId.HasValue)
                {
                    bool specificationExists = await recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value);
                    if (!specificationExists)
                    {
                        result.IsValid = false;
                        result.Errors.Add(nameof(ingredient.SpecificationId), SpecificationRangeErrorMessage);
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
                AddValidationError(result, string.Empty, AlreadyHaveAccountErrorMessage);
            }

            if (userWithUserNameExists != null)
            {
                AddValidationError(result, nameof(model.Username), UsernameAlreadyExistsErrorMessage);
            }

            if (userWithEmailExists != null)
            {
                AddValidationError(result, nameof(model.Email), EmailAlreadyExistsErrorMessage);
            }

            return result;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ValidationResult> ValidateMealPlanServiceModelAsync(MealPlanServiceModel serviceModel)
        {
            var result = new ValidationResult();

            string userId = serviceModel.UserId;
            var user = await userRepository.ExistsByIdAsync(userId);

            // Validate userId exists in the database;
            if (!user)
            {
                AddValidationError(result, nameof(serviceModel.UserId), UserNotFoundErrorMessage);
            }

            // Validate the currently logged in user is the same
            string? currentUserId = await userRepository.GetCurrentUserId();

            if (!string.IsNullOrEmpty(currentUserId) && userId.ToLower() != currentUserId.ToLower())
            {
                AddValidationError(result, nameof(serviceModel.UserId), InvalidUserIdErrorMessage);
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
                    AddValidationError(result, key, InvalidRecipeIdErrorMessage);
                }
            }

            return result;
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
