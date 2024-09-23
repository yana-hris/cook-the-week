namespace CookTheWeek.Data.Repositories
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

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
            IQueryable<Recipe>? allRecipes = this.dbContext
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
        public async Task<Recipe> GetByIdAsync(string id)
        {
            id = id.ToLower();
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
                .FirstOrDefaultAsync(r => string.Equals(r.Id.ToString(), id, StringComparison.OrdinalIgnoreCase));
                //.FirstOrDefaultAsync(r => r.Id.ToString().ToLower() == id.ToLower()); // Check if works!

            if (recipe == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);
            }
            
            return recipe;
           
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Recipe recipe)
        {
            this.dbContext.Update(recipe);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Delete(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(ArgumentNullExceptionMessages.RecipeNullExceptionMessage);
            }
            recipe.OwnerId = Guid.Parse(DeletedUserId);
            recipe.IsDeleted = true;
            await UpdateAsync(recipe);
        }

        /// <inheritdoc/>
        public async Task DeleteAllByOwnerIdAsync(string userId)
        {
            await this.dbContext.Recipes
                .Where(r => r.OwnerId.ToString().ToLower() == userId.ToLower())
            .ExecuteUpdateAsync(r => r
                .SetProperty(r => r.OwnerId, Guid.Parse(DeletedUserId))
                .SetProperty(r => r.IsDeleted, true));
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext.Recipes
                .AnyAsync(r => r.Id.ToString().ToLower() == id.ToLower());
        }
    }
}
