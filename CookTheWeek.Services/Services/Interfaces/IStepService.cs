namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.Step;
    using System.Collections.Generic;

    public interface IStepService
    {
        /// <summary>
        /// Updates the steps of an existing Recipe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task<ICollection<Step>> UpdateAll(Guid id, List<StepFormModel> steps);

        /// <summary>
        /// Creates all Steps needed for adding a new Recipe by consuming the collection of Step model
        /// </summary>
        /// <param name="steps"></param>
        /// <returns>A collection of Steps</returns>
        ICollection<Step> CreateAll(ICollection<StepFormModel> steps);

        /// <summary>
        /// Doft deletes all steps of a given Recipe by its ID by setting their IsDeleted flag to true
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid recipeId);

        
    }
}
