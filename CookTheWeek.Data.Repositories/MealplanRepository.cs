namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

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
                .AsQueryable();
        }

        /// <inheritdoc/>
        public IQueryable<MealPlan> GetByIdQuery(Guid id)
        {
            return dbContext.MealPlans
                .Where(mp => mp.Id == id)
                .AsQueryable();


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
        public async Task RemoveAsync(MealPlan mealPlan)
        {
            this.dbContext.MealPlans.Remove(mealPlan);
            await this.dbContext.SaveChangesAsync();

        }

       
        /// <inheritdoc/>
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            // Save all tracked changes (including related entities like meals)
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
