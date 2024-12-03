namespace CookTheWeek.Data.Repositories
{
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
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        
    }
}
