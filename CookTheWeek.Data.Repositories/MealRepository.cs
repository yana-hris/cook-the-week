namespace CookTheWeek.Data.Repositories
{
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
        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<Meal> mealsToDelete = await this.dbContext
                .Meals
                .Where(m => m.RecipeId.ToString().ToLower() == recipeId.ToLower())
                .ToListAsync();

            this.dbContext.RemoveRange(mealsToDelete);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<int> GetAllCountByRecipeIdAsync(string recipeId)
        {
            return await this.dbContext
                .Meals
                .AsNoTracking()
                .Where(m => m.RecipeId.ToString().ToLower() == recipeId.ToLower())
                .CountAsync();
        }

        public async Task<Meal> GetByIdAsync(int id)
        {
            Meal meal = await this.dbContext
                .Meals
                .FirstAsync(m => m.Id == id);

            return meal;
        }

        
    }
}
