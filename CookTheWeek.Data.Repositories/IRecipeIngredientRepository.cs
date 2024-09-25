namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IRecipeIngredientRepository
    {
        /// <summary>
        /// Adds a collection of Recipe Ingredients to the database
        /// </summary>
        /// <param name="recipeIngredients"></param>
        /// <returns></returns>
        Task AddAllAsync(ICollection<RecipeIngredient> recipeIngredients);
        
        /// <summary>
        /// Updates the Recipe Ingredients of an existing recipe by deleting all old ingredients for this recipe and adding the new ingredients
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="recipeIngredients"></param>
        /// <returns></returns>
        Task UpdateAllByRecipeIdAsync(string recipeId, ICollection<RecipeIngredient> recipeIngredients);

        /// <summary>
        /// Deletes all Recipe Ingredients by a given recipeID
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        Task DeleteAllByRecipeIdAsync(string recipeId);
        Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetAllMeasuresAsync();
    }
}
