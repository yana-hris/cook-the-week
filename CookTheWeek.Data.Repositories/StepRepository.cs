namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;

    public class StepRepository : IStepRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public StepRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        /// <inheritdoc/>
        public IQueryable<Step> GetAllQuery()
        {
            return dbContext.Steps
                .AsNoTracking()
                .AsQueryable();
        }

        /// <inheritdoc/>
        public async Task AddAllAsync(ICollection<Step> steps)
        {
            await this.dbContext.AddRangeAsync(steps);
            await this.dbContext.SaveChangesAsync();
        }
                
        /// <inheritdoc/>
        public async Task DeleteAllAsync(ICollection<Step> steps)
        {
            this.dbContext.Steps.RemoveRange(steps);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SoftDeleteAsync(Step step)
        {
            step.IsDeleted = true;
            dbContext.Steps.Update(step);
            await dbContext.SaveChangesAsync();
        }
    }
}
