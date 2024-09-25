namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;

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
                .Where(mp => GuidHelper.CompareGuidStringWithGuid(id, mp.Id))
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

            string mealPlanId = mealPlan.Id.ToString();
            return mealPlanId;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(MealPlan mealPlan)
        {
            this.dbContext.MealPlans.Update(mealPlan);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteByIdAsync(MealPlan mealPlan)
        {
            this.dbContext.MealPlans.Remove(mealPlan);
            await this.dbContext.SaveChangesAsync();

        }

        /// <inheritdoc/>
        public async Task DeleteAllAsync(ICollection<MealPlan> mealPlans)
        {
            this.dbContext.MealPlans.RemoveRange(mealPlans);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
