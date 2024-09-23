namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class MealplanRepository : IMealplanRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealplanRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        /// <inheritdoc/>
        public IQueryable<MealPlan> GetAllQuery()
        {
            return this.dbContext.MealPlans
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task<MealPlan?> GetByIdAsync(string id)
        {
            return await this.dbContext.MealPlans
                .Where(mp => mp.Id.ToString().ToLower() == id.ToLower())
                    .Include(mp => mp.Meals)
                        .ThenInclude(m => m.Recipe)
                            .ThenInclude(r => r.RecipesIngredients)
                    .Include(mp => mp.Meals)
                        .ThenInclude(m => m.Recipe)
                            .ThenInclude(r => r.Category)
                .FirstAsync();
        }

        /// <inheritdoc/>
        public async Task<string> AddAsync(MealPlan mealPlan)
        {
            await this.dbContext.MealPlans.AddAsync(mealPlan);
            await this.dbContext.SaveChangesAsync();

            string mealPlanId = mealPlan.Id.ToString().ToLower();
            return mealPlanId;
        }

        /// <inheritdoc/>
        public Task UpdateAsync(MealPlan newMealPlan)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(MealPlan mealPlan)
        {
            throw new NotImplementedException();
        }

        

        

       
    }
}
