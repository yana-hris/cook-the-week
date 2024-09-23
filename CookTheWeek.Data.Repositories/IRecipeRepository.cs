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

        
    }
}
