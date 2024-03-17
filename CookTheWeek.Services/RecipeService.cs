﻿namespace CookTheWeek.Services
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Interfaces;
    using Data.Models.Recipe;
    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

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
                .Where(r => !r.IsDeleted)
                .Skip((queryModel.CurrentPage - 1) * queryModel.RecipesPerPage)
                .Take(queryModel.RecipesPerPage)
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
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
        
        public async Task<ICollection<RecipeAllViewModel>> AllUnsortedUnfilteredAsync()
        {
            ICollection<RecipeAllViewModel> allRecipes = await this.dbContext
                .Recipes
                .AsNoTracking()
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.RecipeCategoryId,
                        Name = r.RecipeCategory.Name
                    },
                    Servings = r.Servings,
                    CookingTime = String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),
                }).ToListAsync();

            return allRecipes;
        }

        public async Task AddRecipeAsync(RecipeFormViewModel model)
        {
            Recipe recipe = new Recipe()
            {
                Title = model.Title,
                Description = model.Description,
                Instructions = model.Instructions,
                Servings = model.Servings,
                TotalTime = TimeSpan.FromMinutes(model.CookingTimeMinutes),
                ImageUrl = model.ImageUrl,
                RecipeCategoryId = model.RecipeCategoryId                
            };

            foreach (var item in model.RecipeIngredients!)
            {
                int ingredientId = await this.dbContext.Ingredients
                    .Where(i => i.Name.ToLower() == item.Name.ToLower())
                    .Select(i => i.Id)
                    .FirstOrDefaultAsync();
                if(ingredientId != 0)
                {
                    recipe.RecipesIngredients.Add(new RecipeIngredient()
                    {
                        IngredientId = ingredientId, 
                        Qty = item.Qty,
                        MeasureId = item.MeasureId,
                        SpecificationId = item.SpecificationId
                    });
                }
            }
            await this.dbContext.Recipes.AddAsync(recipe);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task<RecipeDetailsViewModel>? DetailsByIdAsync(string id)
        {
            RecipeDetailsViewModel? model = await this.dbContext
                .Recipes
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
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext
                .Recipes
                .Where(r => r.Id.ToString() == id)
                .AnyAsync();
        }
    }
}
