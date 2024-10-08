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
        public async Task UpdateRangeAsync(ICollection<FavouriteRecipe> favourites)
        {
            dbContext.FavoriteRecipes.UpdateRange(favourites);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>  
        public async Task DeleteAsync(FavouriteRecipe like)
        {
            dbContext.FavoriteRecipes.Remove(like);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>  
        public async Task DeleteRangeAsync(ICollection<FavouriteRecipe> userLikes)
        {
            dbContext.FavoriteRecipes.RemoveRange(userLikes);
            await dbContext.SaveChangesAsync();
        }

       
    }
}
