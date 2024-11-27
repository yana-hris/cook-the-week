namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
   
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Step;

    public class StepService : IStepService
    {
        private readonly IStepRepository stepRepository;
        private readonly ILogger<StepService> logger;   

        public StepService(IStepRepository stepRepository, 
            ILogger<StepService> logger)
        {
            this.stepRepository = stepRepository;
            this.logger = logger;
        }


        /// <inheritdoc/>
        public ICollection<Step> CreateAll(ICollection<StepFormModel> steps)
        {
            ICollection<Step> newSteps = new HashSet<Step>();

            foreach (var stepModel in steps)
            {
                Step newStep = Create(null, stepModel);
                newSteps.Add(newStep);
            }

            return newSteps;
        }

        /// <inheritdoc/>
        public async Task<ICollection<Step>> UpdateAll(Guid id, List<StepFormModel> updatedStepsModelCollection)
        {
            ICollection<Step> oldSteps = await GetAllByRecipeIdAsync(id);

            HashSet<Step> updatedSteps = new HashSet<Step>();
            
            foreach (var stepModel in updatedStepsModelCollection)
            {
                var existingStep = oldSteps.FirstOrDefault(s => s.Id == stepModel.Id);

                if (existingStep == null)
                {
                    Step stepToAdd = Create(id, stepModel);
                    updatedSteps.Add(stepToAdd);
                }
                else
                {
                    existingStep.Description = stepModel.Description;
                    updatedSteps.Add(existingStep);
                }
            }

            return updatedSteps;
        }

        /// <inheritdoc/>
        //public async Task UpdateByRecipeIdAsync(Guid recipeId, ICollection<StepFormModel> stepsModel)
        //{
        //    ICollection<Step> existingSteps = await GetAllByRecipeIdAsync(recipeId);

        //    ICollection<Step> newStepsToAdd = new HashSet<Step>();
        //    ICollection<Step> stepsToDelete = new HashSet<Step>();

        //    foreach (var step in stepsModel)
        //    {
        //        var existingStep = existingSteps.FirstOrDefault(s => s.Id == step.Id);

        //        if (existingStep == null)
        //        {
        //            Step stepToAdd = Create(recipeId, step);
        //            newStepsToAdd.Add(stepToAdd);
        //        }
        //        else
        //        {
        //            existingStep.Description = step.Description;    // On Save changes will be saved
        //        }
        //    }

        //    foreach (var probableStepToRemove in existingSteps)
        //    {
        //        var existingStep = stepsModel.FirstOrDefault(s => s.Id == probableStepToRemove.Id);

        //        if (existingStep == null)
        //        {
        //            stepsToDelete.Add(probableStepToRemove);
        //        }
        //    }

        //    try
        //    {
        //        if (newStepsToAdd.Count > 0)
        //        {
        //            stepRepository.AddRange(newStepsToAdd);
        //        }

        //        if (stepsToDelete.Count > 0)
        //        {
        //            stepRepository.DeleteRange(stepsToDelete);
        //        }

        //        await stepRepository.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Database update of recipe steps for recipe with id {recipeId} failed." +
        //            $"Error Message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
        //        throw;
        //    }
        //}

        
        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(Guid recipeId)
        {
            ICollection<Step> stepsToUpdate = await GetAllByRecipeIdAsync(recipeId);

            foreach (var step in stepsToUpdate)
            {
                step.IsDeleted = true;
            }

            await stepRepository.SaveChangesAsync();
        }


        
        /// <summary>
        /// Helper method to get a collection of all steps by a given recipe ID
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task<ICollection<Step>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await stepRepository
                .GetAllTrackedQuery()
                .Where(s => s.RecipeId == recipeId)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a single step by a given Step Form Model
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="step"></param>
        /// <returns>Step</returns>
        private static Step Create(Guid? recipeId, StepFormModel step)
        {
            var newStep = new Step
            {
                Description = step.Description
            };

            if (recipeId != null && recipeId != Guid.Empty)
            {
                newStep.RecipeId = recipeId.Value;
            }

            return newStep;
        }

        
    }
}
