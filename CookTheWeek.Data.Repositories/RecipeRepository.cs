namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;

    using static CookTheWeek.Common.ExceptionMessagesConstants.DataRetrievalExceptionMessages;
    using static CookTheWeek.Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;

    public class RecipeRepository : IRecipeRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<string> AddAsync(Recipe recipe)
        {
            await this.dbContext.Recipes.AddAsync(recipe);
            await this.dbContext.SaveChangesAsync();

            return recipe.Id.ToString();
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<ICollection<Recipe>>? GetAllByUserIdAsync(string userId)
        {
            var recipes = await this.dbContext
                .Recipes
                .Include(r => r.Category)
                .AsNoTracking()
                .Where(r => r.OwnerId.ToString() == userId)
                .ToListAsync();

            return recipes;
        }

        public IQueryable<Recipe> GetAllQuery()
        {

            IQueryable<Recipe>? allRecipes = this.dbContext
                .Recipes
                .AsNoTracking()
                .AsQueryable();

            if (!allRecipes.Any())
            {
                throw new RecordNotFoundException(NoRecipesFoundExceptionMessage, null);
            }

            return allRecipes;
        }
        
        public async Task<Recipe?> GetByIdAsync(string id)
        {
            try
            {
                Recipe? recipe = await this.dbContext.Recipes
                    .AsNoTracking()
                    .Include(r => r.Owner)
                    .Include(r => r.Steps)
                    .Include(r => r.Category)
                    .Include(r => r.RecipesIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                            .ThenInclude(i => i.Category)
                    .Include(r => r.RecipesIngredients)
                        .ThenInclude(ri => ri.Measure)
                    .Include(r => r.RecipesIngredients)
                        .ThenInclude(ri => ri.Specification)
                    .FirstOrDefaultAsync(r => r.Id.ToString().ToLower() == id);


                return recipe;
            }
            catch (Exception ex)
            {
                throw new DataRetrievalException(RecipeDataRetrievalExceptionMessage, ex);
            }
        }

        public Task<Recipe> GetForEditByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> GetForMealByIdAsync(string recipeId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMineCountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsIncludedInMealPlansAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            this.dbContext.Update(recipe);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
