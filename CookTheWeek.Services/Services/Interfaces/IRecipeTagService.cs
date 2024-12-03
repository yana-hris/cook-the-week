namespace CookTheWeek.Services.Data.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;

    public interface IRecipeTagService
    {
        /// <summary>
        /// Creates a collection of RecipeTags
        /// </summary>
        /// <param name="selectedTagIds"></param>
        /// <returns></returns>
        ICollection<RecipeTag> CreateAll(List<int> selectedTagIds);

        /// <summary>
        /// Updates the collection of RecipeTags for a given Recipe
        /// </summary>
        /// <param name="id">the Recipe ID</param>
        /// <param name="selectedTagIds">A collection of the updated RecipeTag IDs</param>
        /// <returns>A collection of RecipeTags</returns>
        Task<ICollection<RecipeTag>> UpdateAll(Guid id, List<int> selectedTagIds);
    }
}
