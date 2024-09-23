namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeRepository
    {
        /// <summary>
        /// Gets all recipes as a query collection 
        /// </summary>
        /// <returns>A queryable collection of all recipes</returns>
        IQueryable<Recipe> GetAllQuery();

        /// <summary>
        /// Adds a recipe to the database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns>Newly created Recipe ID</returns>
        Task<string> AddAsync(Recipe recipe);

        /// <summary>
        /// Checks if a given recipe exists in the database and returns a flag
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(string id);

        /// <summary>
        /// Gets a Recipe By id (including recipe Owner, recipe Steps, Category, Recipe Likes, Recipe Meals, Recipe Ingredients + their categories + their measures + their specifications)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException">If Recipe is null, throws a RecordNotFoundException when recipe is not found</exception>
        Task<Recipe> GetByIdAsync(string id);

        /// <summary>
        /// Update an existing Recipe in Database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        Task UpdateAsync(Recipe recipe);

        /// <summary>
        /// Soft deletes a single recipe by setting its boolean property IsDeleted to true. If accepted Recipe is null, throws an ArgumentNullException.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task Delete(Recipe recipe);

        /// <summary>
        /// Soft deletes a collection of recipes by setting its boolean property IsDeleted to true. Does not delete any nested entities or properties of the recipe itself.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAllByOwnerIdAsync(string userId);

        
    }
}
