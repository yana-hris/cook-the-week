namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IStepRepository
    {
        /// <summary>
        /// Adds a collection of steps to the database, recipe Id is not needed
        /// </summary>
        Task AddAllAsync(ICollection<Step> steps);

        /// <summary>
        /// Updates the steps of an existing recipe by deleting the old steps and adding the new ones
        /// </summary>
        /// <param name="recipeId">Needed for the old steps to be deleted</param>
        /// <param name="steps">The new steps to add</param>
        Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<Step> steps);

        /// <summary>
        /// Deletes all steps of a Recipe
        /// </summary>
        /// <param name="recipeId">recipeId</param>
        Task DeleteAllByRecipeIdAsync(string recipeId);
    }
}
