namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Step;

    public class StepService : IStepService
    {
        private readonly IStepRepository stepRepository;

        public StepService(IStepRepository stepRepository)
        {
            this.stepRepository = stepRepository;
        }

       
        /// <inheritdoc/>
        public async Task AddByRecipeIdAsync(string recipeId, ICollection<StepFormModel> model)
        {
            ICollection<Step> steps = model.Select
                (s => new Step()
                {
                    Id = s.Id.Value,
                    Description = s.Description,
                }).ToList();    

            await stepRepository.AddAllAsync(steps);
        }

        /// <inheritdoc/>
        public async Task UpdateByRecipeIdAsync(string recipeId, ICollection<StepFormModel> model)
        {
            ICollection<Step> oldSteps = await stepRepository.GetAllQuery()
                .Where(s => GuidHelper.CompareGuidStringWithGuid(recipeId, s.RecipeId))
                .ToListAsync();

            await stepRepository.DeleteAllAsync(oldSteps);
            await AddByRecipeIdAsync(recipeId, model);
            
        }

        /// <inheritdoc/>
        public async Task DeleteByRecipeIdAsync(string recipeId)
        {
            ICollection<Step> stepsToDelete = await stepRepository
                .GetAllQuery()
                .Where(s => GuidHelper.CompareGuidStringWithGuid(recipeId, s.RecipeId))
                .ToListAsync();

            await stepRepository.DeleteAllAsync(stepsToDelete);
        }
    }
}
