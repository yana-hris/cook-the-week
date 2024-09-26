namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Data;

    using Microsoft.EntityFrameworkCore;

    using Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Interfaces;
    using Web.ViewModels.Meal;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Web.ViewModels.RecipeIngredient;
    using Web.ViewModels.Step;

    using static Common.ExceptionMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;
    using CookTheWeek.Common.HelperMethods;

    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IIngredientService ingredientService;
        private readonly IUserRepository userRepository;
        private readonly IMealService mealService;
        private readonly IRecipeRepository recipeRepository;
        private readonly IStepRepository stepRepository;
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository;
        private readonly IMealRepository mealRepository;

        public RecipeService(CookTheWeekDbContext dbContext,
            IngredientService ingredientService,
            IRecipeRepository recipeRepository,
            IStepRepository stepRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IFavouriteRecipeRepository favouriteRecipeRepository,
            IMealService mealService,
            IUserRepository userRepository,
            IMealRepository mealRepository)
        {
            this.dbContext = dbContext;
            this.ingredientService = ingredientService;
            this.recipeRepository = recipeRepository;
            this.stepRepository = stepRepository;
            this.mealService = mealService;
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
            this.mealRepository = mealRepository;
            this.userRepository = userRepository;
        }


        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters (if any)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="userId"></param>
        /// <returns>A collection of RecipeAllViewModel</returns>
        /// <exception cref="RecordNotFoundException">Thrown if no recipes exist in the database</exception>
        /// <exception cref="DataRetrievalException">Thrown when a database Exception occurs</exception>
        public async Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId)
        {
            try
            {
                IQueryable<Recipe> recipesQuery = recipeRepository.GetAllQuery();

                if (!recipesQuery.Any())
                {
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

                return model;
            }
            catch (RecordNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataRetrievalException(DataRetrievalExceptionMessages.RecipeDataRetrievalExceptionMessage, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<string> AddAsync(RecipeAddFormModel model, string userId, bool isAdmin)
        {

            Recipe recipe = MapNonCollectionPropertiesToRecipe(model, userId, isAdmin);
            await AddOrUpdateIngredients(model, model.RecipeIngredients);
            await AddOrUpdateSteps(model, model.Steps);

            string recipeId = await recipeRepository.AddAsync(recipe);
            return recipeId.ToLower();
        }

        /// <inheritdoc/>
        public async Task EditAsync(RecipeEditFormModel model)
        {
            // Load the recipe including related entities
            Recipe recipe = await recipeRepository.GetByIdAsync(model.Id);

            await recipeRepository.UpdateAsync(recipe);
            MapRecipeModelToRecipe(model, recipe);
            await AddOrUpdateSteps(model, model.Steps);
        }

        /// <inheritdoc/>
        public async Task<RecipeDetailsViewModel> DetailsByIdAsync(string id)
        {

            Recipe recipe = await recipeRepository.GetByIdAsync(id);

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
                DiaryMeatSeafood = MapIngredientsByCategory(recipe, DiaryMeatSeafoodIngredientCategories),
                Produce = MapIngredientsByCategory(recipe, ProduceIngredientCategories),
                Legumes = MapIngredientsByCategory(recipe, LegumesIngredientCategories),
                PastaGrainsBakery = MapIngredientsByCategory(recipe, PastaGrainsBakeryIngredientCategories),
                OilsHerbsSpicesSweeteners = MapIngredientsByCategory(recipe, OilsHerbsSpicesSweetenersIngredientCategories),
                NutsSeedsAndOthers = MapIngredientsByCategory(recipe, NutsSeedsAndOthersIngredientCategories)
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(string id, string userId, bool isAdmin)
        {
            Recipe recipeToDelete = await recipeRepository.GetByIdAsync(id);

            if (!GuidHelper.CompareGuidStringWithGuid(userId, recipeToDelete.OwnerId) && !isAdmin)
            {
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeDeleteAuthorizationMessage);
            }

            bool hasMealPlans = await IsIncludedInMealPlansAsync(recipeToDelete.Id.ToString());

            if (hasMealPlans)
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidRecipeOperationDueToMealPlansInclusionExceptionMessage);
            }

            // SOFT Delete
            await recipeRepository.Delete(recipeToDelete);

            // Delete all relevant recipe Steps, Ingredients, Likes and Meals 
            await stepRepository.DeleteAllByRecipeIdAsync(id);
            await recipeIngredientRepository.DeleteAllByRecipeIdAsync(id);

            if (recipeToDelete.FavouriteRecipes.Any())
            {
                await favouriteRecipeRepository.DeleteAllByRecipeIdAsync(id);
            }

            if (recipeToDelete.Meals.Any())
            {
                await mealService.DeleteAllByRecipeIdAsync(id);
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
                throw new UnauthorizedUserException(UnauthorizedExceptionMessages.RecipeEditAuthorizationExceptionMessage);
            }

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
        public async Task<ICollection<RecipeAllViewModel>> AllAddedByUserIdAsync(string userId)
        {
            var recipes = await recipeRepository
                .GetAllQuery()
                .Where(r => GuidHelper.CompareGuidStringWithGuid(userId, r.OwnerId))
                .ToListAsync();

            if (!recipes.Any())
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoRecipesFoundExceptionMessage, null);
            }

            try
            {
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
            catch (RecordNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataRetrievalException(DataRetrievalExceptionMessages.RecipeDataRetrievalExceptionMessage, ex);
            }

        }

        /// <inheritdoc/>
        public async Task<bool> IsIncludedInMealPlansAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .AnyAsync();
        }

        /// <inheritdoc/>
        public async Task<int?> AllCountAsync()
        {
            return await recipeRepository
                .GetAllQuery()
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task<int?> MineCountAsync(string userId)
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
        public async Task<ICollection<RecipeAllViewModel>> AllSiteAsync()
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
        public async Task<ICollection<RecipeAllViewModel>> AllUserRecipesAsync()
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
            return await favouriteRecipeRepository.AllCountByRecipeIdAsync(recipeId);
        }

        /// <inheritdoc/>
        public async Task<ICollection<RecipeAllViewModel>> AllLikedByUserAsync(string userId)
        {
            ICollection<FavouriteRecipe> likedRecipes = await
                favouriteRecipeRepository.GetAllByUserIdAsync(userId);

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
        public async Task ToggleLike(string userId, string recipeId)
        {
            bool exists = await this.recipeRepository.ExistsByIdAsync(recipeId);
            var currentUserId = this.userRepository.GetCurrentUserId();

            if (!exists)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }

            if (currentUserId != null && !String.IsNullOrEmpty(userId) && 
                GuidHelper.CompareTwoGuidStrings(currentUserId, userId))
            {

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

        // Helper methods for improved code reusability
        private Recipe MapNonCollectionPropertiesToRecipe(RecipeAddFormModel model, string ownerId, bool isAdmin)
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

        private async Task AddOrUpdateSteps(IRecipeFormModel recipe, IEnumerable<StepFormModel> steps)
        {

            ICollection<Step> newSteps = steps
                .Select(s => new Step()
                {
                    Description = s.Description,
                })
                .ToList();

            if (recipe is RecipeAddFormModel)
            {
                await stepRepository.AddAllAsync(newSteps);
            }
            else if (recipe is RecipeEditFormModel recipeEditFormModel)
            {
                await stepRepository.UpdateAllByRecipeIdAsync(recipeEditFormModel.Id, newSteps);
            }
            else
            {
                throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
            }
        }

        private async Task AddOrUpdateIngredients(IRecipeFormModel recipe, IEnumerable<RecipeIngredientFormModel> newIngredients)
        {
            ICollection<RecipeIngredient> validRecipeIngredients = new List<RecipeIngredient>();

            foreach (var ingredient in newIngredients)
            {
                bool isValidIngredient = await ingredientService.ExistsByIdAsync(ingredient.IngredientId);

                // Complex logic allowing the user to add ingredients with the same name but different measure or specification type
                if (isValidIngredient)
                {
                    if (CheckAndUpdateAnExistingIngredient(validRecipeIngredients, ingredient))
                    {
                        continue;
                    }

                    // Otherwise, create a new ingredient
                    validRecipeIngredients.Add(CreateNewRecipeIngredient(ingredient, isValidIngredient.Value));

                }
                else
                {
                    // such ingredient does not exist in DB => to do => server-side error 
                    throw new RecordNotFoundException(RecordNotFoundExceptionMessages.IngredientNotFoundExceptionMessage, null);
                }
            }

            if (recipe is RecipeEditFormModel existingRecipe)
            {
                await recipeIngredientRepository.UpdateAllByRecipeIdAsync(existingRecipe.Id, validRecipeIngredients);
            }
            else if (recipe is RecipeAddFormModel)
            {
                //TODO: implement a service in recipeIngredientService for adding recipe-ingredients instead of using repository
                await recipeIngredientRepository.AddAllAsync(validRecipeIngredients);
            }
            else
            {
                throw new InvalidCastException(InvalidCastExceptionMessages.RecipeAddOrEditModelUnsuccessfullyCasted);
            }
        }

        // Create new RecipeIngredient
        private static RecipeIngredient CreateNewRecipeIngredient(RecipeIngredientFormModel ingredient, int ingredientId)
        {
            return new RecipeIngredient
            {
                IngredientId = ingredientId,
                Qty = ingredient.Qty.GetDecimalQtyValue(),
                MeasureId = ingredient.MeasureId!.Value,
                SpecificationId = ingredient.SpecificationId
            };
        }

        // Check if an ingredient with the same characteristics is already existing to update QTY
        private static bool CheckAndUpdateAnExistingIngredient(ICollection<RecipeIngredient> validRecipeIngredients,
                                                             RecipeIngredientFormModel ingredient)
        {
            // Check if the ingredient is already added with the same measure and specification
            var existingIngredient = validRecipeIngredients
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

        // Mapping non collection properties from viewModel to Recipe entity
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


        // Helper method to map ingredients by category in Recipe Details View
        private static List<RecipeIngredientDetailsViewModel> MapIngredientsByCategory(Recipe recipe, IEnumerable<int> ingredientCategoryIds)
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
