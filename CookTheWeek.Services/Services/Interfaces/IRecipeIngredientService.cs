namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    public interface IRecipeIngredientService
    {
        Task<bool> IngredientMeasureExistsAsync(int measureId);
        Task<bool> IngredientSpecificationExistsAsync(int specificationId);
        //Task<bool> IsAlreadyAddedAsync(string ingredientName, string recipeId);
        //Task<int> AddAsync(RecipeIngredientFormModel model, string recipeId);

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
        /// Updates the recipe ingredients of an existing recipe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addedIngredients"></param>
        /// <returns></returns>
        Task EditAsync(string id, ICollection<RecipeIngredient> addedIngredients);

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
        Task DeleteAllByRecipeIdAsync(string id);
    }
}
