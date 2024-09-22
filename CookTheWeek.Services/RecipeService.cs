namespace CookTheWeek.Services.Data
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
    using Web.ViewModels.Meal;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Web.ViewModels.RecipeIngredient;
    using Web.ViewModels.Step;

    using static Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;
    using static Common.ExceptionMessagesConstants.DataRetrievalExceptionMessages;
    using static Common.ExceptionMessagesConstants.UnauthorizedExceptionMessages;
    using static Common.ExceptionMessagesConstants.InvalidCastExceptionMessages;
    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;
    using CookTheWeek.Web.ViewModels.Interfaces;

    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IIngredientService ingredientService;
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
        }


        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException">Rethrown if a record is not found in the database</exception>
        /// <exception cref="DataRetrievalException">Thrown when a database Exception occurs</exception>
        public async Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId)
        {
            try
            {
                IQueryable<Recipe> recipesQuery = this.recipeRepository.GetAllQuery();

                if (userId != String.Empty)
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
                        OwnerId = r.OwnerId.ToString().ToLower(),
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
                throw new DataRetrievalException(RecipeDataRetrievalExceptionMessage, ex);
            }
        }
       

        /// <inheritdoc/>
        public async Task<string> AddAsync(RecipeAddFormModel model, string userId, bool isAdmin)
        {
            
            Recipe recipe = MapNonCollectionPropertiesToRecipe(model, userId, isAdmin);
            await AddOrUpdateIngredients(model, model.RecipeIngredients);
            await AddOrUpdateSteps(model, model.Steps);

            string recipeId = await this.recipeRepository.AddAsync(recipe);
            return recipeId.ToLower();
        }

        /// <inheritdoc/>
        public async Task EditAsync(RecipeEditFormModel model)
        {
            // Load the recipe including related entities
            Recipe recipe = await this.recipeRepository.GetByIdAsync(model.Id);

            await this.recipeRepository.UpdateAsync(recipe);
            MapRecipeModelToRecipe(model, recipe);
            await AddOrUpdateSteps(model, model.Steps);
        }        


        public async Task<RecipeDetailsViewModel> DetailsByIdAsync(string id)
        {
            
            Recipe recipe = await this.recipeRepository.GetByIdAsync(id);

            
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
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext
                .Recipes
                .AsNoTracking()
                .Where(r => r.Id.ToString().ToLower() == id.ToLower())
                .AnyAsync();
        }
        public async Task DeleteByIdAsync(string id, string userId, bool isAdmin)
        {
            Recipe recipeToDelete = await this.recipeRepository.GetByIdAsync(id);

            if (recipeToDelete.OwnerId.ToString().ToLower() != userId && !isAdmin)
            {
                throw new UnauthorizedUserException(RecipeDeleteAuthorizationMessage);
            }

            // SOFT Delete
            recipeToDelete.IsDeleted = true;

            // Delete all relevant recipe Steps, Ingredients, Likes and Meals 
            await this.stepRepository.DeleteAllAsync(id);
            await this.recipeIngredientRepository.DeleteAllAsync(id);

            if(recipeToDelete.FavouriteRecipes.Any())
            {
                await this.favouriteRecipeRepository.DeleteAllByRecipeIdAsync(id);
            }
           
            if(recipeToDelete.Meals.Any()) 
            {
                await this.mealRepository.DeleteAllByRecipeIdAsync(id);
            }

        }
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin)
        {
            Recipe recipe = await this.recipeRepository.GetByIdAsync(id);            
           
            if (userId != recipe.OwnerId.ToString().ToLower() && !isAdmin)
            {
                throw new UnauthorizedUserException(RecipeEditAuthorizationExceptionMessage);
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
                    Name = ri.Ingredient.Name,
                    Qty = RecipeIngredientQtyFormModel.ConvertFromDecimalQty(ri.Qty, ri.Measure.Name),
                    MeasureId = ri.MeasureId,
                    SpecificationId = ri.SpecificationId
                }).ToList()
            };

            return model;
        }
        
        public async Task<ICollection<RecipeAllViewModel>> AllAddedByUserIdAsync(string userId)
        {

            var recipes = await this.recipeRepository.GetAllByUserIdAsync(userId);

            if (recipes == null || !recipes.Any())
            {
                throw new RecordNotFoundException(NoRecipesFoundExceptionMessage, null);
            }

            try
            {
                ICollection<RecipeAllViewModel> model = recipes.Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    OwnerId = r.OwnerId.ToString().ToLower(),
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
            catch (Exception ex)
            {
                throw new DataRetrievalException("An error occurred while retrieving recipes.", ex);
            }
                
        }
        
        public async Task<bool> IsIncludedInMealPlans(string id)
        {
            return await this.dbContext
                .Meals
                .Where(m => m.Id.ToString() == id && m.IsCooked == false)
                .AnyAsync();
        }
        public int? AllCountAsync()
        {
            return this.recipeRepository
                .GetAllQuery()
                .Count();
        }
        public int? MineCountAsync(string userId)
        {
            return this.recipeRepository
                .GetAllQuery()
                .Where(r => r.OwnerId.ToString().ToLower() == userId)
                .Count();

        }
        public Task<MealAddFormModel> GetForMealByIdAsync(string recipeId)
        {
            return this.dbContext
                .Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Where(r => r.Id.ToString() == recipeId)
                .Select(r => new MealAddFormModel()
                {
                    RecipeId = r.Id.ToString(),
                    Title = r.Title,
                    Servings = r.Servings,
                    ImageUrl = r.ImageUrl,
                    CategoryName = r.Category.Name,
                    Date = DateTime.Now.ToString(MealDateFormat),
                })
                .FirstAsync();
        }

        public async Task<ICollection<RecipeAllViewModel>> AllSiteAsync()
        {
            List<RecipeAllViewModel> siteRecipes = await this.dbContext.Recipes
                    .Where(r => r.IsSiteRecipe)
                    .Select(r => new RecipeAllViewModel()
                    {
                        Id = r.Id.ToString(),
                        OwnerId = r.OwnerId.ToString().ToLower(),
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
        public async Task<ICollection<RecipeAllViewModel>> AllUserRecipesAsync()
        {
            ICollection<RecipeAllViewModel> allUserRecipes = await this.dbContext
                .Recipes
                .Where(r => !r.IsSiteRecipe)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    OwnerId = r.OwnerId.ToString().ToLower(),
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
                await this.stepRepository.AddAllAsync(newSteps);
            }
            else if (recipe is RecipeEditFormModel recipeEditFormModel)
            {
                await this.stepRepository.UpdateAllAsync(recipeEditFormModel.Id, newSteps);
            }
            else
            {
                throw new InvalidCastException(RecipeAddOrEditModelUnsuccessfullyCasted);
            }
        }

        private async Task AddOrUpdateIngredients(IRecipeFormModel recipe, IEnumerable<RecipeIngredientFormModel> newIngredients)
        {
            
            ICollection<RecipeIngredient> validRecipeIngredients = new List<RecipeIngredient>();

            foreach (var ingredient in newIngredients)
            {
                var ingredientId = await this.ingredientService.GetIdByNameAsync(ingredient.Name);

                // Complex logic allowing the user to add ingredients with the same name but different measure or specification type
                if (ingredientId != null)
                {
                    if (CheckAndUpdateAnExistingIngredient(validRecipeIngredients, ingredient, ingredientId.Value))
                    {
                        continue;
                    }

                    // Otherwise, create a new ingredient
                    validRecipeIngredients.Add(CreateNewRecipeIngredient(ingredient, ingredientId.Value));

                }
                else
                {
                    // such ingredient does not exist in DB => to do => server-side error 
                    throw new RecordNotFoundException(IngredientNotFoundExceptionMessage, null);
                }
            }

            if (recipe is RecipeEditFormModel existingRecipe)
            {
                await this.recipeIngredientRepository.UpdateAllAsync(existingRecipe.Id, validRecipeIngredients);
            }
            else if(recipe is RecipeAddFormModel)
            {
                await this.recipeIngredientRepository.AddAllAsync(validRecipeIngredients);
            }
            else
            {
                throw new InvalidCastException(RecipeAddOrEditModelUnsuccessfullyCasted);
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
                                                             RecipeIngredientFormModel ingredient,
                                                             int ingredientId)
        {
            // Check if the ingredient is already added with the same measure and specification
            var existingIngredient = validRecipeIngredients
                .FirstOrDefault(ri => ri.IngredientId == ingredientId &&
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

            throw new InvalidCastException(RecipeAddOrEditModelUnsuccessfullyCasted);
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

        public Task<bool> IsLikedByUserAsync(string userId, string recipeId)
        {
            return this.favouriteRecipeRepository.GetByIdAsync(userId, recipeId);
        }

        public async Task<int?> GetAllRecipeLikesAsync(string recipeId)
        {
            return await this.favouriteRecipeRepository.AllCountByRecipeIdAsync(recipeId);
        }

        public async Task<ICollection<RecipeAllViewModel>> AllLikedByUserAsync(string userId)
        {
            ICollection<FavouriteRecipe> likedRecipes = await
                this.favouriteRecipeRepository.GetAllByUserIdAsync(userId);

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

        public async Task<int?> GetAllRecipeMealsCountAsync(string recipeId)
        {
            return await this.mealRepository.GetAllCountByRecipeIdAsync(recipeId);
        }
    }
}
