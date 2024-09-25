namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        Task<int> AddAsync(IngredientAddFormModel model);

        /// <summary>
        /// Edits an existing ingredient in the database.
        /// </summary>
        /// <param name="model">The model containing updated ingredient data.</param>
        Task EditAsync(IngredientEditFormModel model);

        /// <summary>
        /// Generates a list of ingredient suggestions based on the input search string.
        /// </summary>
        /// <param name="input">The input string to search for ingredient suggestions.</param>
        /// <returns>A list of ingredient suggestions that match the input search string.</returns>
        Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input);

        /// <summary>
        /// Retrieves an ingredient by ID for editing purposes.
        /// </summary>
        /// <param name="id">The ID of the ingredient to retrieve.</param>
        /// <returns>The ingredient edit form model containing the ingredient data, or null if not found.</returns>
        Task<IngredientEditFormModel?> GetForEditByIdAsync(int id);

        /// <summary>
        /// Retrieves the ID of an ingredient by its name.
        /// </summary>
        /// <param name="name">The name of the ingredient to search for.</param>
        /// <returns>The ID of the ingredient, or null if not found.</returns>
        Task<int?> GetIdByNameAsync(string name);

        /// <summary>
        /// Checks if an ingredient exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to check for existence.</param>
        /// <returns>True if the ingredient exists, false otherwise.</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Checks if an ingredient exists by its name.
        /// </summary>
        /// <param name="name">The name of the ingredient to check for existence.</param>
        /// <returns>True if the ingredient exists, false otherwise.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Retrieves the total count of all ingredients in the database.
        /// </summary>
        /// <returns>The total count of ingredients.</returns>
        Task<int> AllCountAsync();

        /// <summary>
        /// Deletes an ingredient by its ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete.</param>
        Task DeleteByIdAsync(int id);



        // OLD CODE:
        //Task<AllIngredientsFilteredAndPagedServiceModel> AllAsync(AllIngredientsQueryModel queryModel);
        //Task<int> AddAsync(IngredientAddFormModel model);
        //Task EditAsync(IngredientEditFormModel model);
        //Task<IEnumerable<RecipeIngredientSuggestionServiceModel>> GenerateIngredientSuggestionsAsync(string input);
        //Task<IngredientEditFormModel> GetForEditByIdAsync(int id);
        //Task<int?> GetIdByNameAsync(string name);
        //Task<bool> ExistsByIdAsync(int id);
        //Task<bool> ExistsByNameAsync(string name);
        //Task<int> AllCountAsync();
        //Task DeleteById(int id);

    }
}
