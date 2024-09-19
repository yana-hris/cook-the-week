﻿namespace CookTheWeek.Services.Data
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
    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;

    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeRepository recipeRepository;
        private readonly IStepRepository stepRepository;
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly IFavouriteRecipeRepository favouriteRecipeRepository
        
        public RecipeService(CookTheWeekDbContext dbContext, 
            IngredientService ingredientService,
            IRecipeRepository recipeRepository, 
            IStepRepository stepRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IFavouriteRecipeRepository favouriteRecipeRepository)
        {
            this.dbContext = dbContext;
            this.ingredientService = ingredientService;
            this.recipeRepository = recipeRepository;
            this.stepRepository = stepRepository;
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.favouriteRecipeRepository = favouriteRecipeRepository;
        }

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
        public async Task<string> AddAsync(RecipeAddFormModel model, string ownerId, bool isAdmin)
        {
            // TODO: refactor
            Recipe recipe = MapNonCollectionPropertiesToRecipe(model, ownerId, isAdmin);
            await AddOrUpdateSteps(null, model.Steps);
            await AddOrUpdateIngredients(null, model.RecipeIngredients);

            string recipeId = await this.recipeRepository.AddAsync(recipe);

            return recipeId.ToLower();
        }
        public async Task EditAsync(RecipeEditFormModel model)
        {
            // Load the recipe including related entities
            Recipe recipe = await this.recipeRepository.GetByIdAsync(model.Id);

            await this.recipeRepository.UpdateAsync(recipe);
            await MapRecipeModelToRecipe(model, recipe);
            await AddOrUpdateSteps(model.Id, model.Steps);
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
        public async Task DeleteByIdAsync(string id, string userId)
        {
            Recipe recipeToDelete = await this.recipeRepository.GetByIdAsync(id);

            // Implementing soft delete 
            recipeToDelete.IsDeleted = true;

            // Delete Steps of deleted recipe
            await this.stepRepository.DeleteAllAsync(id);

            // Delete RecipeIngredients of deleted recipe
            await this.recipeIngredientRepository.DeleteAllAsync(id);

            // Delete User Likes for Deleted Recipe
            if(recipeToDelete != null && recipeToDelete.FavouriteRecipes.Any())
            {
                await this.favouriteRecipeRepository.DeleteAllByRecipeIdAsync(id);
            }

            // Delete All Meals that contain the Recipe
            if(recipeToDelete != null && recipeToDelete.Meals.Any()) 
            {
                this.dbContext.Meals.RemoveRange(recipeToDelete.Meals);
            }

            await this.dbContext.SaveChangesAsync();
        }
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin)
        {
            Recipe recipe = await this.recipeRepository.GetByIdAsync(id);            
           
            if (userId != recipe.OwnerId.ToString().ToLower() && !isAdmin)
            {
                throw new UnauthorizedException(RecipeAuthorizationExceptionMessage);
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
        public async Task<int> AllCountAsync()
        {
            return await this.dbContext
               .Recipes
               .CountAsync();
        }
        public async Task<int> MineCountAsync(string userId)
        {
            return await this.dbContext
                .Recipes
                .Where(r => r.OwnerId.ToString() == userId)
                .CountAsync();
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

        private async Task AddOrUpdateSteps(string? recipeId, IEnumerable<StepFormModel> steps)
        {
            
            ICollection<Step> newSteps = steps
                .Select(s => new Step()
                    {
                        Description = s.Description,
                    })
                .ToList();

            if (recipeId == null)
            {
                await this.stepRepository.AddAllAsync(newSteps);
            }
            else
            {
                await this.stepRepository.UpdateAllAsync(recipeId, newSteps);
            }
            
        }

        private async Task AddOrUpdateIngredients(string? recipeId, IEnumerable<RecipeIngredientFormModel> newIngredients)
        {
            
            ICollection<RecipeIngredient> validRecipeIngredients = new List<RecipeIngredient>();

            foreach (var ingredient in newIngredients)
            {
                var ingredientId = await GetIngredientIdByNameAsync(ingredient.Name);

                // Complex logic allowing the user to add ingredients with the same name but different measure or specification type
                if (ingredientId != null)
                {
                    if (CheckAndUpdateAnExistingIngredient(validRecipeIngredients, ingredient, ingredientId.Value))
                    {
                        continue;
                    }

                    // Otherwise, create a new ingredient
                    validRecipeIngredients.Add(CreateNewRecipeIngredient(ingredient, ingredientId.Value);

                }
                else
                {
                    // such ingredient does not exist in DB => to do => server-side error to upon form submit
                    throw new RecordNotFoundException(IngredientNotFoundExceptionMessage, null);
                }
            }

            if (recipeId != null)
            {
                await this.recipeIngredientRepository.UpdateAllAsync(recipeId, validRecipeIngredients);
            }
            else
            {
                await this.recipeIngredientRepository.AddAllAsync(validRecipeIngredients);
            }
        }

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

        private bool CheckAndUpdateAnExistingIngredient(ICollection<RecipeIngredient> validRecipeIngredients,
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


        private async Task<Recipe> MapRecipeModelToRecipe(IRecipeFormModel model, Recipe recipe)
        {
            
            recipe.Title = model.Title;
            recipe.Description = model.Description;
            recipe.Servings = model.Servings!.Value;
            recipe.TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes!.Value);
            recipe.ImageUrl = model.ImageUrl;
            recipe.CategoryId = model.RecipeCategoryId!.Value;
           
            return recipe;
        }


        
        // Helper method to map ingredients by category in Recipe Details View
        private List<RecipeIngredientDetailsViewModel> MapIngredientsByCategory(Recipe recipe, IEnumerable<int> ingredientCategoryIds)
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
