namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IRecipeIngredientService
    {

        /// <summary>
        /// Gets all measures for recipe ingredients as a view model for select menus
        /// </summary>
        /// <returns>A collection of RecipeIngredientSelectMeasureViewModel</returns>
        Task<ICollection<RecipeIngredientSelectMeasureViewModel>> GetRecipeIngredientMeasuresAsync();

        /// <summary>
        /// Gets all specifications for recipe ingredients as a view model for select menus
        /// </summary>
        /// <returns>A collection of RecipeIngredientSelectSpecificationViewModel</returns>
        Task<ICollection<RecipeIngredientSelectSpecificationViewModel>> GetRecipeIngredientSpecificationsAsync();
        //Task RemoveAsync(int ingredientId, string recipeId);

        /// <summary>
        /// Creates a new RecipeIngredient from a RecipeIngredientFormModel or throws an exception
        /// </summary>
        /// <param name="model"></param>
        /// <returns>RecipeIngredient</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<RecipeIngredient> CreateRecipeIngredientForAddRecipeAsync(RecipeIngredientFormModel model);

        /// <summary>
        /// Updates the Recipe Ingredients of an existing recipe by deleting all old ingredients for this recipe and adding the new ingredients
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newIngredients"></param>
        /// <returns></returns>
        Task EditAsync(Guid id, ICollection<RecipeIngredient> newIngredients);

        /// <summary>
        /// Adds recipe ingredients upon creating a new recipe in the database
        /// </summary>
        /// <param name="addedIngredients"></param>
        /// <returns></returns>
        Task AddAsync(ICollection<RecipeIngredient> addedIngredients);

        /// <summary>
        /// Deletes all recipeIngredients by a recipe id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task HardDeleteAllByRecipeIdAsync(Guid id);

        /// <summary>
        /// Soft Deletes all recipe ingredients by a given recipe ID by setting their IsDeleted flag to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid id);
    }
}
