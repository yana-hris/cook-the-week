
namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IIngredientRepository
    {
        /// <summary>
        /// Gets an Ingredient by Id if it exists or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        Task<Ingredient> GetByIdAsync(int id);

        /// <summary>
        /// Gets a queryable collection of all Ingredients (incl. their Category) that can be filtered, sorted and materialized (awaited) with any of the Async methods later
        /// </summary>
        /// <returns>A queryable collection of Ingredient (+ Category)</returns>
        IQueryable<Ingredient> GetAllQuery();

        /// <summary>
        /// Returns a listof all ingredients, containing in their name a certain search string. To be consumed by the suggestive search functionality
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A collection of Ingredients, filtered by a search string or an empty collection</returns>
        IQueryable<Ingredient> GetAllBySearchStringQuery(string name);

        /// <summary>
        /// Adds an ingredient to the database and returns its id as a result
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns>int</returns>
        Task<int> AddAsync(Ingredient ingredient);

        /// <summary>
        /// Edits an ingredient
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        Task UpdateAsync(Ingredient ingredient);

        /// <summary>
        /// Deletes an ingredient
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        Task DeleteAsync(Ingredient ingredient);

        /// <summary>
        /// Checks if a given ingredient exists by a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Checks if an ingredient with a given name exists and if not, returns false
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true or false</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// The total count of all ingredients in the database
        /// </summary>
        /// <returns>int or </returns>
        Task<int?> CountAsync();
    }
}
