namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.Ingredient;
    using CookTheWeek.Services.Data.Models.RecipeIngredient;
    using CookTheWeek.Web.ViewModels.Admin.IngredientAdmin;

    public interface IIngredientService
    {
        /// <summary>
        /// Retrieves all ingredients with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="queryModel">The model containing filtering, sorting, and pagination options.</param>
        /// <returns>An object containing the filtered, sorted, and paginated list of ingredients along with the total count.</returns>
        Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel);

        /// <summary>
        /// Adds a new ingredient to the database.
        /// </summary>
        /// <param name="model">The model containing the ingredient data to be added.</param>
        /// <returns>The ID of the newly added ingredient.</returns>
        Task<OperationResult> TryAddIngredientAsync(IngredientAddFormModel model);

        /// <summary>
        /// Tries to edit an existing ingredient by first validating the form model. May throw exceptions or return model errors.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException due to using GetById method</remarks>
        /// <param name="model">The model containing updated ingredient data.</param>
        /// <returns>The result from the operation (IsSuccess or if not => result.Errors for ModelState) </returns>
        Task<OperationResult> TryEditIngredientAsync(IngredientEditFormModel model);

        /// <summary>
        /// Generates a collection of ingredient suggestions model, based on the input search string, to be used for suggestive search functionality
        /// </summary>
        /// <param name="input">The input string to search for ingredient suggestions.</param>
        /// <returns>A collection of RecipeIngredientSuggestionsServiceModel</returns>
        Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input);

        /// <summary>
        /// Retrieves an ingredient by ID for editing purposes or throws an exception
        /// </summary>
        /// <param name="id">The ID of the ingredient to retrieve.</param>
        /// <remarks>May throw RecordNotFoundException due to using GetById method</remarks>
        /// <returns>The IngredientEditFormModel containing the ingredient data, or null if not found.</returns>
        Task<IngredientEditFormModel> TryGetIngredientModelForEditAsync(int id);

       
        /// <summary>
        /// Checks if an ingredient exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to check for existence.</param>
        /// <returns>True if the ingredient exists, false otherwise.</returns>
        Task<bool> ExistsByIdAsync(int id);
        
        /// <summary>
        /// Retrieves the total count of all ingredients in the database.
        /// </summary>
        /// <returns>The total count of ingredients (int or 0)</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Tries to delete an ingredient if it is not already included in any Recipes.
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete</param>
        /// <remarks>May throw RecordNotFoundException of ingredient does not exist</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        Task TryDeleteByIdAsync(int id);

        /// <summary>
        /// Gets an ingredient by id or throws an exception
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns>Ingredient</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<Ingredient> GetByIdAsync(int ingredientId);

        // <summary>
        /// Checks if there are any ingredients in the given category (by id) and returns true or false
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns>true or false</returns>
        Task<bool> HasAnyWithCategory(int categoryId);
    }
}
