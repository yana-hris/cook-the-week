namespace CookTheWeek.Data.Repositories
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    
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
        public IQueryable<Recipe> GetByIdQuery(Guid id)
        {

            return dbContext.Recipes
                .Where(r => r.Id == id)
                .AsQueryable();
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
        public async Task DeleteAsync(Recipe recipe)
        {
            dbContext.Recipes.Remove(recipe);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
