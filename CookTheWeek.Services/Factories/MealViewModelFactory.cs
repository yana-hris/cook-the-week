namespace CookTheWeek.Services.Data.Factories
{
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.Step;
    using CookTheWeek.Web.ViewModels.SupplyItem;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;

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
        public async Task<MealFormModel> CreateMealAddFormModelAsync(MealServiceModel meal)
        {
            // Retrieve the recipe from database
            if (Guid.TryParse(meal.RecipeId, out Guid guidMealId))
            {
                try
                {
                    Recipe recipe = await recipeService.GetForMealByIdAsync(guidMealId);

                    MealFormModel model = new MealFormModel()
                    {
                        RecipeId = recipe.Id,
                        Title = recipe.Title,
                        Servings = recipe.Servings,
                        CookingTime = FormatCookingTime(recipe.TotalTimeMinutes),
                        ImageUrl = recipe.ImageUrl,
                        CategoryName = recipe.Category.Name
                    };

                    // Make sure all select menus are filled with data
                    if (model.SelectDates == null || model.SelectDates.Count() == 0)
                    {
                        model.SelectDates = DateGenerator.GenerateNext7Days();
                    }

                    if (model.SelectServingOptions == null || model.SelectServingOptions.Count() == 0)
                    {
                        model.SelectServingOptions = ServingsOptions;
                    }
                    model.Date = model.SelectDates!.First();

                    return model;
                }
                catch (RecordNotFoundException)
                {
                    logger.LogError($"Invalid recipeId, old, inexisting or deleted recipe.");
                    throw;
                }
            }

            throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);

        }

        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> CreateMealDetailsViewModelAsync(int mealId, bool isMealPlanFinished)
        {
            Meal meal = await mealService.GetByIdAsync(mealId);

            decimal servingSizeMultiplier = ingredientHelper.CalculateServingSizeMultiplier(meal.ServingSize, meal.Recipe.Servings);
            
            var adjustedIngredients = ingredientHelper.CreateAdjustedIngredientCollection(meal.Recipe.RecipesIngredients, servingSizeMultiplier);

            var ingredientsByCategories = await ingredientHelper
                .AggregateIngredientsByCategory<IngredientItemViewModel>(adjustedIngredients, RecipeAndMealDetailedProductListCategoryDictionary);
            
            return new MealDetailsViewModel
            {
                Id = meal.Id,
                Title = meal.Recipe.Title,
                ImageUrl = meal.Recipe.ImageUrl,
                Description = meal.Recipe.Description,
                DifficultyLevel = meal.Recipe.DifficultyLevel.HasValue ? meal.Recipe.DifficultyLevel.ToString() : "",
                IsMealPlanFinished = isMealPlanFinished,
                CookingTime = FormatCookingTime(meal.Recipe.TotalTimeMinutes),
                CookingDate = meal.CookDate.ToString(MealDateFormat),
                CategoryName = meal.Recipe.Category.Name,
                IsCooked = meal.IsCooked,
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
