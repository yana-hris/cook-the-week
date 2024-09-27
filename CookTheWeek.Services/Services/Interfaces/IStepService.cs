namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;

    public interface IStepService
    {
        /// <summary>
        /// Updates the steps of an existing recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<Step> steps);


        /// <summary>
        /// Adds new steps to a recipe
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task AddAllAsync(ICollection<Step> steps);

        /// <summary>
        /// Deletes all steps a of a Recipe by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteRecipeStepsAsync(string id);
    }
}
