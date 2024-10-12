namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Step;

    public interface IStepService
    {
        /// <summary>
        /// Updates the steps of an existing recipe by deleting the old steps and adding the new ones to the database
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task UpdateAllByRecipeIdAsync(Guid recipeId, ICollection<StepFormModel> model);


        /// <summary>
        /// Adds new steps to a recipe from a collection of SteFormModel
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task AddAllByRecipeIdAsync(Guid recipeId, ICollection<StepFormModel> model);

        /// <summary>
        /// Doft deletes all steps of a given Recipe by its ID by setting their IsDeleted flag to true
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid recipeId);

        /// <summary>
        /// Hard deletes all steps a given recipe by its recipe ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task HardDeleteAllByRecipesIdAsync(Guid id);
        
    }
}
