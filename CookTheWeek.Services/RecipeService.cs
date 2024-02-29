namespace CookTheWeek.Services
{
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;
    using CookTheWeek.Web.ViewModels.Recipe;
    public class RecipeService : IRecipeService
    {
        private readonly CookTheWeekDbContext dbContext;
        public RecipeService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// This generates a predefined dictionary for the servings of each recipe instead of using a new DB entity.
        /// The first int value is the key, the second is the serving size
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, int> GenerateServingOptions()
        {
            return new Dictionary<int, int>()
            {
                { 1, 2 },
                { 2, 4 },
                { 3, 6 },
                { 4, 8 },
                { 5, 10 },
                { 6, 12 }
            };
        }

        public async Task<ICollection<RecipeAllViewModel>> GetAllRecipesAsync()
        {
            ICollection<RecipeAllViewModel> allRecipes = await this.dbContext
                .Recipes
                .AsNoTracking()
                .Select(r => new RecipeAllViewModel()
                {
                    Id = r.Id.ToString(),
                    ImageUrl = r.ImageUrl,
                    Title = r.Title,
                    Category = new RecipeCategorySelectViewModel()
                    {
                        Id = r.RecipeCategoryId,
                        Name = r.RecipeCategory.Name
                    },
                    Servings = r.Servings,
                    CookingTime = String.Format(@"{0}h {1}min", r.TotalTime.Hours.ToString(), r.TotalTime.Minutes.ToString()),
                }).ToListAsync();

            return allRecipes;
        }
    }
}
