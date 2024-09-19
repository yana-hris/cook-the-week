namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    
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
        
        public async Task<Recipe> GetByIdAsync(string id)
        {
            // TODO: think about optimizing this query or splitting to separate by needed nested entities
            Recipe? recipe = await this.dbContext.Recipes
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
                .Include(r => r.Meals)
                .Include(r => r.FavouriteRecipes)
                .FirstOrDefaultAsync(r => r.Id.ToString().ToLower() == id.ToLower());

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecipeNotFoundExceptionMessage, null);
            }
            
            return recipe;
           
        }                        
        
        public async Task UpdateAsync(Recipe recipe)
        {
            this.dbContext.Update(recipe);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
