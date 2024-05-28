namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Data.Interfaces;
    using Web.ViewModels.ShoppingList;

    using static Common.GeneralApplicationConstants;
    using static Common.HelperMethods.IngredientHelper;

    public class ShoppingListService : IShoppingListService
    {
        private readonly CookTheWeekDbContext dbContext;

        public ShoppingListService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ShoppingListViewModel> GetByMealPlanIdAsync(string id)
        {
            ShoppingListViewModel model = await this.dbContext
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

            var meals = await this.dbContext.Meals
                .Where(m => m.MealPlanId.ToString() == id)
                .ToListAsync();

            var products = new List<ProductServiceModel>();

            foreach (var meal in meals)
            {
                var recipeId = meal.RecipeId;
                int mealServings = meal.ServingSize;

                var recipe = await this.dbContext.Recipes
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
                        MeasureId = ri.MeasureId,
                        Qty = ri.Qty * servingSizeMultiplier,
                        CategoryId = ri.Ingredient.CategoryId,
                        SpecificationId = ri.SpecificationId,
                    };

                    // Check if a product with the same measure is already added to the list, otherwise add it
                    if (products.Any(p => p.Name == product.Name &&
                                     p.MeasureId == product.MeasureId))
                    {  
                        var existingProduct = products.First(p => p.Name == product.Name);

                        // check if the product has the same specification if any at all
                        if ((product.SpecificationId.HasValue && existingProduct.SpecificationId.HasValue &&
                            product.SpecificationId == existingProduct.SpecificationId) ||
                            !product.SpecificationId.HasValue && !existingProduct.SpecificationId.HasValue)
                        {
                            existingProduct.Qty += product.Qty;
                        }
                    }
                    else
                    {
                        products.Add(product);
                    }
                }

            }

            ICollection<ProductListViewModel> productsByCategories = new List<ProductListViewModel>();

            var measures = await this.dbContext.Measures.ToListAsync();
            var specifications = await this.dbContext.Specifications.ToListAsync();
            

            for(int i = 0; i < ProductListCategoryNames.Length; i++)
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
