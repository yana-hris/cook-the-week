namespace CookTheWeek.Services.Data.Factories
{
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.GeneralApplicationConstants;
    public class MealViewModelFactory : IMealViewModelFactory
    {
        private readonly IRecipeService recipeService;
        private readonly IMealService mealService;
        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<MealViewModelFactory> logger;

        public MealViewModelFactory(IRecipeService recipeService,
                                      ILogger<MealViewModelFactory> logger,
                                      IIngredientAggregatorHelper ingredientHelper,
                                      IMealService mealService)
        {
            this.recipeService = recipeService;
            this.logger = logger;
            this.mealService = mealService;
            this.ingredientHelper = ingredientHelper;
        }


        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId)
        {
            Meal meal = await mealService.GetByIdAsync(mealId);

            decimal servingSizeMultiplier = ingredientHelper.CalculateServingSizeMultiplier(meal.ServingSize, meal.Recipe.Servings);
            
            var adjustedIngredients = ingredientHelper.CreateAdjustedIngredientCollection(meal.Recipe.RecipesIngredients, servingSizeMultiplier);

            var ingredientsByCategories = await ingredientHelper
                .AggregateIngredientsByCategory<MealIngredientDetailsViewModel>(adjustedIngredients, RecipeAndMealDetailedProductListCategoryDictionary);
            
            return new MealDetailsViewModel
            {
                Id = meal.Id,
                Title = meal.Recipe.Title,
                ImageUrl = meal.Recipe.ImageUrl,
                Description = meal.Recipe.Description,
                CookingTime = meal.Recipe.TotalTime,
                CategoryName = meal.Recipe.Category.Name,
                CookingSteps = meal.Recipe.Steps.Select(st => new StepViewModel
                {
                    Id = st.Id,
                    Description = st.Description
                }).ToList(),
                ServingSize = meal.ServingSize,
                IngredientsByCategories = ingredientsByCategories
            };
        }

    }
}
