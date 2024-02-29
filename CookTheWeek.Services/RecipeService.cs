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
