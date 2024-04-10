namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;

    public class MealplanService : IMealplanService
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealplanService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AllActiveCountAsync()
        {
            return await this.dbContext
                .MealPlans
                .Where(m => m.IsFinished == false)
                .CountAsync();
        }
    }
}
