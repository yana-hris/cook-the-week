namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    using static Common.GeneralApplicationConstants;

    public class ShoppingListService : IShoppingListService
    {
        private readonly IMealPlanService mealPlanService;
        private readonly IRecipeIngredientService recipeIngredientService;

        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<ShoppingListService> logger;

        public ShoppingListService(IMealPlanService mealPlanService, 
            IIngredientAggregatorHelper ingredientHelper,
            IRecipeIngredientService recipeIngredientService,
            ILogger<ShoppingListService> logger)
        {
            this.logger = logger;
            this.mealPlanService = mealPlanService;
            this.ingredientHelper = ingredientHelper;
            this.recipeIngredientService = recipeIngredientService; 
        }
        public async Task<ShoppingListViewModel> TryGetShoppingListDataByMealPlanIdAsync(string id)
        {
            MealPlan mealplan = await mealPlanService.GetByIdAsync(id);

            ShoppingListViewModel model = new ShoppingListViewModel
            {
                MealPlanId = mealplan.Id.ToString(),
                Title = mealplan.Name,
                StartDate = mealplan.StartDate.ToString(MealDateFormat),
                EndDate = mealplan.StartDate.AddDays(6).ToString(MealDateFormat),
                ProductsByCategories = new List<ProductListViewModel>()
            };

            // Dictionary for fast product lookup
            var productDict = new Dictionary<(string Name, int MeasureId, int? SpecificationId), ProductServiceModel>();


            foreach (var meal in mealplan.Meals)
            {
                int mealServings = meal.ServingSize;
                int recipeServings = meal.Recipe.Servings;

                decimal servingSizeMultiplier = mealServings * 1.0m / recipeServings * 1.0m;

                foreach (var ri in meal.Recipe.RecipesIngredients)
                {
                    var key = (ri.Ingredient.Name, ri.MeasureId, ri.SpecificationId);

                    if (productDict.TryGetValue(key, out var existingProduct))
                    {
                        existingProduct.Qty += ri.Qty * servingSizeMultiplier;
                    }
                    else
                    {
                        productDict[key] = new ProductServiceModel
                        {
                            Name = ri.Ingredient.Name,
                            CategoryId = ri.Ingredient.CategoryId,
                            MeasureId = ri.MeasureId,
                            Qty = ri.Qty * servingSizeMultiplier,
                            SpecificationId = ri.SpecificationId
                        };
                    }
                }
            }

            var products = productDict.Values.ToList();

            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            model.ProductsByCategories = ingredientHelper.AggregateIngredientsByCategory(products, measures, specifications);

            return model;

        }
    }
}
