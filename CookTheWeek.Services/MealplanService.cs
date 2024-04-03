namespace CookTheWeek.Services.Data
{
    using CookTheWeek.Data;
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

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
