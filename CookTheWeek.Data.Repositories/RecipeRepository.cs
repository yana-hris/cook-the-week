namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    
    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class RecipeRepository : IRecipeRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IQueryable<Recipe> GetAllQuery()
        {
            IQueryable<Recipe> allRecipes = this.dbContext
                .Recipes
                .Include(r => r.Category)
                .AsNoTracking()
                .AsQueryable();

            return allRecipes;
        }

        /// <inheritdoc/>
        public async Task<string> AddAsync(Recipe recipe)
        {
            await this.dbContext.Recipes.AddAsync(recipe);
            await this.dbContext.SaveChangesAsync();

            return recipe.Id.ToString();
        }        

        /// <inheritdoc/>
        public async Task<Recipe> GetByIdAsync(Guid id)
        {
            
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
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }
            
            return recipe;
           
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Recipe recipe)
        {
            dbContext.Recipes.Update(recipe);
            await this.dbContext.SaveChangesAsync();
        }
        
        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await dbContext.Recipes
                .AnyAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task UpdateAllAsync(ICollection<Recipe> recipes)
        {
            dbContext.Recipes.UpdateRange(recipes);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAsync(Recipe recipe)
        {
            recipe.IsDeleted = true;
            dbContext.Recipes.Update(recipe);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Recipe recipe)
        {
            dbContext.Recipes.Remove(recipe);
            await dbContext.SaveChangesAsync();
        }
    }
}
