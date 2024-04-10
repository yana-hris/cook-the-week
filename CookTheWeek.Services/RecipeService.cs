namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Data.Models.Recipe;
    using Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Web.ViewModels.RecipeIngredient;

    using static Common.GeneralApplicationConstants;

    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        public RecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AllRecipesFilteredAndPagedServiceModel> AllAsync(AllRecipesQueryModel queryModel)
        {
            IQueryable<Recipe> recipesQuery = this.dbContext
                .Recipes
                .AsNoTracking()
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                recipesQuery = recipesQuery
                    .Where(r => r.RecipeCategory.Name == queryModel.Category);
            }

            if(!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                recipesQuery = recipesQuery
                    .Where(r => EF.Functions.Like(r.Title, wildCard) ||
                            EF.Functions.Like(r.Description, wildCard) ||
                            r.RecipesIngredients!.Any(ri => EF.Functions.Like(ri.Ingredient.Name, wildCard)));                                                      
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
                        Id = r.RecipeCategoryId,
                        Name = r.RecipeCategory.Name
                    },
                    Servings = r.Servings,
                    CookingTime = String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),
                })
                .ToListAsync();

            int totalRecipes = recipesQuery.Count();

            return new AllRecipesFilteredAndPagedServiceModel()
            {
                TotalRecipesCount = totalRecipes,
                Recipes = allRecipes
            };
        }

        public async Task<string> AddAsync(RecipeAddFormModel model, string ownerId)
        {
            Recipe recipe = new Recipe()
            {
                Title = model.Title,
                OwnerId = ownerId,
                Description = model.Description,
                Instructions = model.Instructions,
                Servings = model.Servings,
                TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes),
                ImageUrl = model.ImageUrl,
                RecipeCategoryId = model.RecipeCategoryId                
            };

            foreach (var ingredient in model.RecipeIngredients!)
            {
                int ingredientId = await this.dbContext.Ingredients
                    .Where(i => i.Name.ToLower() == ingredient.Name.ToLower())
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();
                if(ingredientId != 0)
                {
                    recipe.RecipesIngredients.Add(new RecipeIngredient()
                    {
                        IngredientId = ingredientId, 
                        Qty = ingredient.Qty,
                        MeasureId = ingredient.MeasureId,
                        SpecificationId = ingredient.SpecificationId
                    });
                }
            }
            await this.dbContext.Recipes.AddAsync(recipe);
            await this.dbContext.SaveChangesAsync();

            return recipe.Id.ToString();
        }
        public async Task EditAsync(RecipeEditFormModel model)
        {
            Recipe recipe = await this.dbContext
                .Recipes
                .Include(r => r.RecipesIngredients)
                .Where(r => r.Id.ToString() == model.Id)
                .FirstAsync();

            recipe.Title = model.Title;
            recipe.Description = model.Description;
            recipe.Instructions = model.Instructions;
            recipe.Servings = model.Servings;
            recipe.TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes);
            recipe.ImageUrl = model.ImageUrl;
            recipe.RecipeCategoryId = model.RecipeCategoryId;

            ICollection<RecipeIngredient> oldIngredients = recipe.RecipesIngredients;
            this.dbContext.RecipesIngredients.RemoveRange(oldIngredients);

            foreach (var ingredient in model.RecipeIngredients!)
            {
                int ingredientId = await this.dbContext.Ingredients
                    .Where(i => i.Name.ToLower() == ingredient.Name.ToLower())
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();
                if (ingredientId != 0)
                {
                    recipe.RecipesIngredients.Add(new RecipeIngredient()
                    {
                        IngredientId = ingredientId,
                        Qty = ingredient.Qty,
                        MeasureId = ingredient.MeasureId,
                        SpecificationId = ingredient.SpecificationId
                    });
                }
            }
            
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
                    Instructions = r.Instructions,
                    Servings = r.Servings,
                    TotalTime = r.TotalTime.ToString(@"hh\:mm"),
                    ImageUrl = r.ImageUrl,
                    CreatedOn = r.CreatedOn.ToString("dd-MM-yyyy"),
                    CategoryName = r.RecipeCategory.Name,
                    MainIngredients = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => MainIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList() ,
                    SecondaryIngredients = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => SecondaryIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
                        .Select(ri => new RecipeIngredientDetailsViewModel()
                        {
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty,
                            Measure = ri.Measure.Name,
                            Specification = ri.Specification.Description,
                        }).ToList(),
                    AdditionalIngredients = r.RecipesIngredients
                        .OrderBy(ri => ri.Ingredient.IngredientCategoryId)
                        .ThenBy(ri => ri.Ingredient.Name)
                        .Where(ri => AdditionalIngredientsCategories.Contains(ri.Ingredient.IngredientCategoryId))
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
                .Where(r => r.Id.ToString() == id)
                .AnyAsync();
        }
        public async Task DeleteByIdAsync(string id)
        {
            // Implementing soft delete requires to delete manually all related records, as we do not need them in the database
            Recipe recipeToDelete = await this.dbContext
                .Recipes
                .Include(r => r.RecipesIngredients)
                .Where(r => r.Id.ToString() == id)
                .FirstAsync();
            
            recipeToDelete.IsDeleted = true;

            // Delete RecipeIngredients of deleted recipes
            if(recipeToDelete != null && recipeToDelete.RecipesIngredients.Any())
            {
                this.dbContext.RecipesIngredients.RemoveRange(recipeToDelete.RecipesIngredients);
            }

            // Delete User Likes for Deleted Recipes
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
                    Instructions = r.Instructions,
                    Servings = r.Servings,
                    CookingTimeMinutes = (int)r.TotalTime.TotalMinutes,
                    ImageUrl = r.ImageUrl,
                    RecipeCategoryId = r.RecipeCategoryId,
                    RecipeIngredients = r.RecipesIngredients.Select(ri => new RecipeIngredientFormViewModel()
                    {
                        Name = ri.Ingredient.Name,
                        Qty = ri.Qty,
                        MeasureId = ri.MeasureId,
                        SpecificationId = ri.SpecificationId
                    }).ToList()
                }).FirstAsync();

            return recipe;
        }
        public async Task<RecipeDeleteViewModel> GetForDeleteByIdAsync(string id)
        {
            RecipeDeleteViewModel model = await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == id)
                .Select(r => new RecipeDeleteViewModel()
                {
                    Id = r.Id.ToString(),
                    Title = r.Title,
                    ImageUrl = r.ImageUrl,
                    Servings = r.Servings,
                    TotalTime = (int)r.TotalTime.TotalMinutes,
                    CreatedOn = r.CreatedOn.ToString("dd-MM-yyyy"),
                    CategoryName = r.RecipeCategory.Name
                }).FirstAsync();

            return model;
        }
        public async Task<ICollection<RecipeAllViewModel>> AllAddedByUserAsync(string userId)
        {
            ICollection<RecipeAllViewModel> myRecipes = await this.dbContext
                .Recipes
                .Include("RecipeCategory")
                .Where(r => r.OwnerId == userId)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Description = r.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.RecipeCategoryId,
                        Name = r.RecipeCategory.Name
                    },
                    Servings = r.Servings,
                    CookingTime = String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),

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
                .Where(r => r.OwnerId == userId)
                .CountAsync();
        }
        
        
    }
}
