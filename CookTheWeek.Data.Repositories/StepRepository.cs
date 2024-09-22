namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
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
        public async Task AddAllAsync(ICollection<Step> steps)
        {
            await this.dbContext.AddRangeAsync(steps);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task UpdateAllAsync(string recipeId, ICollection<Step> steps)
        {
            var oldSteps = await this.dbContext.Steps
                .Where(s => s.RecipeId.ToString().ToLower() == recipeId.ToLower())
                .ToListAsync();

            this.dbContext.Steps.RemoveRange(oldSteps);
            await this.dbContext.Steps.AddRangeAsync(steps);

            await this.dbContext.SaveChangesAsync();
        }
        public async Task DeleteAllAsync(string recipeId)
        {
            var stepsToDelete = await this.dbContext
                .Steps
                .Where(s => s.RecipeId.ToString().ToLower() == recipeId.ToLower())
                .ToListAsync();

            this.dbContext.Steps.RemoveRange(stepsToDelete);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
