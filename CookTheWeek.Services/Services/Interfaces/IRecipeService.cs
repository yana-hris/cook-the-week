﻿namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using CookTheWeek.Common;
    using CookTheWeek.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.ViewModels.Recipe;

    public interface IRecipeService
    {

        /// <summary>
        /// Returns a collection of all Recipes, filtered and sorted according to the query model parameters (if any)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="userId"></param>
        /// <returns>A collection of RecipeAllViewModel</returns>
        /// <exception cref="RecordNotFoundException">Thrown if no recipes exist in the database</exception>
        Task<ICollection<RecipeAllViewModel>> GetAllAsync(AllRecipesQueryModel queryModel, string userId); 

        /// <summary>
        /// Takes the model and validates further for any errors, if the model is correct, persists the Recipe in the database
        /// </summary>
        /// <param name="model">The Recipe form model to create a new recipe from</param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>The Result of the Operation: Success/Failure (and the recipeId if Success)</returns>
        /// <remarks>May throw RecordNotFoundException due to underlying usage of GetById method</remarks>
        Task<OperationResult<string>> TryAddRecipeAsync(RecipeAddFormModel model, string userId, bool isAdmin); 

        /// <summary>
        /// Edits an existing Recipe by first validating the model, including all nested entities. 
        /// Persists the changes in the database if model is valid.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException</remarks>
        /// <returns>Operation Result if the model is valid or not</returns>
        Task<OperationResult> TryEditRecipeAsync(RecipeEditFormModel model);

        /// <summary>
        /// Gets and creates a Detailed Viewmodel for a specific recipe or throws an exception
        /// </summary>
        /// <param name="id">Recipe Id</param>
        /// <remarks>May throw RecordNotFoundException due to GetById</remarks>
        /// <returns>RecipeDetailsViewModel</returns>
        Task<RecipeDetailsViewModel> TryGetModelForDetailsById(string id);
                

        /// <summary>
        /// Creates a RecipeEditFormModel or throws exceptions if recipe is not found or is not owned by the current user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns>RecipeEditFormModel</returns>
        /// <exception cref="UnauthorizedUserException"></exception>
        /// <exception cref="RecordNotFoundException">rethrown</exception>
        Task<RecipeEditFormModel> GetForEditByIdAsync(string id, string userId, bool isAdmin);


        /// <summary>
        /// Deletes a specific (soft delete) recipe and all its nested nested and connected entities (hard delete): recipe ingredients, recipe steps, meals and likes. Throws an exception if the recipe is included in Meal Plans.
        /// </summary>
        /// <remarks>May throw RecordNotFoundException due to using GetById</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="UnauthorizedUserException"></exception>
        Task DeleteByIdAsync(string id, string userId, bool isAdmin);

        /// <summary>
        /// Returns a viewmodel collection of all recipes, added by a specific user by userId. 
        /// Does not throw any exceptions.
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> GetAllAddedByUserIdAsync(string userId); 

        /// <summary>
        /// Returns the number of recipes added by a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int or 0</returns>
        Task<int?> GetMineCountAsync(string userId);

        /// <summary>
        /// Returns the count of all recipes
        /// </summary>
        /// <returns>int or 0</returns>
        Task<int?> GetAllCountAsync();

        /// <summary>
        /// Returns a flag indicating if a recipe is included in any meals which aren`t cooked yet (or in any active meal plans)
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsIncludedInMealPlansAsync(string recipeId);

        /// <summary>
        /// Returns a collection of all Site Recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> GetAllSiteRecipesAsync();

        /// <summary>
        /// Returns a collection of all users` recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> GetAllNonSiteRecipesAsync();

        /// <summary>
        /// Returns all user-liked recipes
        /// </summary>
        /// <returns>A collection of RecipeAllViewModel</returns>
        Task<ICollection<RecipeAllViewModel>> GetAllLikedByUserIdAsync(string userId);
        
        /// <summary>
        /// Gets the total amount of meals, cooked using a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>int?</returns>
        Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId);

        /// <summary>
        /// Soft deletes a collection of recipes. All sub-entities will be deleted too. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAllByUserIdAsync(string userId);

        /// <summary>
        /// Gets a Recipe for a meal details view including all its steps and recipee ingredients info (incl. ingredients category)
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A single Recipe</returns>
        /// <remarks>May throw a RecordNotFoundException if recipe id doesn not exist</remarks>
        Task<Recipe> GetForMealByIdAsync(string recipeId);

        /// <summary>
        /// Checks if there are any recipes in the given category (by id) and returns true or false
        /// </summary>
        /// <param name="categoryId">Recipe Category id</param>
        /// <returns>true or false</returns>
        Task<bool> HasAnyWithCategory(int categoryId);
    }
}
