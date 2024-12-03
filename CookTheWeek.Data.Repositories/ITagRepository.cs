namespace CookTheWeek.Data.Repositories
{
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Models.Interfaces;

    public interface ITagRepository
    {
        /// <summary>
        /// Returns a queryable collection of all existing recipe tags
        /// </summary>
        /// <returns>A collection of tags</returns>
        IQueryable<Tag> GetAllQuery();

        /// <summary>
        /// Returns the tag if existing or null
        /// </summary>
        /// <returns>FavouriteRecipe or null</returns>
        Task<Tag?> GetByIdAsync(int tagId);

        /// <summary>
        ///  Adds a given tag to the database
        /// </summary>     
        Task AddAsync(Tag tag);

        /// <summary>
        /// Edits the current TCategory and persists changes in the database
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task UpdateAsync(Tag tag);

        /// <summary>
        /// Finds a Tag by id and deletes it from the database
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task DeleteAsync(Tag tag);


    }
}
