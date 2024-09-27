namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Web.ViewModels.Step;

    public interface IStepService
    {
        /// <summary>
        /// Updates the steps of an existing recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task UpdateByRecipeIdAsync(string recipeId, ICollection<StepFormModel> model);


        /// <summary>
        /// Adds new steps to a recipe from a collection of SteFormModel
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task AddByRecipeIdAsync(string recipeId, ICollection<StepFormModel> model);

        /// <summary>
        /// Deletes all steps a of a Recipe by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByRecipeIdAsync(string id);
    }
}
