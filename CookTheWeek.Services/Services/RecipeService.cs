namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static Common.ExceptionMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;
   

    public class RecipeService : IRecipeService
    {
        
        private readonly IRecipeRepository recipeRepository;

        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IStepService stepService;
        
        private readonly IValidationService validationService;
        private readonly ILogger<RecipeService> logger;
        private readonly Guid userId;
        private readonly bool isAdmin;

        public RecipeService(IRecipeRepository recipeRepository,
            IStepService stepService,
            IRecipeIngredientService recipeIngredientService,
            ILogger<RecipeService> logger,
            IUserContext userContext,
            IValidationService validationService)
        {
            this.recipeRepository = recipeRepository;
            
            this.recipeIngredientService = recipeIngredientService;
            this.stepService = stepService;

            this.validationService = validationService;
            this.logger = logger;
            this.userId = userContext.UserId;
            this.isAdmin = userContext.IsAdmin; 
        }


        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllAsync(AllRecipesQueryModel queryModel)
        {
            
            IQueryable<Recipe> recipesQuery = recipeRepository.GetAllQuery();

            if (!recipesQuery.Any())
            {
                logger.LogError("No recipes found in the database.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }


            if (userId != default)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.OwnerId == userId || r.IsSiteRecipe);
            }
            else
            {
                recipesQuery = recipesQuery
                    .Where(r => r.IsSiteRecipe);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                recipesQuery = recipesQuery
                    .Where(r => r.Category.Name == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                recipesQuery = recipesQuery
                    .Where(r => EF.Functions.Like(r.Title, wildCard) ||
                            EF.Functions.Like(r.Description, wildCard) ||
                            r.RecipesIngredients!.Any(ri => EF.Functions.Like(ri.Ingredient.Name, wildCard)));
            }

            // Check if sorting is applied and if it exists in sorting enum
            string recipeSorting = queryModel.RecipeSorting.ToString("G");

            if (string.IsNullOrEmpty(recipeSorting) || !Enum.IsDefined(typeof(RecipeSorting), recipeSorting))
            {
                queryModel.RecipeSorting = RecipeSorting.Newest;
            }

            recipesQuery = queryModel.RecipeSorting switch
            {
                RecipeSorting.Newest => recipesQuery
                    .OrderByDescending(r => r.CreatedOn),
                RecipeSorting.Oldest => recipesQuery
                    .OrderBy(r => r.CreatedOn),
                RecipeSorting.CookingTimeAscending => recipesQuery
                    .OrderBy(r => r.TotalTime),
                RecipeSorting.CookingTimeDescending => recipesQuery
                    .OrderByDescending(r => r.TotalTime),
                _ => recipesQuery.OrderByDescending(r => r.CreatedOn)
            };

            if (queryModel.CurrentPage == default)
            {
                queryModel.CurrentPage = DefaultPage;
            }

            if (queryModel.RecipesPerPage == default)
            {
                queryModel.RecipesPerPage = DefaultRecipesPerPage;
            }

            queryModel.TotalRecipes = recipesQuery.Count();

            
            ICollection<RecipeAllViewModel> model = await recipesQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.RecipesPerPage)
                .Take(queryModel.RecipesPerPage)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id,
                    OwnerId = r.OwnerId,
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Description = r.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.CategoryId,
                        Name = r.Category.Name
                    },
                    Servings = r.Servings,
                    CookingTime = FormatCookingTime(r.TotalTime)
                })
                .ToListAsync();

            if (!model.Any())
            {
                logger.LogError("No recipes found by these critearia.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }

            return model;
           
        }

        /// <inheritdoc/>
        public async Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model)
        {
            ValidationResult result = await validationService.ValidateRecipeFormModelAsync(model);
            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            Recipe recipe = MapFromModelToRecipe(model, new Recipe());
            string recipeId = await recipeRepository.AddAsync(recipe); 
            await recipeIngredientService.EditAsync(recipe.Id, model.RecipeIngredients);
            await ProcessRecipeStepsAsync(Guid.Parse(recipeId), model);
            
            return OperationResult<string>.Success(recipeId);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryEditRecipeAsync(RecipeEditFormModel model)
        {
            Recipe? recipe = await recipeRepository
                .GetByIdQuery(model.Id) 
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Measure)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Specification)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            validationService.ValidateUserIsResourceOwnerAsync(recipe.OwnerId); // UnauthorizedUserExc

            ValidationResult result = await validationService.ValidateRecipeFormModelAsync(model); // no exception

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            recipe = MapFromModelToRecipe(model, recipe);
            await recipeIngredientService.EditAsync(recipe.Id, model.RecipeIngredients);
            await ProcessRecipeStepsAsync(model.Id, model);

            await recipeRepository.UpdateAsync(recipe);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<Recipe> GetByIdForDetailsAsync(Guid id)
        {
            Recipe? recipe = await recipeRepository.GetByIdQuery(id) 
                .Include(r => r.Owner)
                .Include(r => r.Steps)
                .Include(r => r.Category)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            return recipe;
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(Guid id)
        {

            Recipe? recipeToDelete = await recipeRepository
                .GetByIdQuery(id)
                .FirstOrDefaultAsync();

            if (recipeToDelete == null)
            {
                logger.LogError($"Record not found: {nameof(Recipe)} with ID {id} was not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            validationService.ValidateUserIsResourceOwnerAsync(recipeToDelete.OwnerId); // UnauthorizedUserExc

            
            bool isIncluded = await validationService.CanRecipeBeDeletedAsync(id);

            if (isIncluded)
            {
                logger.LogError($"Invalid operation while trying to delete recipe with id {id}. Recipe is included in active mealplans and cannot be deleted.");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage);
            }

            await SoftDeleteRecipeAsync(recipeToDelete);
           
        }
        
       
        /// <inheritdoc/>
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(Guid id)
        {
            Recipe? recipe = await recipeRepository.GetByIdQuery(id)
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Measure)
                .Include(ri => ri.RecipesIngredients)
                    .ThenInclude(ri => ri.Specification)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                logger.LogError($"Recipe with id {id} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            validationService.ValidateUserIsResourceOwnerAsync(recipe.OwnerId);

            // TODO: Consider using Automapper
            RecipeEditFormModel model = new RecipeEditFormModel()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Steps = recipe.Steps.Select(s => new StepFormModel()
                {
                    Id = s.Id,
                    Description = s.Description

                }).ToList(),
                Servings = recipe.Servings,
                CookingTimeMinutes = (int)recipe.TotalTime.TotalMinutes,
                ImageUrl = recipe.ImageUrl,
                RecipeCategoryId = recipe.CategoryId,
                RecipeIngredients = recipe.RecipesIngredients.Select(ri => new RecipeIngredientFormModel()
                {
                    IngredientId = ri.IngredientId,
                    Name = ri.Ingredient.Name,
                    Qty = RecipeIngredientQtyFormModel.ConvertFromDecimalQty(ri.Qty, ri.Measure.Name),
                    MeasureId = ri.MeasureId,
                    SpecificationId = ri.SpecificationId
                }).ToList()
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllAddedByUserIdAsync()
        {
            var recipes = await recipeRepository
                .GetAllQuery()
                .Where(r => r.OwnerId == userId)
                .ToListAsync();
            
            // TODO: Consider using Automapper
            ICollection<RecipeAllViewModel> model = recipes.Select(r => new RecipeAllViewModel()
            {
                Id = r.Id,
                OwnerId = r.OwnerId,
                ImageUrl = r.ImageUrl,
                Title = r.Title,
                Description = r.Description,
                Category = new RecipeCategorySelectViewModel()
                {
                    Id = r.CategoryId,
                    Name = r.Category.Name
                },
                Servings = r.Servings,
                CookingTime = FormatCookingTime(r.TotalTime)
            }).ToList();

            return model;
        }
       
        /// <inheritdoc/>
        public async Task<int?> GetAllCountAsync()
        {
            return await recipeRepository
                .GetAllQuery()
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task<int?> GetMineCountAsync()
        {
            return await recipeRepository
                .GetAllQuery()
                .Where(r => r.OwnerId == userId)
                .CountAsync();

        }
       
        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllSiteRecipesAsync()
        {
            List<RecipeAllViewModel> siteRecipes = await recipeRepository.GetAllQuery()
                    .Where(r => r.IsSiteRecipe)
                    .Select(r => new RecipeAllViewModel()
                    {
                        Id = r.Id,
                        OwnerId = r.OwnerId,
                        ImageUrl = r.ImageUrl,
                        Title = r.Title,
                        Description = r.Description,
                        Category = new RecipeCategorySelectViewModel()
                        {
                            Id = r.CategoryId,
                            Name = r.Category.Name
                        },
                        Servings = r.Servings,
                        CookingTime = FormatCookingTime(r.TotalTime)
                    }).ToListAsync();

            return siteRecipes;
        }

        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllNonSiteRecipesAsync()
        {
            ICollection<RecipeAllViewModel> allUserRecipes = await recipeRepository.GetAllQuery()
                .Where(r => !r.IsSiteRecipe)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id,
                    OwnerId = r.OwnerId,
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Description = r.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.CategoryId,
                        Name = r.Category.Name
                    },
                    Servings = r.Servings,
                    CookingTime = FormatCookingTime(r.TotalTime)
                }).ToListAsync();

            return allUserRecipes;
        }
       
        
        /// <inheritdoc/>
        public async Task<Recipe> GetForMealByIdAsync(Guid recipeId)
        {
            Recipe? recipe =  await recipeRepository.GetByIdQuery(recipeId)
                .Include(r => r.Category)
                .FirstOrDefaultAsync();

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            return recipe;
        }

        /// <inheritdoc/>
        public async Task<ICollection<Recipe>> GetAllByIds(ICollection<string> recipeIds)
        {
            var guidRecipeIds = recipeIds.Select(id => Guid.Parse(id)).ToList(); // Convert strings to Guids

            var recipes = await recipeRepository.GetAllQuery()
                .Where(r => guidRecipeIds.Contains(r.Id))
                .ToListAsync();

            return recipes;
        }

        /// <inheritdoc/>
        public async Task<ICollection<string>> GetAllRecipeIdsAddedByCurrentUserAsync()
        {
            ICollection<string> allUserAddedRecipesIds = await recipeRepository.GetAllQuery()
                .Where(recipe => recipe.OwnerId == userId)
                .Select(recipe => recipe.Id.ToString())
                .ToListAsync();

            return allUserAddedRecipesIds;
        }

        // PRIVATE HELPER METHODS        

        /// <summary>
        /// Helper method which implements the soft delete of a recipe. All related entities are deleted as well using an event dispatcher.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private async Task SoftDeleteRecipeAsync(Recipe recipe)
        {
            // SOFT DELETE
            recipe.OwnerId = Guid.Parse(DeletedUserId);
            recipe.IsDeleted = true;          

            await recipeRepository.UpdateAsync(recipe);
        }


        /// <summary>
        /// Maps only non-collection properties of a RecipeAdd/EditFormModel to a Recipe or throws an exception.
        /// </summary>
        /// <remarks>For adding a new recipe, userId and isAdmin parameters need to be passed for the method to work.
        /// For editing an existing recipe these parameters are not necessary.</remarks>
        /// <param name="model"></param>
        /// <param name="recipe"></param>
        /// <returns>static method</returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        // TODO: Consider using Automapper
        private Recipe MapFromModelToRecipe(IRecipeFormModel model, Recipe recipe)
        {
            if (model is RecipeAddFormModel || model is RecipeEditFormModel)
            {
                recipe.Title = model.Title;
                recipe.Description = model.Description;
                recipe.Servings = model.Servings!.Value;
                recipe.TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes!.Value);
                recipe.ImageUrl = model.ImageUrl;
                recipe.CategoryId = model.RecipeCategoryId!.Value;

                if (model is RecipeAddFormModel addModel)
                {
                    if (userId == Guid.Empty)
                    {
                        logger.LogError($"Missing argument: {nameof(userId)} is null and model of type {model.GetType().Name} cannot be mapped to Entity {nameof(Recipe)} in method {nameof(MapFromModelToRecipe)}");
                        throw new ArgumentNullException(ArgumentNullExceptionMessages.UserNullExceptionMessage);
                    }

                    recipe.OwnerId = userId;
                    recipe.IsSiteRecipe = isAdmin;
                }

                return recipe;
            }

            logger.LogError($"Type cast error: Unable to cast {model.GetType().Name} to {nameof(RecipeAddFormModel)} or {nameof(RecipeEditFormModel)} in method {nameof(MapFromModelToRecipe)}.");
            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);

        }

        /// <summary>
        /// A helper method to Add or Edit a recipe`s steps. Works with both Add and Edit Recipe Form Models. Throws an exception in case of unsucessful cast
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        private async Task ProcessRecipeStepsAsync(Guid recipeId, IRecipeFormModel model)
        {
            var steps = model.Steps;

            if (model is RecipeAddFormModel)
            {
                await stepService.AddByRecipeIdAsync(recipeId, steps);
            }
            else if (model is RecipeEditFormModel recipeEditFormModel)
            {
                await stepService.UpdateByRecipeIdAsync(recipeId, steps);
            }
            else
            {
                logger.LogError($"Type cast error: Uable to cast {model.GetType().Name} to {nameof(RecipeAddFormModel)} or {nameof(RecipeEditFormModel)} in method {nameof(ProcessRecipeStepsAsync)}.");
                throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
            }
        }
    }
}
