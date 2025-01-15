namespace CookTheWeek.Data.Repositories
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    
    using CookTheWeek.Data.Models;
    using Microsoft.Extensions.Logging;

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
        public async Task<FavouriteRecipe?> GetByIdAsync(Guid userId, Guid recipeId)
        {
            return await dbContext.FavoriteRecipes
                .FirstOrDefaultAsync(fr => fr.UserId == userId &&
                                fr.RecipeId == recipeId);
        }

        /// <inheritdoc/>  
        public async Task AddAsync(FavouriteRecipe like)
        {
            //await dbContext.FavouriteRecipes.AddAsync(like);
            //await dbContext.SaveChangesAsync();

            try
            {
                await dbContext.FavoriteRecipes.AddAsync(like);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                //logger.LogError($"Error adding like: {ex.Message}");
                throw; // Re-throw to let the service handle it
            }
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
