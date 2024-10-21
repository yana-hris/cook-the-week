namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Models.Interfaces;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.User;

    using static CookTheWeek.Common.EntityValidationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    

    public class ValidationService : IValidationService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IUserRepository userRepository;
        private readonly IIngredientRepository ingredientRepository;
        private readonly ICategoryRepository<IngredientCategory> ingredientCategoryRepository;
        private readonly ILogger<ValidationService> logger;

        private readonly Guid userId;
        private readonly bool isAdmin;

        public ValidationService(
            IRecipeRepository recipeRepository,
            IUserRepository userRepository,
            IIngredientRepository ingredientRepository,
            ICategoryRepository<IngredientCategory> ingredientCategoryRepository,
            IUserContext userContext,
            ILogger<ValidationService> logger)
        {
            this.recipeRepository = recipeRepository;
            this.userRepository = userRepository;
            this.ingredientRepository = ingredientRepository;
           
            this.ingredientCategoryRepository = ingredientCategoryRepository;
            this.logger = logger;
            userId = userContext.UserId;
            isAdmin = userContext.IsAdmin;
        }

        

        // INGREDIENT:
        /// <inheritdoc/>
        /// // ADMIN
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
        /// // ADMIN
        public Task<bool> CanIngredientBeDeletedAsync(int id)
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
                if ((existingCategoryId.HasValue && existingCategoryId.Value != default) && existingCategoryId.Value != editModel.Id)
                {
                    AddValidationError(result, nameof(editModel.Name), CategoryValidation.CategoryExistsErrorMessage);
                }
            }
            else
            {
                // Adding scenario: Check if a category with the same name already exists
                int? existingCategoryId = await categoryRepository.GetIdByNameAsync(model.Name);
                if (existingCategoryId.HasValue && existingCategoryId.Value != default)
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
            // Check if the category exists
            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                logger.LogError($"Category of type {typeof(TCategory).Name} with ID {categoryId} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.CategoryNotFoundExceptionMessage, null);
            }

            // Check if the category has dependencies using the generic HasDependenciesAsync method
            bool hasDependencies = await categoryRepository.HasDependenciesAsync<TDependency>(categoryId);

            if (hasDependencies)
            {
                logger.LogError($"Category of type {typeof(TCategory).Name} with ID {categoryId} has dependencies and cannot be deleted.");
                throw new InvalidOperationException($"Category of type {typeof(TCategory).Name} cannot be deleted because it has dependencies.");
            }

            return true;  // Category can be deleted
        }

       
       

        // PRIVATE METHODS:
        
        

        

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
