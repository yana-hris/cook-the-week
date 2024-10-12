namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeRepository
    {
        /// <summary>
        /// Gets all recipes as a query collection of Recipe, that can be awaited and materialized later with any of the Async methods
        /// </summary>
        /// <returns>A queryable collection of all recipes (inlcuding their Categories)</returns>
        IQueryable<Recipe> GetAllQuery();

        /// <summary>
        /// Adds a recipe to the database and returns the id
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns>string, recipeId</returns>
        Task<string> AddAsync(Recipe recipe);

        /// <summary>
        /// Checks if a given recipe exists in the database and returns a flag
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(Guid id);
        
        /// <summary>
        /// Returns an IQueryable for a Recipe entity based on the provided recipe ID.
        /// This query can be further extended before being executed.
        /// </summary>
        /// <param name="id">The unique identifier (Guid) of the recipe to be queried.</param>
        /// <returns>An IQueryable of Recipe filtered by the specified ID, which can be extended with additional query operations.</returns>
        IQueryable<Recipe> GetByIdQuery(Guid id);

        /// <summary>
        /// Update an existing Recipe in Database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        Task UpdateAsync(Recipe recipe);
        

        /// <summary>
        /// Updates a range of Recipes all at once
        /// </summary>
        /// <param name="recipes"></param>
        /// <returns></returns>
        Task UpdateAllAsync(ICollection<Recipe> recipes);

        /// <summary>
        /// Sets the flag IsDeleted to true
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        Task SoftDeleteAsync(Recipe recipe);

        /// <summary>
        /// Hard deletes the Recipe from the database
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        Task DeleteAsync(Recipe recipe);
    }
}
