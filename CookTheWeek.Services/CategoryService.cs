namespace CookTheWeek.Services
{
    using CookTheWeek.Data;
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Category;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly CookTheWeekDbContext dbContext;

        public CategoryService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ICollection<IngredientCategorySelectViewModel>> GetAllIngredientCategoriesAsync()
        {
            ICollection<IngredientCategorySelectViewModel> allIngredientCategories = await this.dbContext
                .IngredientCategories
                .AsNoTracking()
                .Select(ic => new IngredientCategorySelectViewModel()
                {
                    Id = ic.Id,
                    Name = ic.Name,
                })
                .ToListAsync();

            return allIngredientCategories;
        }

        public async Task<ICollection<RecipeCategorySelectViewModel>> GetAllRecipeCategoriesAsync()
        {
            ICollection<RecipeCategorySelectViewModel> allRecipeCategories = await this.dbContext
                .RecipeCategories
                .AsNoTracking()
                .Select(rc => new RecipeCategorySelectViewModel()
                { 
                    Id = rc.Id,
                    Name = rc.Name 
                })
                .ToListAsync();

            return allRecipeCategories;
        }

        public async Task<bool> ingredientCategoryExistsByIdAsync(int ingredientCategoryId)
        {
            bool exists = await this.dbContext
                .IngredientCategories
                .AsNoTracking()
                .AnyAsync(c => c.Id == ingredientCategoryId);

            return exists;
        }
    }
}
