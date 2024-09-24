namespace CookTheWeek.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;

    using static CookTheWeek.Common.ExceptionMessagesConstants.RecordNotFoundExceptionMessages;
    using CookTheWeek.Common.HelperMethods;

    public class FavouriteRecipeRepository : IFavouriteRecipeRepository
    {
        private readonly CookTheWeekDbContext dbContext;

        public FavouriteRecipeRepository(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        ///  Returns true if the recipe is liked by the user
        /// </summary>
        public async Task<bool> GetByIdAsync(string userId, string recipeId)
        {
            return await this.dbContext.FavoriteRecipes
                .AnyAsync(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId) &&
                                GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId));
        }

        /// <summary>
        ///  To get likes count for a Recipe
        /// </summary>
        public async Task<int?> AllCountByRecipeIdAsync(string recipeId)
        {
            int? likes = await this.dbContext
                .FavoriteRecipes
                .AsNoTracking()
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))
                .CountAsync();

            return likes;
        }

        /// <summary>
        ///  When a User Likes a Recipe, new record is created
        /// </summary>        
        /// 
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

        /// <summary>
        /// When a User Unlikes a Recipe, the record is deleted
        /// </summary>       
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

        /// <summary>
        ///  To get all Recipes, liked by a user
        /// </summary>        
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

        /// <summary>
        /// To delete all records of User Liked Recipes (for user deleted profile, etc.)
        /// </summary>
        
        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var recipes = await this.dbContext 
                .FavoriteRecipes
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(userId, fr.UserId))
                .ToListAsync();

            this.dbContext.FavoriteRecipes.RemoveRange(recipes);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// To delete all likes when a Recipe is deleted
        /// </summary>

        public async Task DeleteAllByRecipeIdAsync(string recipeId)
        {
            var recipes = await this.dbContext
                .FavoriteRecipes
                .Where(fr => GuidHelper.CompareGuidStringWithGuid(recipeId, fr.RecipeId))    
                .ToListAsync();

            this.dbContext.FavoriteRecipes.RemoveRange(recipes);
            await this.dbContext.SaveChangesAsync();
        }

        
    }
}
