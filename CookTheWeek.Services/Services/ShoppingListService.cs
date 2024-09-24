namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Data.Services.Interfaces;
    using Web.ViewModels.ShoppingList;

    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.IngredientHelper;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class ShoppingListService : IShoppingListService
    {
        private readonly CookTheWeekDbContext dbContext;

        public ShoppingListService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ShoppingListViewModel> GetByMealPlanIdAsync(string id)
        {
            ShoppingListViewModel model = await dbContext
                .MealPlans
                .Where(mp => mp.Id.ToString() == id)
                .Select(mp => new ShoppingListViewModel()
                {
                    Id = id,
                    Title = mp.Name,
                    StartDate = mp.StartDate.ToString(MealDateFormat),
                    EndDate = mp.StartDate.AddDays(6).ToString(MealDateFormat),
                })
                .FirstAsync();

            var meals = await dbContext.Meals
                .Where(m => m.MealPlanId.ToString() == id)
                .ToListAsync();

            var products = new List<ProductServiceModel>();

            foreach (var meal in meals)
            {
                var recipeId = meal.RecipeId;
                int mealServings = meal.ServingSize;

                var recipe = await dbContext.Recipes
                    .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .FirstAsync(r => r.Id.Equals(recipeId));

                int recipeServings = recipe.Servings;

                decimal servingSizeMultiplier = mealServings * 1.0m / recipeServings * 1.0m;

                foreach (var ri in recipe.RecipesIngredients)
                {
                    ProductServiceModel product = new ProductServiceModel()
                    {
                        Name = ri.Ingredient.Name,
                        CategoryId = ri.Ingredient.CategoryId,
                        MeasureId = ri.MeasureId,
                        Qty = ri.Qty * servingSizeMultiplier,
                        SpecificationId = ri.SpecificationId,
                    };

                    // Check if a product with the same measure is already added to the list, otherwise add it
                    if (products.Any(p => p.Name == product.Name &&
                                     p.MeasureId == product.MeasureId &&
                                     p.SpecificationId == product.SpecificationId))
                    {
                        var existingProduct = products
                            .First(p => p.Name == product.Name &&
                                        p.MeasureId == product.MeasureId &&
                                        p.SpecificationId == product.SpecificationId);

                        existingProduct.Qty += product.Qty;

                    }
                    else
                    {
                        products.Add(product);
                    }
                }

            }

            ICollection<ProductListViewModel> productsByCategories = new List<ProductListViewModel>();

            var measures = await dbContext.Measures.ToListAsync();
            var specifications = await dbContext.Specifications.ToListAsync();


            for (int i = 0; i < ProductListCategoryNames.Length; i++)
            {
                int[] categoriesArr = ProductListCategoryIds[i];

                ProductListViewModel productModel = new ProductListViewModel()
                {
                    Title = ProductListCategoryNames[i],
                    Products = products
                            .Where(p => categoriesArr.Contains(p.CategoryId))
                            .Select(p => new ProductViewModel()
                            {
                                Qty = FormatIngredientQty(p.Qty),
                                Measure = measures.Where(m => m.Id == p.MeasureId).Select(m => m.Name).First(),
                                Name = p.Name,
                                Specification = specifications.Where(s => s.Id == p.SpecificationId).Select(s => s.Description).FirstOrDefault()
                            }).ToList()
                };

                productsByCategories.Add(productModel);
            }

            model.ProductsByCategories = productsByCategories;

            return model;

        }
    }
}
