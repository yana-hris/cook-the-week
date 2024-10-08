namespace CookTheWeek.Data.Repositories
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;

    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class IngredientRepository : IIngredientRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public IngredientRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        /// <inheritdoc/>
        public IQueryable<Ingredient> GetAllQuery()
        {
            return dbContext.Ingredients
                .Include(i => i.Category)
                .AsNoTracking()
                .AsQueryable();
        }
       
        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await dbContext
                .Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await dbContext
                .Ingredients
                .AsNoTracking()
                .AnyAsync(i => i.Name == name);
        }

        /// <inheritdoc/>
        public async Task<Ingredient> GetByIdAsync(int id)
        {
            Ingredient? ingredient = await dbContext.Ingredients
                            .Include(i => i.Category)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.IngredientNotFoundExceptionMessage, null);
            }

            return ingredient;
        }

        /// <inheritdoc/>
        public IQueryable<Ingredient> GetAllBySearchStringQuery(string searchString)
        {
            string wildCard = $"%{searchString.ToLower()}%";

            return dbContext
                .Ingredients
                .AsNoTracking()
                .Where(i => EF.Functions.Like(i.Name.ToLower(), wildCard))
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<int> AddAsync(Ingredient ingredient)
        {
            await this.dbContext.Ingredients.AddAsync(ingredient);
            await this.dbContext.SaveChangesAsync();

            return ingredient.Id;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Ingredient ingredient)
        {
            dbContext.Ingredients.Update(ingredient);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Ingredient ingredient)
        {
            dbContext.Ingredients.Remove(ingredient);
            await dbContext.SaveChangesAsync();
        }
        
    }
}
