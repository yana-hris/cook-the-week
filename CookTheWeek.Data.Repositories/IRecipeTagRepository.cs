namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;

    public interface IRecipeTagRepository
    {
        /// <summary>
        /// Gets a queriable collection of tracked recipe tags
        /// which can be changed and persisted in the database when calling SaveChangesAsync() method
        /// </summary>
        /// <returns>An IQueryable of RecipeTags</returns>
        IQueryable<RecipeTag> GetAllTrackedQuery();
    }
}
