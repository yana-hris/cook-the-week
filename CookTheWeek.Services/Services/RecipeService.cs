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
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Step;

    using static Common.ExceptionMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;
    using Azure;

    public class RecipeService : IRecipeService
    {
        
        private readonly IRecipeRepository recipeRepository;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IMealRepository mealRepository;

        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IIngredientService ingredientService;
        private readonly IMealService mealService;
        private readonly IStepService stepService;

        private readonly IUserService userService;
        private readonly IValidationService validationService;
        private readonly ILogger<RecipeService> logger;

        public RecipeService(IngredientService ingredientService,
            IRecipeRepository recipeRepository,
            IMealRepository mealRepository,
            IStepService stepService,
            IRecipeIngredientService recipeIngredientService,
            IRecipeIngredientRepository recipeIngredientRepository,
            IFavouriteRecipeRepository favouriteRecipeRepository,
            IMealService mealService,
            IUserService userService,
            ILogger<RecipeService> logger,
            IValidationService validationService)
        {
            this.recipeRepository = recipeRepository;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.mealRepository = mealRepository;

            this.recipeIngredientService = recipeIngredientService;
            this.stepService = stepService;
            this.userService = userService;
            this.ingredientService = ingredientService;

            this.mealService = mealService;
            this.validationService = validationService;
            this.logger = logger;
        }


        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters (if any)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="userId"></param>
        /// <returns>A collection of RecipeAllViewModel</returns>
        /// <exception cref="RecordNotFoundException">Thrown if no recipes exist in the database</exception>
        public async Task<ICollection<RecipeAllViewModel>> GetAllAsync(AllRecipesQueryModel queryModel, string userId)
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

            if (queryModel.TotalRecipes == 0)
            {
                //TODO: Show no recipes message by throwing an exception (and redirect in the controller)
            }

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
        public async Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model, string userId, bool isAdmin)
        {
            ValidationResult result = await validationService.ValidateRecipeWithIngredientsAsync(model);
            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            Recipe recipe = MapNonCollectionPropertiesFromModelToRecipe(model, userId, isAdmin);
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
            
            MapRecipeModelToRecipe(model, recipe);
            await ProcessRecipeIngredientsAsync(model);
            await ProcessRecipeStepsAsync(model.Id, model);

            await recipeRepository.UpdateAsync(recipe);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsViewModel> TryGetForDetailsByRecipeId(string id)
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
        public async Task DeleteByIdAsync(string id, string userId, bool isAdmin)
        {
            try
            {
                Recipe recipeToDelete = await recipeRepository.GetByIdAsync(id);

                if (!GuidHelper.CompareGuidStringWithGuid(userId, recipeToDelete.OwnerId) && !isAdmin)
                {
                    logger.LogError($"Unauthorized access attempt: User {userId} tried to delete a recipe {id} but does not have the necessary permissions.");
                    throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeDeleteAuthorizationMessage);
                }

                bool hasMealPlans = await HasMealPlansAsync(recipeToDelete.Id.ToString());

                if (hasMealPlans)
                {
                    logger.LogError($"Invalid operation while trying to delete recipe with id {id}. Recipe is included in mealplans and cannot be deleted.");
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage);
                }

                // SOFT Delete
                await recipeRepository.Delete(recipeToDelete);

                // Delete all relevant recipe Steps, Ingredients, Likes and Meals 
                await stepService.DeleteByRecipeIdAsync(id);
                await recipeIngredientService.DeleteByRecipeIdAsync(id);

                if (recipeToDelete.FavouriteRecipes.Any())
                {
                    await DeleteRecipeLikesByRecipeIdAsync(id);
                }

                if (recipeToDelete.Meals.Any())
                {
                    await mealService.DeleteByRecipeIdAsync(id);
                }
            }
            catch (RecordNotFoundException)
            {
                logger.LogError($"Record not found: {nameof(Recipe)} with ID {id} was not found.");
                throw;
            }
        }

        
        /// <inheritdoc/>
        public async Task DeleteAllByUserIdAsync(string userId)
        {
            await recipeRepository.DeleteAllByOwnerIdAsync(userId);
        }

        /// <inheritdoc/>
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin)
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
        public async Task<ICollection<RecipeAllViewModel>> GetAllAddedByUserIdAsync(string userId)
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
        public async Task<bool> HasMealPlansAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
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
        public async Task<int?> GetMineCountAsync(string userId)
        {
            return await recipeRepository
                .GetAllQuery()
                .Where(r => GuidHelper.CompareGuidStringWithGuid(userId, r.OwnerId))
                .CountAsync();

        }


        // CreateMealViewModel ??
        public async Task<MealAddFormModel> GetForMealByIdAsync(string recipeId)
        {
            Recipe recipe = await recipeRepository.GetByIdAsync(recipeId);


            MealAddFormModel model = new MealAddFormModel()
            {
                RecipeId = recipe.Id.ToString(),
                Title = recipe.Title,
                Servings = recipe.Servings,
                ImageUrl = recipe.ImageUrl,
                CategoryName = recipe.Category.Name,
                Date = DateTime.Now.ToString(MealDateFormat),
            };

            return model;
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
        public Task<bool> IsLikedByUserAsync(string userId, string recipeId)
        {
            return favouriteRecipeRepository.GetByIdAsync(userId, recipeId);
        }

        /// <inheritdoc/>
        public async Task<int?> GetAllRecipeLikesAsync(string recipeId)
        {
            return await favouriteRecipeRepository.GetAllCountByRecipeIdAsync(recipeId);
        }

        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> GetAllLikedByUserIdAsync(string userId)
        {
            ICollection<FavouriteRecipe> likedRecipes = await
                favouriteRecipeRepository.GetAllByUserIdAsync(userId);

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
        public async Task<int?> GetAllRecipeMealsCountAsync(string recipeId)
        {
            return await mealRepository
                .GetAllQuery()
                .Select(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task LikeOrUnlikeRecipeByUserIdAsync(string userId, string recipeId)
        {
            bool exists = await this.recipeRepository.ExistsByIdAsync(recipeId);
            var currentUserId = this.userRepository.GetCurrentUserId();

            if (!exists)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            if (!String.IsNullOrEmpty(currentUserId) && !String.IsNullOrEmpty(userId) && 
                GuidHelper.CompareTwoGuidStrings(currentUserId, userId))
            {
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.UserNotLoggedInExceptionMessage);
            }

            bool isAlreadyAdded = await this.favouriteRecipeRepository.GetByIdAsync(userId, recipeId);

            if (isAlreadyAdded)
            {
                await this.favouriteRecipeRepository.DeleteAsync(userId, recipeId);
            }
            else
            {
                await this.favouriteRecipeRepository.AddAsync(userId, recipeId);
            }
        }

        // PRIVATE HELPER METHODS        

        /// <summary>
        /// Helper method that deletes the likes of a recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task DeleteRecipeLikesByRecipeIdAsync(string id)
        {
            await favouriteRecipeRepository.DeleteAllByRecipeIdAsync(id);
        }

        /// <summary>
        /// Map a recipe form model to Recipe entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ownerId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Recipe</returns>
        // TODO: Consider using Automapper
        private static Recipe MapNonCollectionPropertiesFromModelToRecipe(IRecipeFormModel model, string ownerId, bool isAdmin)
        {
            return new Recipe
            {
                Title = model.Title,
                OwnerId = Guid.Parse(ownerId),
                Description = model.Description,
                Servings = model.Servings!.Value,
                TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes!.Value),
                ImageUrl = model.ImageUrl,
                CategoryId = model.RecipeCategoryId!.Value,
                IsSiteRecipe = isAdmin
            };
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
               if (UpdateAlreadyExistingRecipeIngredient(recipeIngredients, currentRecipeIngredient))
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
        private static bool UpdateAlreadyExistingRecipeIngredient(ICollection<RecipeIngredient> alreadyAdded,
                                                             RecipeIngredientFormModel ingredient)
        {
            // Check if the ingredient is already added with the same measure and specification
            var existingIngredient = alreadyAdded
                .FirstOrDefault(ri => ri.IngredientId == ingredient.IngredientId &&
                                      ri.MeasureId == ingredient.MeasureId &&
                                      (ri.SpecificationId == ingredient.SpecificationId ||
                                      ri.SpecificationId == null && ingredient.SpecificationId == null));

            // Update the quantity if found
            if (existingIngredient != null)
            {
                existingIngredient.Qty += ingredient.Qty.GetDecimalQtyValue();
                return true;
            }

            return false;
        }

        
        // TODO: Consider using Automapper
        /// <summary>
        /// Maps a form model to a Recipe entity or throws an exception in cacse of invalid model type
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recipe"></param>
        /// <returns>static method</returns>
        /// <exception cref="InvalidCastException"></exception>
        private static Recipe MapRecipeModelToRecipe(IRecipeFormModel model, Recipe recipe)
        {
            if (model is RecipeAddFormModel || model is RecipeEditFormModel)
            {
                recipe.Title = model.Title;
                recipe.Description = model.Description;
                recipe.Servings = model.Servings!.Value;
                recipe.TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes!.Value);
                recipe.ImageUrl = model.ImageUrl;
                recipe.CategoryId = model.RecipeCategoryId!.Value;

                return recipe;
            }

            throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
        }


        /// <summary>
        /// Maps ingredients to a category and creates a collection view model for Recipe Edit View
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
