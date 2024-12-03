namespace CookTheWeek.Data.Repositories
{
    using System.Linq;

    using CookTheWeek.Data.Models;

    public class RecipeTagRepository : IRecipeTagRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public RecipeTagRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IQueryable<RecipeTag> GetAllTrackedQuery()
        {
            IQueryable<RecipeTag> allRecipeTags = dbContext
                .RecipeTags
                .AsQueryable();

            return allRecipeTags;
        }
    }
}
