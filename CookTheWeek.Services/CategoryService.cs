namespace CookTheWeek.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.Category;
    using CookTheWeek.Data.Models;

    public class CategoryService : ICategoryService
    {
        private readonly CookTheWeekDbContext dbContext;

        public CategoryService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Recipe Categories Service

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
        public async Task<ICollection<string>> AllRecipeCategoryNamesAsync()
        {
            ICollection<string> allRecipeCategoryNames = await this.dbContext
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

            await this.dbContext.RecipeCategories
                .AddAsync(recipeCategory);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task EditRecipeCategoryAsync(RecipeCategoryEditFormModel model)
        {
            RecipeCategory recipeCategory = await this.dbContext
                .RecipeCategories
                .Where(rc => rc.Id == model.Id)
                .FirstAsync();

            recipeCategory.Name = model.Name;

            await this.dbContext.SaveChangesAsync();
        }
        public async Task DeleteRecipeCategoryById(int id)
        {
            RecipeCategory recipeCategory = await this.dbContext
                .RecipeCategories
                .Where(rc => rc.Id == id)
                .FirstAsync();

            this.dbContext.RecipeCategories.Remove(recipeCategory);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task<RecipeCategoryEditFormModel> GetRecipeCategoryForEditByIdAsync(int id)
        {
            RecipeCategoryEditFormModel model = await this.dbContext
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
            return this.dbContext.RecipeCategories
                .AsNoTracking()
                .Where(rc => rc.Name.ToLower() == name.ToLower())
                .Select(rc => rc.Id)
                .FirstAsync();
        }
        public async Task<bool> RecipeCategoryExistsByIdAsync(int recipeCategoryId)
        {
            return await this.dbContext
                .RecipeCategories
                .AnyAsync(rc => rc.Id == recipeCategoryId);
        }
        public Task<bool> RecipeCategoryExistsByNameAsync(string name)
        {
            return this.dbContext.RecipeCategories
                .Where(rc => rc.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }
        public async Task<int> AllRecipeCategoriesCountAsync()
        {
            return await this.dbContext
                .RecipeCategories
                .CountAsync();
        }


        // Ingredient Categories Service

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
        public async Task<ICollection<string>> AllIngredientCategoryNamesAsync()
        {
            ICollection<string> allIngredientCategoryNames = await this.dbContext
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

            await this.dbContext.IngredientCategories
                .AddAsync(ingredientCategory);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task EditIngredientCategoryAsync(IngredientCategoryEditFormModel model)
        {
            IngredientCategory ingredientCategory = await this.dbContext
                .IngredientCategories
                .Where(ic => ic.Id == model.Id)
                .FirstAsync();

            ingredientCategory.Name = model.Name;

            await this.dbContext.SaveChangesAsync();

        }
        public async Task DeleteIngredientCategoryById(int id)
        {
            IngredientCategory ingredientCategory = await this.dbContext
                .IngredientCategories
                .Where(ic => ic.Id == id)
                .FirstAsync();

            this.dbContext.IngredientCategories.Remove(ingredientCategory);
            await this.dbContext.SaveChangesAsync();
        }
        public async Task<IngredientCategoryEditFormModel> GetIngredientCategoryForEditByIdAsync(int id)
        {
            IngredientCategoryEditFormModel model = await this.dbContext
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
            return this.dbContext.IngredientCategories
                .Where(ic => ic.Name.ToLower() == name.ToLower())
                .Select(ic => ic.Id)
                .FirstAsync();
        }
        public async Task<bool> IngredientCategoryExistsByIdAsync(int ingredientCategoryId)
        {
            bool exists = await this.dbContext
                .IngredientCategories
                .AsNoTracking()
                .AnyAsync(c => c.Id == ingredientCategoryId);

            return exists;
        }
        public async Task<bool> IngredientCategoryExistsByNameAsync(string name)
        {
            return await this.dbContext.IngredientCategories
                .Where(ic => ic.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }
        public async Task<int> AllIngredientCategoriesCountAsync()
        {
            return await this.dbContext
                .IngredientCategories
                .CountAsync();
        }

    }
}
