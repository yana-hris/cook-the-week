namespace CookTheWeek.Data.Repositories
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
   
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;

    public class FavouriteRecipeRepository : IFavouriteRecipeRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public FavouriteRecipeRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        

        /// <inheritdoc/>    
        public IQueryable<FavouriteRecipe> GetAllQuery()
        {
            return dbContext.FavoriteRecipes
                .AsNoTracking()
                .Include(fr => fr.Recipe)
                    .ThenInclude(r => r.Category)
                .AsQueryable();
        }

        /// <inheritdoc/>    
        public async Task<FavouriteRecipe?> GetByIdAsync(string userId, string recipeId)
        {
            return  await this.dbContext.FavoriteRecipes
                .FirstOrDefaultAsync(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId) &&
                                GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId));
        }

        /// <inheritdoc/>  
        public async Task AddAsync(FavouriteRecipe like)
        {
            await dbContext.FavoriteRecipes.AddAsync(like);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>  
        public async Task DeleteAsync(FavouriteRecipe like)
        {
            dbContext.Remove(like);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>  
        public async Task DeleteAllAsync(ICollection<FavouriteRecipe> userLikes)
        {
            dbContext.RemoveRange(userLikes);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>  
        public async Task SoftDeleteAsync(FavouriteRecipe like)
        {
            like.IsDeleted = true;
            dbContext.FavoriteRecipes.Update(like);
            await dbContext.SaveChangesAsync();
        }
    }
}
