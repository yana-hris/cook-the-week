namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Helpers;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.ShoppingList;
    using CookTheWeek.Web.ViewModels.Step;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;

        private readonly IRecipeService recipeService;
        private readonly IRecipeIngredientService recipeIngredientService;

        private readonly IIngredientAggregatorHelper ingredientHelper;
        private readonly ILogger<MealService> logger;

        public MealService(IMealRepository mealRepository,
            IRecipeService recipeService,
            IRecipeIngredientService recipeIngredientService,
            IIngredientAggregatorHelper ingredientHelper,
            ILogger<MealService> logger)
        {
            this.mealRepository = mealRepository;

            this.recipeService = recipeService;
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientHelper = ingredientHelper;
            this.logger = logger;   
        }
        

        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> GetForDetailsAsync(int mealId)
        {
            Meal meal = await GetMealByIdAsync(mealId);
           
            decimal servingSizeMultiplier = CalculateServingSizeMultiplier(meal.ServingSize, meal.Recipe.Servings);

            // Process ingredients
            var ingredients = ProcessIngredients(meal.Recipe.RecipesIngredients, servingSizeMultiplier);

            // Group ingredients by categories and build the model
            var model = await BuildMealDetailsViewModel(meal, ingredients);

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

            if (mealsToDelete.Any())
            {
                await mealRepository.DeleteAll(mealsToDelete);
            }
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

        /// <inheritdoc/>
        public async Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .CountAsync();
        }

        // HELPER METHODS:

        /// <summary>
        /// A helper method that gets the Meal (including all its ingredients) by a given id or throws an Exception
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>Meal</returns>
        /// <exception cref="RecordNotFoundException"></exception>
        private async Task<Meal> GetMealByIdAsync(int mealId)
        {
            Meal? meal = await mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                logger.LogError($"Meal with id {mealId} not found in the database. Error occured in method {nameof(GetMealByIdAsync)} in MealService.");
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
                CategoryId = ri.Ingredient.CategoryId,
                Name = ri.Ingredient.Name,
                Qty = ri.Qty * servingSizeMultiplier,
                MeasureId = ri.MeasureId,
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
        private async Task<MealDetailsViewModel> BuildMealDetailsViewModel(Meal meal, List<ProductServiceModel> ingredients)
        {
            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            // TODO: check if this works as expected
            var ingredientsByCategories = ingredientHelper.AggregateIngredientsByCategory(ingredients, measures, specifications);

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

        /// <summary>
        /// Processes the ingredients by groups and returns a collection of collections of ingredients, grouped by categories, defined in a constant dictionary
        /// </summary>
        /// <param name="ingredients"></param>
        /// <param name="measures"></param>
        /// <param name="specifications"></param>
        /// <returns>A collection of ProductListViewModels</returns>
        /// 
        // TODO: Check if the newly implemented method AggregateIngredientsByCategory works right before deleting the old logic
        //private ICollection<ProductListViewModel> BuildIngredientsByCategories(List<ProductServiceModel> ingredients, ICollection<RecipeIngredientSelectMeasureViewModel> measures, ICollection<RecipeIngredientSelectSpecificationViewModel> specifications)
        //{
        //    var ingredientsByCategories = new List<ProductListViewModel>();

        //    for (int i = 0; i < ProductListCategoryNames.Length; i++)
        //    {
        //        int[] categoriesArr = ProductListCategoryIds[i];

        //        ProductListViewModel productListViewModel = new ProductListViewModel
        //        {
        //            Title = ProductListCategoryNames[i],
        //            Products = ingredients
        //                .Where(p => categoriesArr.Contains(p.CategoryId))
        //                .Select(p => new ProductViewModel
        //                {
        //                    Qty = FormatIngredientQty(p.Qty),
        //                    Measure = measures.First(m => m.Id == p.MeasureId).Name,
        //                    Name = p.Name,
        //                    Specification = specifications.First(sp => sp.Id == p.SpecificationId).Name ?? ""
        //                }).ToList()
        //        };

        //        ingredientsByCategories.Add(productListViewModel);
        //    }

        //    return ingredientsByCategories;
        //}
    }
}
