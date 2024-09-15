namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Meal;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Web.ViewModels.RecipeIngredient;
    using Web.ViewModels.Step;

    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.CookingTimeHelper;

    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        
        public RecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<RecipeAllViewModel>> AllAsync(AllRecipesQueryModel queryModel, string userId)
        {
            
            // TODO: move to repository
            IQueryable<Recipe> recipesQuery = this.dbContext
                .Recipes
                .AsNoTracking()
                .AsQueryable();
                        
            if(userId != String.Empty)
            {
                recipesQuery = recipesQuery
                    .Where(r => r.OwnerId.ToString() == userId || r.IsSiteRecipe);
            }
            else
            {
                recipesQuery = recipesQuery
                    .Where(r => r.IsSiteRecipe);
            }

            if(!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                recipesQuery = recipesQuery
                    .Where(r => r.Category.Name == queryModel.Category);
            }

            if(!string.IsNullOrWhiteSpace(queryModel.SearchString))
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

            if (queryModel.CurrentPage == (int)default)
            {
                queryModel.CurrentPage = DefaultPage;
            }

            if (queryModel.RecipesPerPage == (int)default)
            {
                queryModel.RecipesPerPage = DefaultRecipesPerPage;
            }

            queryModel.TotalRecipes = await recipesQuery.CountAsync();

            ICollection<RecipeAllViewModel> allRecipes = await recipesQuery
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

            return allRecipes;
        }
        public async Task<string> AddAsync(RecipeAddFormModel model, string ownerId, bool isAdmin)
        {

            var recipe = MapToRecipe(model, ownerId, isAdmin);
            await AddStepsToRecipeAsync(recipe, model.Steps);
            await AddIngredientsToRecipeAsync(recipe, model.RecipeIngredients);

            await this.dbContext.Recipes.AddAsync(recipe);
            await this.dbContext.SaveChangesAsync();

            return recipe.Id.ToString().ToLower();
        }
        public async Task EditAsync(RecipeEditFormModel model)
        {
            // Load the recipe including related entities
            Recipe recipe = await this.dbContext
                .Recipes
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                .Where(r => r.Id.ToString() == model.Id)
                .FirstAsync();
            
            UpdateRecipeNonCollectionFields(model, recipe);
            
            this.dbContext.Steps.RemoveRange(recipe.Steps);
            recipe.Steps.Clear();
            await AddStepsToRecipeAsync(recipe, model.Steps);

            
            this.dbContext.RecipesIngredients.RemoveRange(recipe.RecipesIngredients);
            recipe.RecipesIngredients.Clear();
            await AddIngredientsToRecipeAsync(recipe, model.RecipeIngredients);
            
            await this.dbContext.SaveChangesAsync();
        }        
        public async Task<RecipeDetailsViewModel> DetailsByIdAsync(string id)
        {
            RecipeDetailsViewModel model = await this.dbContext
                .Recipes
                .AsNoTracking()
                .Where(r => r.Id.ToString() == id)
                .Select(r => new RecipeDetailsViewModel()
                {
                    Id = r.Id.ToString(),
                    Title = r.Title,
                    Description = r.Description,
                    Steps = r.Steps.Select(s => new StepViewModel()
                    {
                        Id = s.Id,
                        Description = s.Description
                    }).ToList(),
                    Servings = r.Servings,
                    TotalTime = r.TotalTime, //String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),
                    ImageUrl = r.ImageUrl,
                    CreatedOn = r.CreatedOn.ToString("dd-MM-yyyy"),
                    CategoryName = r.Category.Name,
                    DiaryMeatSeafood = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => DiaryMeatSeafoodIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList() ,
                    Produce = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => ProduceIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                    Legumes = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => LegumesIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                    PastaGrainsBakery = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => PastaGrainsBakeryIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                    OilsHerbsSpicesSweeteners = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => OilsHerbsSpicesSweetenersIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                    NutsSeedsAndOthers = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.CategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => NutsSeedsAndOthersIngredientCategories.Contains(ri.Ingredient.CategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                })
                .FirstAsync();

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
        public async Task DeleteByIdAsync(string id)
        {
            // Implementing soft delete requires to delete manually all related records, as we do not need them in the database
            Recipe recipeToDelete = await this.dbContext
                .Recipes
                .Include(r => r.RecipesIngredients)
                .Include(r => r.Steps)
                .Include(r => r.FavouriteRecipes)
                .Include(r => r.Meals)
                .Where(r => r.Id.ToString() == id)
                .FirstAsync();
            
            recipeToDelete.IsDeleted = true;

            // Delete Steps of deleted recipe
            if (recipeToDelete != null && recipeToDelete.Steps.Any())
            {
                this.dbContext.Steps.RemoveRange(recipeToDelete.Steps);
            }

            // Delete RecipeIngredients of deleted recipe
            if(recipeToDelete != null && recipeToDelete.RecipesIngredients.Any())
            {
                this.dbContext.RecipesIngredients.RemoveRange(recipeToDelete.RecipesIngredients);
            }

            // Delete User Likes for Deleted Recipe
            if(recipeToDelete != null && recipeToDelete.FavouriteRecipes.Any())
            {
                this.dbContext.FavoriteRecipes.RemoveRange(recipeToDelete.FavouriteRecipes);
            }

            // Delete All Meals that contain the Recipe
            if(recipeToDelete != null && recipeToDelete.Meals.Any()) 
            {
                this.dbContext.Meals.RemoveRange(recipeToDelete.Meals);
            }

            await this.dbContext.SaveChangesAsync();
        }
        public async Task<RecipeEditFormModel> GetForEditByIdAsync(string id)
        {
            RecipeEditFormModel recipe = await this.dbContext
                .Recipes
                .AsNoTracking()
                .Where(r => r.Id.ToString() == id)
                .Select(r => new RecipeEditFormModel()
                {
                    Id = r.Id.ToString(),
                    Title = r.Title,
                    Description = r.Description,
                    Steps = r.Steps.Select(s => new StepFormModel()
                    {
                        Id = s.Id,
                        Description = s.Description
                        
                    }).ToList(),
                    Servings = r.Servings,
                    CookingTimeMinutes = (int)r.TotalTime.TotalMinutes,
                    ImageUrl = r.ImageUrl,
                    RecipeCategoryId = r.CategoryId,
                    RecipeIngredients = r.RecipesIngredients.Select(ri => new RecipeIngredientFormModel()
                    {
                        Name = ri.Ingredient.Name,
                        Qty = RecipeIngredientQtyFormModel.ConvertFromDecimalQty(ri.Qty, ri.Measure.Name),
                        MeasureId = ri.MeasureId,
                        SpecificationId = ri.SpecificationId
                    }).ToList()
                }).FirstAsync();

            return recipe;
        }
        
        public async Task<ICollection<RecipeAllViewModel>> AllAddedByUserAsync(string userId)
        {
            ICollection<RecipeAllViewModel> myRecipes = await this.dbContext
                .Recipes
                .Include(r => r.Category)
                .Where(r => r.OwnerId.ToString() == userId)
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

            return myRecipes;
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
        private Recipe MapToRecipe(RecipeAddFormModel model, string ownerId, bool isAdmin)
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

        private async Task AddStepsToRecipeAsync(Recipe recipe, IEnumerable<StepFormModel> steps)
        {
            foreach (var step in steps)
            {
                recipe.Steps.Add(new Step
                {
                    Description = step.Description
                });
            }
        }

        private async Task AddIngredientsToRecipeAsync(Recipe recipe, IEnumerable<RecipeIngredientFormModel> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                int? ingredientId = await GetIngredientIdByNameAsync(ingredient.Name);

                if (ingredientId.HasValue && ingredientId != 0)
                {                    
                    bool isIngredientAlreadyAdded = recipe.RecipesIngredients
                        .Any(ri => ri.IngredientId == ingredientId &&
                                ri.MeasureId == ingredient.MeasureId);

                    if (isIngredientAlreadyAdded)
                    {
                        // Check if specs is the same and if yes update qty
                        var ingredientWithSpecsAdded = recipe.RecipesIngredients
                        .FirstOrDefault(ri => ri.IngredientId == ingredientId &&
                                        ri.MeasureId == ingredient.MeasureId &&
                                        ri.SpecificationId == ingredient.SpecificationId);

                        if (ingredientWithSpecsAdded != null)
                        {
                            decimal newQty = ingredient.Qty.GetDecimalQtyValue() + ingredientWithSpecsAdded.Qty;
                            ingredientWithSpecsAdded.Qty = newQty;
                            continue;
                        }
                    }

                    // Or alternatively create the new ingredient
                    recipe.RecipesIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = ingredientId.Value,
                        Qty = ingredient.Qty.GetDecimalQtyValue(),
                        MeasureId = ingredient.MeasureId!.Value,
                        SpecificationId = ingredient.SpecificationId
                    });
                }               
            }
        }

        private static void UpdateRecipeNonCollectionFields(RecipeEditFormModel model, Recipe recipe)
        {
            recipe.Title = model.Title;
            recipe.Description = model.Description;
            recipe.Servings = model.Servings!.Value;
            recipe.TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes!.Value);
            recipe.ImageUrl = model.ImageUrl;
            recipe.CategoryId = model.RecipeCategoryId!.Value;
        }


        // TODO: move to ingredient service?
        private async Task<int?> GetIngredientIdByNameAsync(string name)
        {
            return await this.dbContext.Ingredients
                .Where(i => i.Name.ToLower() == name.ToLower())
                .Select(i => i.Id)
                .FirstOrDefaultAsync();
        }

       
    }
}
