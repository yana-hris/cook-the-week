namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;

    using static CookTheWeek.Common.ExceptionMessagesConstants.DataRetrievalExceptionMessages;

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

        public Task<bool> ExistsByIdAsync(string id)
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

        public async Task<ICollection<Recipe>> GetAllAsync()
        {
            return await this.dbContext
                .Recipes
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<int> GetAllCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Recipe>> GetAllSiteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Recipe>> GetAllUserRecipesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Recipe> GetByIdAsync(string id)
        {
            try
            {
                Recipe? recipe = await this.dbContext.Recipes
                .AsNoTracking()
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                .FirstOrDefaultAsync(r => r.Id.ToString().ToLower() == id);

                return recipe == null ? throw new RecordNotFoundException($"No recipe found with ID {id}.", null) : recipe;
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
