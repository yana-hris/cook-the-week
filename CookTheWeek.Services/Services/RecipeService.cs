namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Events.EventHandlers;
    using CookTheWeek.Services.Data.Events.Dispatchers;
    using CookTheWeek.Services.Data.Events;
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
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly IStepService stepService;

        private readonly IRecipeSoftDeletedEventHandler recipeSoftDeletedEventHandler;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        private readonly IValidationService validationService;
        private readonly ILogger<RecipeService> logger;
        private readonly string? userId;
        private readonly bool isAdmin;

        public RecipeService(IRecipeRepository recipeRepository,
            IStepService stepService,
            IRecipeIngredientService recipeIngredientService,
            IFavouriteRecipeService favouriteRecipeService,
            ILogger<RecipeService> logger,
            IUserContext userContext,
            IRecipeSoftDeletedEventHandler recipeSoftDeletedEventHandler,
            IDomainEventDispatcher domainEventDispatcher,
            IValidationService validationService)
        {
            this.recipeRepository = recipeRepository;

            this.favouriteRecipeService = favouriteRecipeService;
            this.recipeIngredientService = recipeIngredientService;
            this.stepService = stepService;

            this.validationService = validationService;
            this.recipeSoftDeletedEventHandler = recipeSoftDeletedEventHandler;
            this.domainEventDispatcher = domainEventDispatcher;
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


            if (userId != string.Empty)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.OwnerId.ToString() == userId || r.IsSiteRecipe);
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
                    Id = r.Id.ToString(),
                    OwnerId = r.OwnerId.ToString(),
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
            ValidationResult result = await validationService.ValidateRecipeWithIngredientsAsync(model);
            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            Recipe recipe = MapFromModelToRecipe(model, new Recipe());
            string recipeId = await recipeRepository.AddAsync(recipe);
            await ProcessRecipeIngredientsAsync(model);
            await ProcessRecipeStepsAsync(recipeId, model);
            
            return OperationResult<string>.Success(recipeId);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryEditRecipeAsync(RecipeEditFormModel model)
        {
            ValidationResult result = await validationService.ValidateRecipeWithIngredientsAsync(model);

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }
            
            Recipe recipe = await recipeRepository.GetByIdAsync(model.Id);

            recipe = MapFromModelToRecipe(model, recipe);
            await ProcessRecipeIngredientsAsync(model);
            await ProcessRecipeStepsAsync(model.Id, model);

            await recipeRepository.UpdateAsync(recipe);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsViewModel> TryGetModelForDetailsById(string id)
        {

            Recipe recipe = await recipeRepository.GetByIdAsync(id);

            // TODO: Consider using Automapper
            RecipeDetailsViewModel model = new RecipeDetailsViewModel()
            {
                Id = recipe.Id.ToString(),
                Title = recipe.Title,
                Description = recipe.Description,
                Steps = recipe.Steps.Select(s => new StepViewModel()
                {
                    Id = s.Id,
                    Description = s.Description
                }).ToList(),
                Servings = recipe.Servings,
                IsSiteRecipe = recipe.IsSiteRecipe,
                TotalTime = FormatCookingTime(recipe.TotalTime),
                ImageUrl = recipe.ImageUrl,
                CreatedOn = recipe.CreatedOn.ToString("dd-MM-yyyy"),
                CreatedBy = recipe.Owner.UserName!,
                CategoryName = recipe.Category.Name,
                DiaryMeatSeafood = MapIngredientsByCategoryForDetailsView(recipe, DiaryMeatSeafoodIngredientCategories),
                Produce = MapIngredientsByCategoryForDetailsView(recipe, ProduceIngredientCategories),
                Legumes = MapIngredientsByCategoryForDetailsView(recipe, LegumesIngredientCategories),
                PastaGrainsBakery = MapIngredientsByCategoryForDetailsView(recipe, PastaGrainsBakeryIngredientCategories),
                OilsHerbsSpicesSweeteners = MapIngredientsByCategoryForDetailsView(recipe, OilsHerbsSpicesSweetenersIngredientCategories),
                NutsSeedsAndOthers = MapIngredientsByCategoryForDetailsView(recipe, NutsSeedsAndOthersIngredientCategories)
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(string id)
        {
            try
            {
                Recipe recipeToDelete = await recipeRepository.GetByIdAsync(id);

                if (!GuidHelper.CompareGuidStringWithGuid(userId, recipeToDelete.OwnerId) && !isAdmin)
                {
                    logger.LogError($"Unauthorized access attempt: User {userId} tried to delete a recipe {id} but does not have the necessary permissions.");
                    throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeDeleteAuthorizationMessage);
                }

                // TODO: move to validation service to decouple services
                bool isIncluded = await IsIncludedInMealPlansAsync(recipeToDelete.Id.ToString());

                if (isIncluded)
                {
                    logger.LogError($"Invalid operation while trying to delete recipe with id {id}. Recipe is included in active mealplans and cannot be deleted.");
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage);
                }

                await DeleteAsync(recipeToDelete);
            }
            catch (RecordNotFoundException)
            {
                logger.LogError($"Record not found: {nameof(Recipe)} with ID {id} was not found.");
                throw;
            }
        }
        
        /// <inheritdoc/>
        public async Task DeleteAllByUserIdAsync()
        {
            ICollection<Recipe> allUserRecipes = await recipeRepository.GetAllQuery()
                .Where(r => GuidHelper.CompareGuidStringWithGuid(userId, r.OwnerId))
                .ToListAsync();

            foreach (var recipe in allUserRecipes)
            {
                await DeleteAsync(recipe);
            }

        }

        /// <inheritdoc/>
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(string id)
        {
            Recipe recipe = await recipeRepository.GetByIdAsync(id);

            if (!GuidHelper.CompareGuidStringWithGuid(userId, recipe.OwnerId) && !isAdmin)
            {
                logger.LogError($"Unauthorized access attempt: User {userId} tried to edit Recipe with {id} but does not have the necessary permissions.");
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeEditAuthorizationExceptionMessage);
            }

            // TODO: Consider using Automapper
            RecipeEditFormModel model = new RecipeEditFormModel()
            {
                Id = recipe.Id.ToString(),
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
                .Where(r => GuidHelper.CompareGuidStringWithGuid(userId, r.OwnerId))
                .ToListAsync();
            
            // TODO: Consider using Automapper
            ICollection<RecipeAllViewModel> model = recipes.Select(r => new RecipeAllViewModel()
            {
                Id = r.Id.ToString(),
                OwnerId = r.OwnerId.ToString(),
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
        public async Task<bool> IsIncludedInMealPlansAsync(string recipeId)
        {
            // Chek if meals are included in query
            return await recipeRepository.GetAllQuery()
                .Where(r => r.Meals.Any(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId)))
                .AnyAsync();
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
                .Where(r => GuidHelper.CompareGuidStringWithGuid(userId, r.OwnerId))
                .CountAsync();

        }
       
        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllSiteRecipesAsync()
        {
            List<RecipeAllViewModel> siteRecipes = await recipeRepository.GetAllQuery()
                    .Where(r => r.IsSiteRecipe)
                    .Select(r => new RecipeAllViewModel()
                    {
                        Id = r.Id.ToString(),
                        OwnerId = r.OwnerId.ToString(),
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
                    Id = r.Id.ToString(),
                    OwnerId = r.OwnerId.ToString(),
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
        public async Task<ICollection<RecipeAllViewModel>> GetAllLikedByUserIdAsync()
        {
            ICollection<FavouriteRecipe> likedRecipes = await
                favouriteRecipeService.GetAllRecipesLikedByCurrentUserAsync();

            // TODO: Consider using Automapper
            var model = likedRecipes
                .Select(fr => new RecipeAllViewModel()
                {
                    Id = fr.Recipe.Id.ToString(),
                    ImageUrl = fr.Recipe.ImageUrl,
                    Title = fr.Recipe.Title,
                    Description = fr.Recipe.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = fr.Recipe.CategoryId,
                        Name = fr.Recipe.Category.Name
                    },
                    Servings = fr.Recipe.Servings,
                    CookingTime = FormatCookingTime(fr.Recipe.TotalTime)
                }).ToList();

            return model;
        }
        
        /// <inheritdoc/>
        public async Task<Recipe> GetForMealByIdAsync(string recipeId)
        {
            return await recipeRepository.GetByIdAsync(recipeId);
        }

        // PRIVATE HELPER METHODS        

        /// <summary>
        /// Helper method which implements the soft delete of a recipe. All related entities are deleted as well using an event dispatcher.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        private async Task DeleteAsync(Recipe recipe)
        {
            // SOFT DELETE
            recipe.OwnerId = Guid.Parse(DeletedUserId);
            recipe.IsDeleted = true;          

            await recipeRepository.UpdateAsync(recipe);

            // Dispatch the soft delete event
            var recipeSoftDeletedEvent = new RecipeSoftDeletedEvent(recipe.Id);
            await domainEventDispatcher.DispatchAsync(recipeSoftDeletedEvent);
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
                    if (userId == null)
                    {
                        logger.LogError($"Missing argument: {nameof(userId)} is null and model of type {model.GetType().Name} cannot be mapped to Entity {nameof(Recipe)} in method {nameof(MapFromModelToRecipe)}");
                        throw new ArgumentNullException(ArgumentNullExceptionMessages.UserNullExceptionMessage);
                    }

                    recipe.OwnerId = Guid.Parse(userId);
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
        private async Task ProcessRecipeStepsAsync(string recipeId, IRecipeFormModel model)
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

        /// <summary>
        ///  A helper method to Add or Edit a recipe`s recipe ingredients. Works with both Add and Edit Recipe Form Models. Throws an exception in case of unsucessful cast
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="ingredientsToAdd"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        private async Task ProcessRecipeIngredientsAsync(IRecipeFormModel model)
        {
            ICollection<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            
            foreach (var currentRecipeIngredient in model.RecipeIngredients)
            {
                // Complex logic allowing the user to add ingredients with the same name but different measure or specification type
               if (UpdateAlreadyExistingRecipeIngredientQty(recipeIngredients, currentRecipeIngredient))
                {
                    continue;
                }

                var newRecipeIngredient = await recipeIngredientService.CreateRecipeIngredientForAddRecipeAsync(currentRecipeIngredient);
                recipeIngredients.Add(newRecipeIngredient);
            }

            if (model is RecipeEditFormModel existingRecipe)
            {
                await this.recipeIngredientService.EditAsync(existingRecipe.Id, recipeIngredients);
            }
            else if (model is RecipeAddFormModel)
            {
                await recipeIngredientService.AddAsync(recipeIngredients);
            }
            else
            {
                logger.LogError($"Type cast error: Uable to cast {model.GetType().Name} to {nameof(RecipeAddFormModel)} or {nameof(RecipeEditFormModel)} in method {nameof(ProcessRecipeIngredientsAsync)}.");
                throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
            }
        }

        
        /// <summary>
        /// A helper method which checks for an existing ingredient id, already added to the recipe ingredients. If it finds one, it checks if the measure and specification are the same. If yes, updated the QTY.
        /// </summary>
        /// <remarks>The method processes the collection internally if the condition is met</remarks>
        /// <param name="alreadyAdded"></param>
        /// <param name="ingredient"></param>
        /// <returns>true or false</returns>
        private static bool UpdateAlreadyExistingRecipeIngredientQty(ICollection<RecipeIngredient> alreadyAdded,
                                                             RecipeIngredientFormModel ingredient)
        {
            // Check if the ingredient is already added with the same measure and specification
            var existingIngredient = alreadyAdded
                .FirstOrDefault(ri => ri.IngredientId == ingredient.IngredientId &&
                                      ri.MeasureId == ingredient.MeasureId &&
                                      (ri.SpecificationId == ingredient.SpecificationId ||
                                      (ri.SpecificationId == null && ingredient.SpecificationId == null)));

            // Update the quantity if found
            if (existingIngredient != null)
            {
                existingIngredient.Qty += ingredient.Qty.GetDecimalQtyValue();
                return true;
            }

            return false;
        }

        

        /// <summary>
        /// Maps ingredients to a category and creates a collection view model for Recipe Details View
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="ingredientCategoryIds"></param>
        /// <returns>A collection of RecipeIngredientDetailsViewModel by category</returns>
        private static List<RecipeIngredientDetailsViewModel> MapIngredientsByCategoryForDetailsView(Recipe recipe, 
            IEnumerable<int> ingredientCategoryIds)
        {
            return recipe.RecipesIngredients
                .OrderBy(ri => ri.Ingredient.CategoryId)
                .ThenBy(ri => ri.Ingredient.Name)
                .Where(ri => ingredientCategoryIds.Contains(ri.Ingredient.CategoryId))
                .Select(ri => new RecipeIngredientDetailsViewModel()
                {
                    Name = ri.Ingredient.Name,
                    Qty = ri.Qty,
                    Measure = ri.Measure.Name,
                    Specification = ri.Specification != null ? ri.Specification.Description : null,
                }).ToList();
        }

        
    }
}
