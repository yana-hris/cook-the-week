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

        Task<RecipeIngredient> CreateRecipeIngredientFromModel(RecipeIngredientFormModel model);
    }
}
