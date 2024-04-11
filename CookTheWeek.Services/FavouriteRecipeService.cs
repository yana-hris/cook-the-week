namespace CookTheWeek.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Data.Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Recipe;

    public class FavouriteRecipeService : IFavouriteRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;

        public FavouriteRecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> ExistsByUserIdAsync(string id, string userId)
        {
            bool isFavourite = await this.dbContext.FavoriteRecipes
                .AnyAsync(fr => fr.UserId.ToString() == userId &&
                                fr.RecipeId.ToString() == id);

            return isFavourite;
        }
        public async Task AddByUserIdAsync(string id, string userId)
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
        public async Task RemoveByUserIdAsync(string id, string userId)
        {
            FavouriteRecipe favouriteRecipe = await this.dbContext
                .FavoriteRecipes
                .FirstAsync(fr => fr.UserId.ToString() == userId && fr.RecipeId.ToString() == id);

            this.dbContext.FavoriteRecipes.Remove(favouriteRecipe);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task<ICollection<RecipeAllViewModel>> AllByUserIdAsync(string userId)
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
                    CookingTime = String.Format(@"{0}h {1}min", fr.Recipe.TotalTime.Hours.ToString(), fr.Recipe.TotalTime.Minutes.ToString()),

                }).ToListAsync();

            return myRecipes;
        }
    }
}
