namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;

    public class StepRepository : IStepRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public StepRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        /// <inheritdoc/>
        public IQueryable<Step> GetAllTrackedQuery()
        {
            return dbContext.Steps
                .AsQueryable();
        }

        /// <inheritdoc/>
        public void AddRange(ICollection<Step> steps)
        {
            dbContext.Steps.AddRange(steps);
        }

        
        /// <inheritdoc/>
        public void DeleteRange(ICollection<Step> steps)
        {
            dbContext.Steps.RemoveRange(steps);
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        
    }
}
