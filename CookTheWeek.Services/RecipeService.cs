namespace CookTheWeek.Services
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models.RecipeEntities;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;
    using Web.ViewModels.Recipe.Enums;
    using Interfaces;
    using Data.Models.Recipe;

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
                    .OrderBy(r => r.CreatedOn),
                RecipeSorting.Oldest => recipesQuery
                    .OrderByDescending(r => r.CreatedOn),
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

        /// <summary>
        /// This generates a predefined dictionary for the servings of each recipe instead of using a new DB entity.
        /// The first int value is the key, the second is the serving size
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, int> GenerateServingOptions()
        {
            return new Dictionary<int, int>()
            {
                { 1, 2 },
                { 2, 4 },
                { 3, 6 },
                { 4, 8 },
                { 5, 10 },
                { 6, 12 }
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
    }
}
