namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Common.HelperMethods;

    public class StepRepository : IStepRepository
    {
        private readonly CookTheWeekDbContext dbContext;
        public StepRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        /// <inheritdoc/>
        public async Task AddAllAsync(ICollection<Step> steps)
        {
            await this.dbContext.AddRangeAsync(steps);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<Step> steps)
        {
            var oldSteps = await this.dbContext.Steps
                .Where(s => GuidHelper.CompareGuidStringWithGuid(recipeId, s.RecipeId))
                .ToListAsync();

            this.dbContext.Steps.RemoveRange(oldSteps);
            await this.dbContext.Steps.AddRangeAsync(steps);

            await this.dbContext.SaveChangesAsync();
        }



        /// <inheritdoc/>
        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            var stepsToDelete = await this.dbContext
                .Steps  
                .Where(s => GuidHelper.CompareGuidStringWithGuid(recipeId, s.RecipeId))
                .ToListAsync();

            this.dbContext.Steps.RemoveRange(stepsToDelete);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
