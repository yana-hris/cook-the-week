namespace CookTheWeek.Services.Data.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static CookTheWeek.Common.EntityValidationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class RecipeValidationService : IRecipeValidationService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IIngredientRepository ingredientRepository;
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly ICategoryRepository<RecipeCategory> recipeCategoryRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILogger<RecipeValidationService> logger;

        private readonly Guid userId;
        private readonly bool isAdmin;
        public RecipeValidationService(IRecipeRepository recipeRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IIngredientRepository ingredientRepository,
            ICategoryRepository<RecipeCategory> recipeCategoryRepository,
            ITagRepository tagRepository,
            IUserContext userContext,
            ILogger<RecipeValidationService> logger)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.recipeCategoryRepository = recipeCategoryRepository;
            this.tagRepository = tagRepository;
            this.logger = logger;

            this.userId = userContext.UserId;
            this.isAdmin = userContext.IsAdmin;
        }     

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateRecipeFormModelAsync(IRecipeFormModel model)
        {
            var result = new ValidationResult();

            if (model == null)
            {
                throw new ArgumentNullException();
            }

            bool categoryExists = await recipeCategoryRepository.ExistsByIdAsync(model.RecipeCategoryId!.Value);
            if (!categoryExists)
            {
                AddValidationError(result, nameof(model.RecipeCategoryId), RecipeValidation.RecipeCategoryIdInvalidErrorMessage);
            }

            if (model.RecipeIngredients.Count == 0)
            {
                AddValidationError(result, nameof(model.RecipeIngredients), RecipeValidation.IngredientsRequiredErrorMessage);
            }

            if (model.Steps.Count == 0)
            {
                AddValidationError(result, nameof(model.Steps), RecipeValidation.StepsRequiredErrorMessage);
            }

            if (model.SelectedTagIds.Count > 0)
            {
                List<Tag> existingTags = await tagRepository.GetAllQuery().ToListAsync();

                foreach (var tagId in model.SelectedTagIds)
                {
                    Tag? tag = existingTags.FirstOrDefault(t => t.Id == tagId);

                    if (tag == null)
                    {
                        AddValidationError(result, nameof(model.SelectedTagIds), RecipeValidation.TagIdInvalidErrorMessage);
                    }
                }
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                try
                {
                    bool exists = await ValidateRecipeIngredientAsync(ingredient);
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
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<bool> CanRecipeBeDeletedAsync(Guid id)
        {
            return recipeRepository.GetAllQuery()
                .Where(r => r.Meals.Any(m => m.RecipeId == id && !m.IsCooked))
                .AnyAsync();
        }


        /// <inheritdoc/>
        public void ValidateUserIsRecipeOwner(Guid ownerId)
        {
            if (userId == default)
            {
                logger.LogError($"Unauthorized attempt of a user to access resource. User not logged in or userId is null.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            if (userId != ownerId && !isAdmin)
            {
                logger.LogError($"User with id {userId} is not the resource owner.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeAuthorizationExceptionMessage);
            }
        }

        /// <inheritdoc/>
        public async Task ValidateRecipeExistsAsync(Guid recipeId)
        {
            // Validate if recipeId is provided
            if (recipeId == default)
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

        // PRIVATE METHODS:

        /// <summary>
        /// A helper method that checks if a recipe-ingredient is a valid and existing ingredient in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false or throws RecordNotFound</returns>
        /// <remarks>May throw RecordNotFoundException if the ingredient does not exist</remarks>
        private async Task<bool> ValidateRecipeIngredientAsync(RecipeIngredientFormModel model)
        {
            Ingredient ingredient = await ingredientRepository.GetByIdAsync(model.IngredientId!.Value);
            return ingredient.Id == model.IngredientId && ingredient.Name.ToLower() == model.Name.ToLower();
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
