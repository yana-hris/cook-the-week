namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.IngredientHelper;

    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;
        private readonly IRecipeService recipeService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly ILogger<MealService> logger;

        public MealService(IMealRepository mealRepository,
            IRecipeService recipeService,
            IRecipeIngredientService recipeIngredientService,
            ILogger<MealService> logger)
        {
            this.mealRepository = mealRepository;
            this.recipeService = recipeService;
            this.recipeIngredientService = recipeIngredientService;
            this.logger = logger;   
        }
        

        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> GetForDetailsAsync(int mealId)
        {
            // Retrieve and validate the meal
            Meal meal = await GetMealByIdAsync(mealId);

            // Retrieve the associated recipe
            Recipe recipe = await recipeService.GetForMealByIdAsync(meal.RecipeId.ToString());

            // Calculate serving size multiplier
            decimal servingSizeMultiplier = CalculateServingSizeMultiplier(meal.ServingSize, recipe.Servings);

            // Process ingredients
            var ingredients = ProcessIngredients(recipe.RecipesIngredients, servingSizeMultiplier);

            // Group ingredients by categories and build the model
            var model = await BuildMealDetailsViewModel(meal, recipe, ingredients);

            return model;
        }

        /// <inheritdoc/>
        public async Task DeleteAllByMealPlanIdAsync(string mealplanId)
        {
            var mealsToDelete = await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(mealplanId, m.MealPlanId))
                .ToListAsync();

            await mealRepository.DeleteAll(mealsToDelete);

        }

        /// <inheritdoc/>
        public async Task DeleteByRecipeIdAsync(string recipeId)
        {
            var mealsToDelete = await mealRepository
                .GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .ToListAsync();

            await mealRepository.DeleteAll(mealsToDelete);

        }

        /// <inheritdoc/>
        public async Task AddAllAsync(IList<MealAddFormModel> meals)
        {
            ICollection<Meal> newMeals = new List<Meal>();

            foreach (var meal in meals)
            {
                Meal newMeal = new Meal()
                {
                    RecipeId = Guid.Parse(meal.RecipeId),
                    ServingSize = meal.Servings,
                    CookDate = DateTime.ParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture),
                };

                newMeals.Add(newMeal);
            }

            await mealRepository.AddAllAsync(newMeals);
        }

        // TODO: Consolidate model creation in a factory?

        /// <inheritdoc/>
        public async Task<MealAddFormModel> CreateMealAddFormModelAsync(MealServiceModel meal)
        {
            // Retrieve the recipe from database
            Recipe recipe = await recipeService.GetForMealByIdAsync(meal.RecipeId);

            MealAddFormModel model = new MealAddFormModel()
            {
                RecipeId = recipe.Id.ToString(),
                Title = recipe.Title,
                Servings = recipe.Servings,
                ImageUrl = recipe.ImageUrl,
                CategoryName = recipe.Category.Name,
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

        /// <summary>
        /// Returns the total count of all meals, cooked by a given recipeId
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>int or 0</returns>
        public async Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .CountAsync();
        }

        // HELPER METHODS:

        /// <summary>
        /// A helper method that gets the Meal by a given id or throws an Exception
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>Meal</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        private async Task<Meal> GetMealByIdAsync(int mealId)
        {
            Meal? meal = await mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            return meal;
        }

        /// <summary>
        /// Helper method that calculates the serving size multiplier for a given meal 
        /// </summary>
        /// <param name="mealServingSize"></param>
        /// <param name="recipeServings"></param>
        /// <returns>decimal</returns>
        private decimal CalculateServingSizeMultiplier(int mealServingSize, int recipeServings)
        {
            return mealServingSize * 1.0m / recipeServings * 1.0m;
        }

        /// <summary>
        /// Processes the ingredient qty for each meal ingredient according to a serving size multiplier
        /// </summary>
        /// <param name="recipeIngredients"></param>
        /// <param name="servingSizeMultiplier"></param>
        /// <returns>A collection of ingredients of type ProductServiceModel</returns>
        private List<ProductServiceModel> ProcessIngredients(IEnumerable<RecipeIngredient> recipeIngredients, decimal servingSizeMultiplier)
        {
            var ingredients = recipeIngredients.Select(ri => new ProductServiceModel()
            {
                Name = ri.Ingredient.Name,
                MeasureId = ri.MeasureId,
                Qty = ri.Qty * servingSizeMultiplier,
                CategoryId = ri.Ingredient.CategoryId,
                SpecificationId = ri.SpecificationId
            }).ToList();

            return ingredients;
        }

        /// <summary>
        /// Builds the viewmodel to return to Details view
        /// </summary>
        /// <param name="meal"></param>
        /// <param name="recipe"></param>
        /// <param name="ingredients"></param>
        /// <returns>A MealDetailsViewModel</returns>
        private async Task<MealDetailsViewModel> BuildMealDetailsViewModel(Meal meal, Recipe recipe, List<ProductServiceModel> ingredients)
        {
            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            var ingredientsByCategories = BuildIngredientsByCategories(ingredients, measures, specifications);

            return new MealDetailsViewModel
            {
                Id = meal.Id,
                Title = recipe.Title,
                ImageUrl = recipe.ImageUrl,
                Description = recipe.Description,
                CookingTime = recipe.TotalTime,
                CategoryName = recipe.Category.Name,
                CookingSteps = recipe.Steps.Select(st => new StepViewModel
                {
                    Id = st.Id,
                    Description = st.Description
                }).ToList(),
                ServingSize = meal.ServingSize,
                IngredientsByCategories = ingredientsByCategories
            };
        }

        /// <summary>
        /// Processes the ingredients by groups and returns a collection of collections of ingredients, grouped by categories, defined in a constant dictionary
        /// </summary>
        /// <param name="ingredients"></param>
        /// <param name="measures"></param>
        /// <param name="specifications"></param>
        /// <returns>A collection of ProductListViewModels</returns>
        private ICollection<ProductListViewModel> BuildIngredientsByCategories(List<ProductServiceModel> ingredients, List<Measure> measures, List<Specification> specifications)
        {
            var ingredientsByCategories = new List<ProductListViewModel>();

            for (int i = 0; i < ProductListCategoryNames.Length; i++)
            {
                int[] categoriesArr = ProductListCategoryIds[i];

                var productListViewModel = new ProductListViewModel
                {
                    Title = ProductListCategoryNames[i],
                    Products = ingredients
                        .Where(p => categoriesArr.Contains(p.CategoryId))
                        .Select(p => new ProductViewModel
                        {
                            Qty = FormatIngredientQty(p.Qty),
                            Measure = measures.First(m => m.Id == p.MeasureId).Name,
                            Name = p.Name,
                            Specification = specifications.FirstOrDefault(s => s.Id == p.SpecificationId)?.Name
                        }).ToList()
                };

                ingredientsByCategories.Add(productListViewModel);
            }

            return ingredientsByCategories;
        }
    }
}
