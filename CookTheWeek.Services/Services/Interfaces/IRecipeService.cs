namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe;


    public interface IRecipeService
    {

        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters (if any)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns>A collection of Recipes</returns>
        /// <exception cref="RecordNotFoundException">Thrown if no recipes exist in the database</exception>
        Task<ICollection<Recipe>> GetAllAsync(AllRecipesQueryModel queryModel); 

        /// <summary>
        /// Takes the model and validates further for any errors, if the model is correct, persists the Recipe in the database
        /// </summary>
        /// <param name="model">The Recipe form model to create a new recipe from</param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>The Result of the Operation: Success/Failure (and the recipeId if Success)</returns>
        /// <remarks>May throw RecordNotFoundException due to underlying usage of GetById method</remarks>
        Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model); 

        /// <summary>
        /// Edits an existing Recipe by first validating the model, including all nested entities. 
        /// Persists the changes in the database if model is valid.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException</remarks>
        /// <returns>Operation Result if the model is valid or not</returns>
        Task<OperationResult> TryEditRecipeAsync(RecipeEditFormModel model);

        /// <summary>
        /// Gets a RecipeDetailsServiceModel if by a given ID if it exists in the database or throws an exception
        /// </summary>
        /// <param name="id">Recipe Id</param>
        /// <remarks>May throw RecordNotFoundException due to GetById</remarks>
        /// <returns>RecipeDetailsServiceModel</returns>
        Task<RecipeDetailsServiceModel> GetForDetailsByIdAsync(Guid id);
                

        /// <summary>
        /// Returns a Recipe by a given Id or throws an exceptions if recipe is not found or is not owned by the current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Recipe</returns>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="RecordNotFoundException">rethrown</exception>
        Task<Recipe> GetForEditByIdAsync(Guid id);


        /// <summary>
        /// Deletes a specific (soft delete) recipe and all its nested nested and connected entities (hard delete): recipe ingredients, recipe steps, meals and likes. Throws an exception if the recipe is included in Meal Plans.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException due to using GetById</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task DeleteByIdAsync(Guid id);

        /// <summary>
        /// Returns the count of all recipes
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> GetAllCountAsync();

        /// <summary>
        /// Returns a collection of Recipes by a list of their IDs
        /// </summary>
        /// <param name="recipeIds">A collection of string (recipeIds)</param>
        /// <returns>A collection of Recipes</returns>
        Task<ICollection<Recipe>> GetAllByIds(ICollection<string> recipeIds);
        
        /// <summary>
        /// Gets a Recipe for a meal details view including all its steps and recipee ingredients info (incl. ingredients category)
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A single Recipe</returns>
        /// <remarks>May throw a RecordNotFoundException if recipe id doesn not exist</remarks>
        Task<Recipe> GetForMealByIdAsync(Guid recipeId);

        /// <summary>
        /// Gets all recipe Ids for added and liked by the current user recipes
        /// </summary>
        /// <returns>A collection IList of recipe ids (string)</returns>
        Task<RecipeAllMineServiceModel> GetAllMineAsync();
    }
}
