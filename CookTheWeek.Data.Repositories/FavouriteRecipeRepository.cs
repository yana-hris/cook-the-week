namespace CookTheWeek.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;

    using static CookTheWeek.Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;

    public class FavouriteRecipeRepository : IFavouriteRecipeRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public FavouriteRecipeRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        /// <inheritdoc/>
        public async Task<bool> GetByIdAsync(string userId, string recipeId)
        {
            return await this.dbContext.FavoriteRecipes
                .AnyAsync(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId) &&
                                GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId));
        }

        /// <inheritdoc/>
        public async Task<int?> GetAllCountByRecipeIdAsync(string recipeId)
        {
            int? likes = await this.dbContext
                .FavoriteRecipes
                .AsNoTracking()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))
                .CountAsync();

            return likes;
        }

        /// <inheritdoc/>
        public async Task AddAsync(string userId, string recipeId)
        {
            FavouriteRecipe favouriteRecipe = new FavouriteRecipe()
            {
                UserId = Guid.Parse(userId),
                RecipeId = Guid.Parse(recipeId)
            };

            await this.dbContext.FavoriteRecipes
                .AddAsync(favouriteRecipe);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string userId, string recipeId)
        {
            FavouriteRecipe? favouriteRecipe = await this.dbContext
                .FavoriteRecipes
                .FirstOrDefaultAsync(fr => fr.UserId.ToString() == userId && fr.RecipeId.ToString() == recipeId);

            if (favouriteRecipe == null)
            {
                throw new RecordNotFoundException(FavouriteRecipeNotFoundExceptionMessage, null);
            }

            this.dbContext.FavoriteRecipes.Remove(favouriteRecipe);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>    
        public async Task<ICollection<FavouriteRecipe>> GetAllByUserIdAsync(string userId)
        {
            ICollection<FavouriteRecipe> myRecipes = await this.dbContext
                .FavoriteRecipes
                .AsNoTracking()
                .Include(fr => fr.Recipe)
                    .ThenInclude(r => r.Category)
                .Where(fr => fr.UserId.ToString() == userId)
                .ToListAsync();

            return myRecipes;
        }

        /// <inheritdoc/>    
        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var recipes = await this.dbContext 
                .FavoriteRecipes
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId))
                .ToListAsync();

            if (recipes.Any())
            {
                this.dbContext.FavoriteRecipes.RemoveRange(recipes);
                await this.dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>    
        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            var recipes = await this.dbContext
                .FavoriteRecipes
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))    
                .ToListAsync();

            if (recipes.Any())
            {
                this.dbContext.FavoriteRecipes.RemoveRange(recipes);
                await this.dbContext.SaveChangesAsync();
            }
        }
        
    }
}
