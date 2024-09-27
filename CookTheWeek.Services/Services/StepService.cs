namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class StepService : IStepService
    {
        private readonly IStepRepository stepRepository;

        public StepService(IStepRepository stepRepository)
        {
            this.stepRepository = stepRepository;
        }

       
        /// <inheritdoc/>
        public async Task AddAllAsync(ICollection<Step> steps)
        {
            await stepRepository.AddAllAsync(steps);
        }

        /// <inheritdoc/>
        public async Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<Step> steps)
        {
            await stepRepository.UpdateAllByRecipeIdAsync(recipeId, steps);
        }

        /// <inheritdoc/>
        public async Task DeleteRecipeStepsAsync(string id)
        {
            await stepRepository.DeleteAllByRecipeIdAsync(id);
        }
    }
}
