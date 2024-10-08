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
        public async Task AddRangeAsync(ICollection<Step> steps)
        {
            await dbContext.Steps.AddRangeAsync(steps);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateRangeAsync(ICollection<Step> steps)
        {
            dbContext.Steps.UpdateRange(steps);
            await dbContext.SaveChangesAsync();
        }
                
        /// <inheritdoc/>
        public async Task DeleteRangeAsync(ICollection<Step> steps)
        {
            dbContext.Steps.RemoveRange(steps);
            await dbContext.SaveChangesAsync();
        }
        
    }
}
