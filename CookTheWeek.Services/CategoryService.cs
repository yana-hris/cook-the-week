namespace CookTheWeek.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.Category;

    public class CategoryService : ICategoryService
    {
        private readonly CookTheWeekDbContext dbContext;

        public CategoryService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync()
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

        public async Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync()
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
        public async Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId)
        {
            bool exists = await this.dbContext
                .IngredientCategories
                .AsNoTracking()
                .AnyAsync(c => c.Id == ingredientCategoryId);

            return exists;
        }
        public async Task<ICollection<string>> AllRecipeCategoryNamesAsync()
        {
            ICollection<string> allRecipeCategoryNames = await this.dbContext
                .RecipeCategories
                .AsNoTracking()
                .Select(rc => rc.Name)
                .ToListAsync();

            return allRecipeCategoryNames;
        }

        public async Task<ICollection<string>> AllIngredientCategoryNamesAsync()
        {
            ICollection<string> allIngredientCategoryNames = await this.dbContext
                .IngredientCategories
                .AsNoTracking()
                .Select(ic => ic.Name)
                .ToListAsync();

            return allIngredientCategoryNames;
        }

        public async Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId)
        {
            return await this.dbContext
                .RecipeCategories
                .AnyAsync(rc => rc.Id == recipeCategoryId);
        }
    }
}
