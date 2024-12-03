namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using System.Collections.Generic;

    public interface IRecipeIngredientService
    {
        /// <summary>
        /// Creates RecipeIngredient collection from the passed model
        /// </summary>
        /// <param name="recipeIngredients"></param>
        /// <returns>A collection of RecipeIngredients to pass to the Recipe entity for persisting in the database</returns>
        HashSet<RecipeIngredient> CreateAll(ICollection<RecipeIngredientFormModel> recipeIngredients);

        /// <summary>
        /// Updates RecipeIngredients collection from the passed model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipeIngredients"></param>
        /// <returns></returns>
        Task<HashSet<RecipeIngredient>> UpdateAll(Guid id, ICollection<RecipeIngredientFormModel> recipeIngredients);
        /// <summary>
        /// Gets all measures for recipe ingredients as a view model for select menus
        /// </summary>
        /// <returns>A collection of RecipeIngredientSelectMeasureViewModel</returns>
        Task<ICollection<SelectViewModel>> GetRecipeIngredientMeasuresAsync();

        /// <summary>
        /// Gets all specifications for recipe ingredients as a view model for select menus
        /// </summary>
        /// <returns>A collection of RecipeIngredientSelectSpecificationViewModel</returns>
        Task<ICollection<SelectViewModel>> GetRecipeIngredientSpecificationsAsync();
        
        
        /// <summary>
        /// Soft Deletes all recipe ingredients by a given recipe ID by setting their IsDeleted flag to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SoftDeleteAllByRecipeIdAsync(Guid id);

        
    }
}
