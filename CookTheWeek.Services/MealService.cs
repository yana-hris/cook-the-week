namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Data.Interfaces;
    using Web.ViewModels.Meal;
    using Web.ViewModels.ShoppingList;
    using Web.ViewModels.Step;

    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.IngredientHelper;

    public class MealService : IMealService
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<bool> ExistsByIdAsync(int id)
        {
            return this.dbContext
                .Meals
                .AnyAsync(m => m.Id == id);
        }
        public async Task<MealDetailsViewModel> DetailsByIdAsync(int mealId)
        {
            var mealInfo = await this.dbContext
                .Meals
                .Where(m => m.Id == mealId)
                .FirstAsync();

            string recipeId = mealInfo.RecipeId.ToString();

            var recipe = await this.dbContext
                .Recipes
                .Include(r => r.Category)
                .Include(r => r.Steps)
                .Include(r => r.RecipesIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .AsNoTracking()
                .Where(r => r.Id.ToString() == recipeId)
                .FirstAsync();

            MealDetailsViewModel model = new MealDetailsViewModel()
            {
                Id = mealId,
                Title = recipe.Title,
                ImageUrl = recipe.ImageUrl,
                Description = recipe.Description,
                CookingTime = recipe.TotalTime,
                CategoryName = recipe.Category.Name,
                CookingSteps = recipe.Steps.Select(st => new StepViewModel()
                {
                    Id = st.Id,
                    Description = st.Description
                }).ToList(),    
            };
            model.ServingSize = mealInfo.ServingSize;

            decimal servingSizeMultiplier = mealInfo.ServingSize * 1.0m / recipe.Servings * 1.0m;

            var ingredients = new List<ProductServiceModel>();

            foreach (var ri in recipe.RecipesIngredients)
            {
                ProductServiceModel ingredient = new ProductServiceModel()
                {
                    Name = ri.Ingredient.Name,
                    MeasureId = ri.MeasureId,
                    Qty = ri.Qty * servingSizeMultiplier,
                    CategoryId = ri.Ingredient.CategoryId,
                    SpecificationId = ri.SpecificationId
                };

                ingredients.Add(ingredient);
            }

            ICollection<ProductListViewModel> ingredientsByCategories = new List<ProductListViewModel>();

            var measures = await this.dbContext.Measures.ToListAsync();
            var specifications = await this.dbContext.Specifications.ToListAsync();


            for (int i = 0; i < ProductListCategoryNames.Length; i++)
            {
                int[] categoriesArr = ProductListCategoryIds[i];

                ProductListViewModel ingredientModel = new ProductListViewModel()
                {
                    Title = ProductListCategoryNames[i],
                    Products = ingredients
                            .Where(p => categoriesArr.Contains(p.CategoryId))
                            .Select(p => new ProductViewModel()
                            {
                                Qty = FormatIngredientQty(p.Qty),
                                Measure = measures.Where(m => m.Id == p.MeasureId).Select(m => m.Name).First(),
                                Name = p.Name,
                                Specification = specifications.Where(s => s.Id == p.SpecificationId).Select(s => s.Description).FirstOrDefault()
                            }).ToList()
                };

                ingredientsByCategories.Add(ingredientModel);
            }
            model.IngredientsByCategories = ingredientsByCategories;
            return model;
        }

    }
}
