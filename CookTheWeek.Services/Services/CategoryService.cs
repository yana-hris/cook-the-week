namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.Admin.CategoryAdmin;
    using CookTheWeek.Web.ViewModels.Category;

    public class CategoryService : ICategoryService
    {
        private readonly CookTheWeekDbContext dbContext;

        public CategoryService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Recipe Categories Services
        public async Task<ICollection<RecipeCategorySelectViewModel>> AllRecipeCategoriesAsync()
        {
            ICollection<RecipeCategorySelectViewModel> allRecipeCategories = await dbContext
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
        public async Task<ICollection<string>> AllRecipeCategoryNamesAsync()
        {
            ICollection<string> allRecipeCategoryNames = await dbContext
                .RecipeCategories
                .AsNoTracking()
                .Select(rc => rc.Name)
                .ToListAsync();

            return allRecipeCategoryNames;
        }
        public async Task AddRecipeCategoryAsync(RecipeCategoryAddFormModel model)
        {
            RecipeCategory recipeCategory = new RecipeCategory()
            {
                Name = model.Name
            };

            await dbContext.RecipeCategories
                .AddAsync(recipeCategory);
            await dbContext.SaveChangesAsync();
        }
        public async Task EditRecipeCategoryAsync(RecipeCategoryEditFormModel model)
        {
            RecipeCategory recipeCategory = await dbContext
                .RecipeCategories
                .Where(rc => rc.Id == model.Id)
                .FirstAsync();

            recipeCategory.Name = model.Name;

            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteRecipeCategoryById(int id)
        {
            RecipeCategory recipeCategory = await dbContext
                .RecipeCategories
                .Where(rc => rc.Id == id)
                .FirstAsync();

            dbContext.RecipeCategories.Remove(recipeCategory);
            await dbContext.SaveChangesAsync();
        }
        public async Task<RecipeCategoryEditFormModel> GetRecipeCategoryForEditByIdAsync(int id)
        {
            RecipeCategoryEditFormModel model = await dbContext
                .RecipeCategories
                .Where(rc => rc.Id == id)
                .Select(rc => new RecipeCategoryEditFormModel()
                {
                    Id = rc.Id,
                    Name = rc.Name
                })
                .FirstAsync();

            return model;
        }
        public Task<int> GetRecipeCategoryIdByNameAsync(string name)
        {
            return dbContext.RecipeCategories
                .AsNoTracking()
                .Where(rc => rc.Name.ToLower() == name.ToLower())
                .Select(rc => rc.Id)
                .FirstAsync();
        }
        public async Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId)
        {
            return await dbContext
                .RecipeCategories
                .AnyAsync(rc => rc.Id == recipeCategoryId);
        }
        public Task<bool> RecipeCategoryExistsByNameAsync(string name)
        {
            return dbContext.RecipeCategories
                .Where(rc => rc.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }
        public async Task<int> AllRecipeCategoriesCountAsync()
        {
            return await dbContext
                .RecipeCategories
                .CountAsync();
        }


        // Ingredient Categories Services
        public async Task<ICollection<IngredientCategorySelectViewModel>> AllIngredientCategoriesAsync()
        {
            ICollection<IngredientCategorySelectViewModel> allIngredientCategories = await dbContext
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
        public async Task<ICollection<string>> AllIngredientCategoryNamesAsync()
        {
            ICollection<string> allIngredientCategoryNames = await dbContext
                .IngredientCategories
                .AsNoTracking()
                .Select(ic => ic.Name)
                .ToListAsync();

            return allIngredientCategoryNames;
        }
        public async Task AddIngredientCategoryAsync(IngredientCategoryAddFormModel model)
        {
            IngredientCategory ingredientCategory = new IngredientCategory()
            {
                Name = model.Name
            };

            await dbContext.IngredientCategories
                .AddAsync(ingredientCategory);
            await dbContext.SaveChangesAsync();
        }
        public async Task EditIngredientCategoryAsync(IngredientCategoryEditFormModel model)
        {
            IngredientCategory ingredientCategory = await dbContext
                .IngredientCategories
                .Where(ic => ic.Id == model.Id)
                .FirstAsync();

            ingredientCategory.Name = model.Name;

            await dbContext.SaveChangesAsync();

        }
        public async Task DeleteIngredientCategoryById(int id)
        {
            IngredientCategory ingredientCategory = await dbContext
                .IngredientCategories
                .Where(ic => ic.Id == id)
                .FirstAsync();

            dbContext.IngredientCategories.Remove(ingredientCategory);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IngredientCategoryEditFormModel> GetIngredientCategoryForEditByIdAsync(int id)
        {
            IngredientCategoryEditFormModel model = await dbContext
                .IngredientCategories
                .Where(ic => ic.Id == id)
                .Select(ic => new IngredientCategoryEditFormModel()
                {
                    Id = ic.Id,
                    Name = ic.Name
                })
                .FirstAsync();

            return model;
        }
        public Task<int> GetIngredientCategoryIdByNameAsync(string name)
        {
            return dbContext.IngredientCategories
                .Where(ic => ic.Name.ToLower() == name.ToLower())
                .Select(ic => ic.Id)
                .FirstAsync();
        }
        public async Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId)
        {
            bool exists = await dbContext
                .IngredientCategories
                .AsNoTracking()
                .AnyAsync(c => c.Id == ingredientCategoryId);

            return exists;
        }
        public async Task<bool> IngredientCategoryExistsByNameAsync(string name)
        {
            return await dbContext.IngredientCategories
                .Where(ic => ic.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }
        public async Task<int> AllIngredientCategoriesCountAsync()
        {
            return await dbContext
                .IngredientCategories
                .CountAsync();
        }

    }
}
