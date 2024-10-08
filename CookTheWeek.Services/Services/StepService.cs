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
        public async Task AddAllByRecipeIdAsync(string recipeId, ICollection<StepFormModel> model)
        {
            ICollection<Step> steps = model.Select
                (s => new Step()
                {
                    Id = s.Id.Value,
                    Description = s.Description,
                }).ToList();    

            await stepRepository.AddRangeAsync(steps);
        }

        /// <inheritdoc/>
        public async Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<StepFormModel> stepsModel)
        {
            ICollection<Step> oldSteps = await GetAllByRecipeIdAsync(recipeId);
            await stepRepository.DeleteRangeAsync(oldSteps);

            var newSteps = stepsModel
                .Select(st => new Step()
                {
                    Id = st.Id.Value,
                    Description = st.Description,
                }).ToList();
            
            await stepRepository.AddRangeAsync(newSteps);
        }

        
        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<Step> stepsToUpdate = await GetAllByRecipeIdAsync(recipeId);

            foreach (var step in stepsToUpdate)
            {
                step.IsDeleted = true;
            }

            await stepRepository.UpdateRangeAsync(stepsToUpdate);
        }


        /// <inheritdoc/>
        public async Task HardDeleteAllByRecipesIdAsync(string recipeId)
        {
            ICollection<Step> stepsToDelete = await GetAllByRecipeIdAsync(recipeId);

            await stepRepository.DeleteRangeAsync(stepsToDelete);
        }

        /// <summary>
        /// Helper method to get a collection of all steps by a given recipe ID
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task<ICollection<Step>> GetAllByRecipeIdAsync(string recipeId)
        {
            return await stepRepository.GetAllQuery()
                .Where(s => GuidHelper.CompareGuidStringWithGuid(recipeId, s.RecipeId))
                .ToListAsync();
        }

    }
}
