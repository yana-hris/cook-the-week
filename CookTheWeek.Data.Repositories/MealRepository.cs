namespace CookTheWeek.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class MealRepository : IMealRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public MealRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        /// <inheritdoc/>
        public IQueryable<Meal> GetAllQuery()
        {
            return this.dbContext
                .Meals
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<Meal> GetByIdAsync(int id)
        {
            Meal meal = await this.dbContext
                .Meals
                .FirstAsync(m => m.Id == id);

            return meal;
        }

        /// <inheritdoc/>
        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<Meal> mealsToDelete = await this.dbContext
                .Meals
                .Where(m => m.RecipeId.ToString().ToLower() == recipeId.ToLower())
                .ToListAsync();

            this.dbContext.RemoveRange(mealsToDelete);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
