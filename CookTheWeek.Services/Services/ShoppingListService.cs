namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Models.SupplyItem;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels;
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    using static Common.GeneralApplicationConstants;

    public class ShoppingListService : IShoppingListService
    {
        private readonly IMealPlanService mealPlanService;

        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<ShoppingListService> logger;

        public ShoppingListService(IMealPlanService mealPlanService, 
            IIngredientAggregatorHelper ingredientHelper,
            ILogger<ShoppingListService> logger)
        {
            this.logger = logger;
            this.mealPlanService = mealPlanService;
            this.ingredientHelper = ingredientHelper; 
        }

        // TODO: move to viewmodel factory
        public async Task<ShoppingListViewModel> TryGetShoppingListDataByMealPlanIdAsync(Guid id)
        {
            MealPlan mealplan = await mealPlanService.TryGetAsync(id);

            ShoppingListViewModel model = new ShoppingListViewModel
            {
                MealPlanId = mealplan.Id.ToString(),
                Title = mealplan.Name,
                StartDate = mealplan.StartDate.ToString(MealDateFormat),
                EndDate = mealplan.StartDate.AddDays(6).ToString(MealDateFormat),
                ShopItemsByCategories = new List<SupplyItemListModel<ShopItemViewModel>>()
            };

            // Dictionary for fast product lookup
            var productDict = new Dictionary<(string Name, int MeasureId, int? SpecificationId), SupplyItemServiceModel>();


            foreach (var meal in mealplan.Meals)
            {
                int mealServings = meal.ServingSize;
                int recipeServings = meal.Recipe.Servings;

                decimal servingSizeMultiplier = ingredientHelper.CalculateServingSizeMultiplier(mealServings, recipeServings);

                foreach (var ri in meal.Recipe.RecipesIngredients)
                {
                    var key = (ri.Ingredient.Name, ri.MeasureId, ri.SpecificationId);

                    if (productDict.TryGetValue(key, out var existingProduct))
                    {
                        existingProduct.Qty += ri.Qty * servingSizeMultiplier;
                    }
                    else
                    {
                        productDict[key] = new SupplyItemServiceModel
                        {
                            CategoryId = ri.Ingredient.CategoryId,
                            Name = ri.Ingredient.Name,
                            Qty = ri.Qty * servingSizeMultiplier,
                            MeasureId = ri.MeasureId,
                            SpecificationId = ri.SpecificationId
                        };
                    }
                }
            }

            var products = productDict.Values.ToList();

            model.ShopItemsByCategories = await ingredientHelper.AggregateIngredientsByCategory<ShopItemViewModel>(products, ShoppingListCategoryGroupDictionary);

            return model;

        }
    }
}
