namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Enums;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.Recipe;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;

    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class RecipeService : IRecipeService
    {
        
        private readonly IRecipeRepository recipeRepository;

        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IStepService stepService;
        private readonly IRecipeTagService recipeTagService;
        
        private readonly IRecipeValidationService recipeValidator;
        private readonly ILogger<RecipeService> logger;
        private readonly Guid userId;
        private readonly bool isAdmin;

        public RecipeService(IRecipeRepository recipeRepository,
            IRecipeIngredientService recipeIngredientService,
            IStepService stepService,
            IRecipeTagService recipeTagService,
            IRecipeValidationService recipeValidator,
            ILogger<RecipeService> logger,
            IUserContext userContext)
        {
            this.recipeRepository = recipeRepository;
            this.recipeIngredientService = recipeIngredientService;
            this.stepService = stepService;
            this.recipeTagService = recipeTagService;
            this.recipeValidator = recipeValidator;
            this.logger = logger;            

            userId = userContext.UserId;
            isAdmin = userContext.IsAdmin; 
        }


        /// <inheritdoc/>
        public async Task<ICollection<Recipe>> GetAllAsync(AllRecipesQueryModel queryModel)
        {
            IQueryable<Recipe> recipesQuery = recipeRepository
                .GetAllQuery()
                .Include(r => r.RecipeTags)
                .Include(r => r.Category)
                .AsNoTracking();

            if (!recipesQuery.Any())
            {
                logger.LogError("No recipes found in the database.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }
            
            if (userId != default && !isAdmin) // Logged in user (sees site recipes + own recipes
            {
                recipesQuery = recipesQuery
                    .Where(r => r.OwnerId == userId || r.IsSiteRecipe);
            }
            else if(userId == default) // No logged in user
            {
                recipesQuery = recipesQuery
                    .Where(r => r.IsSiteRecipe);
            }

            // Filtering Recipe Source: the default behaviour is to show All Recipes (both Site & User)
            if (queryModel.RecipeSource.HasValue && 
                Enum.IsDefined(typeof(RecipeSource), queryModel.RecipeSource.Value))
            {
                RecipeSource recipeSource = (RecipeSource)queryModel.RecipeSource.Value;

                recipesQuery = recipeSource switch
                {
                    RecipeSource.Site => recipesQuery
                     .Where(r => r.IsSiteRecipe),
                    RecipeSource.User => recipesQuery
                        .Where(r => !r.IsSiteRecipe),
                    _ => recipesQuery
                };
            }
            

            if (queryModel.MealTypeId.HasValue)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.CategoryId == queryModel.MealTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                recipesQuery = recipesQuery
                    .Where(r => EF.Functions.Like(r.Title, wildCard) ||
                            EF.Functions.Like(r.Description, wildCard) ||
                            (r.RecipeTags != null && r.RecipeTags.Any(rt => EF.Functions.Like(rt.Tag.Name, wildCard))) ||
                            r.RecipesIngredients!.Any(ri => EF.Functions.Like(ri.Ingredient.Name, wildCard)));
            }

            if(queryModel.MaxPreparationTime.HasValue)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.TotalTimeMinutes <= queryModel.MaxPreparationTime.Value);
            }

            if (queryModel.DifficultyLevel.HasValue)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.DifficultyLevel == (DifficultyLevel)queryModel.DifficultyLevel.Value);
            }

            // Filtering by all tags (only recipes that have all the tags will be included in the result set)
            if (queryModel.SelectedTagIds != null && queryModel.SelectedTagIds.Any())
            {
                // If only one tag is applied => simple query for faster results
                if (queryModel.SelectedTagIds.Count == 1)
                {
                    var singleTagId = queryModel.SelectedTagIds.First();

                    recipesQuery = recipesQuery
                        .Where(r => r.RecipeTags
                            .Any(tag => tag.TagId == singleTagId));
                }
                else // TODO: introduce indexes for query optimization
                {
                    recipesQuery = recipesQuery
                   .Where(r => queryModel.SelectedTagIds
                       .All(tagId => r.RecipeTags
                           .Any(tag => tag.TagId == tagId)));
                }
               
            }

            // Check if sorting is applied and if it exists in sorting enum
            RecipeSorting recipeSorting = (queryModel.RecipeSorting.HasValue &&
                    Enum.IsDefined(typeof(RecipeSorting), queryModel.RecipeSorting)) ? 
                    (RecipeSorting)queryModel.RecipeSorting : 
                    RecipeSorting.Newest;

            
            recipesQuery = recipeSorting switch
            {
                RecipeSorting.Newest => recipesQuery
                    .OrderByDescending(r => r.CreatedOn)
                    .ThenBy(r => r.DifficultyLevel)
                    .ThenBy(r => r.TotalTimeMinutes)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.Oldest => recipesQuery
                    .OrderBy(r => r.CreatedOn)
                    .ThenBy(r => r.DifficultyLevel)
                    .ThenBy(r => r.TotalTimeMinutes)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.CookingTimeAscending => recipesQuery
                    .OrderBy(r => r.TotalTimeMinutes)
                    .ThenBy(r => r.DifficultyLevel)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.CookingTimeDescending => recipesQuery
                    .OrderByDescending(r => r.TotalTimeMinutes)
                    .ThenByDescending(r => r.DifficultyLevel)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.DifficultyLevelAscending => recipesQuery
                    .OrderBy(r => r.DifficultyLevel)
                    .ThenBy(r => r.TotalTimeMinutes)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.DifficultyLevelDescending => recipesQuery
                    .OrderByDescending(r => r.DifficultyLevel)
                    .ThenByDescending(r => r.TotalTimeMinutes)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                RecipeSorting.ViewsDescending => recipesQuery
                    .OrderByDescending(r => r.TotalTimeMinutes) // change to RecipeViews when available
                    .ThenBy(r => r.DifficultyLevel)
                    .ThenBy(r => r.Title)
                    .ThenBy(r => r.Id),
                _ => recipesQuery
                .OrderByDescending(r => r.CreatedOn)
                .ThenBy(r => r.DifficultyLevel)
                .ThenBy(r => r.TotalTimeMinutes)
                .ThenBy(r => r.Title)
                .ThenBy(r => r.Id),
            };

            if (queryModel.CurrentPage == default)
            {
                queryModel.CurrentPage = DefaultPage;
            }

            if (queryModel.RecipesPerPage == default)
            {
                queryModel.RecipesPerPage = DefaultRecipesPerPage;
            }

            queryModel.TotalResults = recipesQuery.Count();

            
            var recipes = await recipesQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.RecipesPerPage)
                .Take(queryModel.RecipesPerPage)
                .ToListAsync();

            if (recipes.Count == 0)
            {
                logger.LogError("No recipes found by these critearia.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }

            return recipes;
           
        }
        
        /// <inheritdoc/>
        public async Task<RecipeAllMineServiceModel> GetAllMineAsync()
        {
            var recipes = await recipeRepository
                .GetAllQuery()
                .Include(r => r.Category)
                .Where(r => r.OwnerId == userId || r.FavouriteRecipes.Any(fr => fr.UserId == userId))
                .Select(r => new
                {
                    RecipeId = r.Id.ToString(),
                    IsAddedByUser = r.OwnerId == userId,
                    IsLikedByUser = r.FavouriteRecipes.Any(fr => fr.UserId == userId)
                })
                .ToListAsync();

            var likedRecipeIds = recipes.Where(r => r.IsLikedByUser).Select(r => r.RecipeId.ToString()).ToList();
            var addedRecipeIds = recipes.Where(r => r.IsAddedByUser).Select(r => r.RecipeId.ToString()).ToList();


            RecipeAllMineServiceModel model = new RecipeAllMineServiceModel
            {
                LikedRecipeIds = likedRecipeIds,
                AddedRecipeIds = addedRecipeIds
            }; 

            return model;

        }

        /// <inheritdoc/>
        public async Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model)
        {
            ValidationResult result = await recipeValidator.ValidateRecipeFormModelAsync(model);
            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            Recipe recipe = MapFromModelToRecipe(model, new Recipe());
            recipe.RecipesIngredients = recipeIngredientService.CreateAll(model.RecipeIngredients);
            recipe.Steps = stepService.CreateAll(model.Steps);

            if (model.SelectedTagIds.Count > 0)
            {
                recipe.RecipeTags = recipeTagService.CreateAll(model.SelectedTagIds);
            }            

            try
            {
                string recipeId = await recipeRepository.AddAsync(recipe);
                return OperationResult<string>.Success(recipeId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Recipe creation failed. Error message: {ex.Message}. Error StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryEditRecipeAsync(RecipeEditFormModel model)
        {
            await recipeValidator.ValidateRecipeExistsAsync(model.Id);

            Recipe recipe = await recipeRepository
                .GetByIdQuery(model.Id) 
                .Include(r => r.RecipeTags)
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient) 
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Measure)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Specification)
                .FirstAsync();
            
            recipeValidator.ValidateUserIsRecipeOwner(recipe.OwnerId); // UnauthorizedUserExc

            ValidationResult result = await recipeValidator.ValidateRecipeFormModelAsync(model); // no exception

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            recipe = MapFromModelToRecipe(model, recipe);
            recipe.RecipesIngredients = await recipeIngredientService.UpdateAll(recipe.Id, model.RecipeIngredients);
            recipe.Steps = await stepService.UpdateAll(recipe.Id, model.Steps);
            recipe.RecipeTags = await recipeTagService.UpdateAll(recipe.Id, model.SelectedTagIds);

            try
            {
                await recipeRepository.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                logger.LogError($"Recipe editting failed. Recipe Id: {recipe.Id}. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsServiceModel> GetForDetailsByIdAsync(Guid id)
        {
            var recipe = await recipeRepository.GetByIdQuery(id)
                .Include(r => r.Owner)
                .Include(r => r.Steps)
                .Include(r => r.Category)
                .Include(r => r.Meals)
                .Include(r => r.FavouriteRecipes)
                .Include(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync();

            if(recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            RecipeDetailsServiceModel? serviceModel = new RecipeDetailsServiceModel
            {
                Recipe = recipe,
                LikesCount = recipe.FavouriteRecipes.Count,
                CookedCount = recipe.Meals.Count,
                IsLikedByUser = recipe.FavouriteRecipes.Any(fr => fr.UserId == userId),
            };
            
            if (!isAdmin) // Increase recipe total views if user is not an admin
            {
                recipe.Views += 1;
                await recipeRepository.SaveChangesAsync();
            }

            return serviceModel;
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(Guid id)
        {
            await recipeValidator.ValidateRecipeExistsAsync(id);

            Recipe recipeToDelete = await recipeRepository
                .GetByIdQuery(id)
                .FirstAsync();
            
            recipeValidator.ValidateUserIsRecipeOwner(recipeToDelete.OwnerId); // UnauthorizedUserExc
            
            bool isIncluded = await recipeValidator.CanRecipeBeDeletedAsync(id);

            if (isIncluded)
            {
                logger.LogError($"Invalid operation while trying to delete recipe with id {id}. Recipe is included in active mealplans and cannot be deleted.");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage);
            }

            await SoftDeleteRecipeAsync(recipeToDelete);           
        }
        
       
        /// <inheritdoc/>
        public async Task<Recipe> GetForEditByIdAsync(Guid id)
        {
            await recipeValidator.ValidateRecipeExistsAsync(id);
            
            Recipe recipe = await recipeRepository.GetByIdQuery(id)
                .Include(r => r.Steps)
                .Include(r => r.RecipeTags)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Measure)                
                .Include(ri => ri.RecipesIngredients)
                    .ThenInclude(ri => ri.Specification)
                .FirstAsync();

            
            recipeValidator.ValidateUserIsRecipeOwner(recipe.OwnerId);

            return recipe;
        }
       
        /// <inheritdoc/>
        public async Task<int?> GetAllCountAsync()
        {
            return await recipeRepository
                .GetAllQuery()
                .CountAsync();
        }
       
        /// <inheritdoc/>
        public async Task<Recipe> GetForMealByIdAsync(Guid recipeId)
        {
            await recipeValidator.ValidateRecipeExistsAsync(recipeId);

            Recipe recipe =  await recipeRepository.GetByIdQuery(recipeId)
                .Include(r => r.Category)
                .FirstAsync();
            
            return recipe;
        }

        /// <inheritdoc/>
        public async Task<ICollection<Recipe>> GetAllByIds(ICollection<string> recipeIds)
        {
            var guidRecipeIds = recipeIds.Select(id => Guid.Parse(id)).ToList(); // Convert strings to Guids

            var recipes = await recipeRepository.GetAllQuery()
                .Include(r => r.Category)
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
                recipe.TotalTimeMinutes = model.CookingTimeMinutes!.Value;
                recipe.ImageUrl = model.ImageUrl;
                recipe.DifficultyLevel = model.DifficultyLevelId.HasValue ? (DifficultyLevel)model.DifficultyLevelId : null;
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

        
    }
}
