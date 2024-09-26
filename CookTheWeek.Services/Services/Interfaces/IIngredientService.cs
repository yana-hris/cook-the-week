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
        /// Edits an existing ingredient in the database or throws an exception if ingredient is not found
        /// </summary>
        /// <remarks>May throw RecordNotFoundException due to using GetById method</remarks>
        /// <param name="model">The model containing updated ingredient data.</param>
        Task EditAsync(IngredientEditFormModel model);

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
        Task<IngredientEditFormModel> GetForEditByIdAsync(int id);

       
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
        /// <returns>The total count of ingredients (int or 0)</returns>
        Task<int?> AllCountAsync();

        /// <summary>
        /// Deletes an ingredient by its ID or throws and exception
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete</param>
        /// <remarks>May throw RecordNotFoundException of ingredient does not exist</remarks>
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
