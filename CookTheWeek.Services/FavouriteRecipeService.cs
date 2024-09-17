namespace CookTheWeek.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Data.Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;

    using static Common.HelperMethods.CookingTimeHelper;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;

        public FavouriteRecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> IsLikedByUserIdAsync(string id, string userId)
        {
            bool isFavourite = await this.dbContext.FavoriteRecipes
                .AnyAsync(fr => fr.UserId.ToString() == userId &&
                                fr.RecipeId.ToString() == id);

            return isFavourite;
        }
        public async Task LikeAsync(string id, string userId)
        {
            FavouriteRecipe favouriteRecipe = new FavouriteRecipe()
            {
                UserId = Guid.Parse(userId),
                RecipeId = Guid.Parse(id)
            };

            await this.dbContext.FavoriteRecipes
                .AddAsync(favouriteRecipe);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task UnlikeAsync(string id, string userId)
        {
            FavouriteRecipe favouriteRecipe = await this.dbContext
                .FavoriteRecipes
                .FirstAsync(fr => fr.UserId.ToString() == userId && fr.RecipeId.ToString() == id);

            this.dbContext.FavoriteRecipes.Remove(favouriteRecipe);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task<ICollection<RecipeAllViewModel>> AllLikedByUserIdAsync(string userId)
        {
            ICollection<RecipeAllViewModel> myRecipes = await this.dbContext
                .FavoriteRecipes
                .Include(fr => fr.Recipe)
                .ThenInclude(r => r.Category)
                .Where(fr => fr.UserId.ToString() == userId)
                .Select(fr => new RecipeAllViewModel()
                {
                    Id = fr.Recipe.Id.ToString(),
                    ImageUrl = fr.Recipe.ImageUrl,
                    Title = fr.Recipe.Title,
                    Description = fr.Recipe.Description,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = fr.Recipe.CategoryId,
                        Name = fr.Recipe.Category.Name
                    },
                    Servings = fr.Recipe.Servings,
                    CookingTime = FormatCookingTime(fr.Recipe.TotalTime)

                }).ToListAsync();

            return myRecipes;
        }
        public async Task<int?> LikesCountAsync(string recipeId)
        {
            int? likes = await this.dbContext
                .FavoriteRecipes
                .AsNoTracking()
                .Where(fr => fr.RecipeId.ToString().ToLower() == recipeId)
                .CountAsync();

            return likes;   
        }
    }
}
