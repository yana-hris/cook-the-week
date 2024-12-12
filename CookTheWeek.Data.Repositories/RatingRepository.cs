namespace CookTheWeek.Data.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookTheWeek.Data.Models;

    public class RatingRepository : IRatingRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public RatingRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        /// <inheritdoc/>    
        public IQueryable<RecipeRating> GetAllQuery()
        {
            return dbContext.Ratings
                .AsQueryable();
        }


        /// <inheritdoc/>    
        public async Task UpdateRangeAsync(ICollection<RecipeRating> allRatings)
        {
            dbContext.Ratings.UpdateRange(allRatings);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>    
        public async Task DeleteRangeAsync(ICollection<RecipeRating> allRatings)
        {
            dbContext.Ratings.RemoveRange(allRatings);
            await dbContext.SaveChangesAsync();
        }
    }
}
