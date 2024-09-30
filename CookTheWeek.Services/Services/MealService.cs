namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using Web.ViewModels.Meal;
    using Web.ViewModels.ShoppingList;
    using Web.ViewModels.Step;

    using static Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.EntityValidationConstants.Recipe;
    using static Common.HelperMethods.IngredientHelper;
    using static Common.ExceptionMessagesConstants;

    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;
        private readonly IRecipeService recipeService;
        private readonly IRecipeIngredientService recipeIngredientService;

        public MealService(IMealRepository mealRepository,
            IRecipeService recipeService,
            IRecipeIngredientService recipeIngredientService)
        {
            this.mealRepository = mealRepository;
            this.recipeService = recipeService;
            this.recipeIngredientService = recipeIngredientService;
        }

        // TODO: consolidate model creation in the factory to reuse methods
        /// <inheritdoc/>
        public async Task<MealDetailsViewModel> GetForDetailsAsync(int mealId)
        {
            // Get Meal from mealRepo by Id
            Meal? meal = await mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            string recipeId = meal.RecipeId.ToString();
            
            Recipe recipe = await recipeService.GetForMealByIdAsync(recipeId);

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
            model.ServingSize = meal.ServingSize;

            decimal servingSizeMultiplier = meal.ServingSize * 1.0m / recipe.Servings * 1.0m;

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

            var measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            var specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();


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
                                Specification = specifications.Where(s => s.Id == p.SpecificationId).Select(s => s.Name).FirstOrDefault()
                            }).ToList()
                };

                ingredientsByCategories.Add(ingredientModel);
            }
            model.IngredientsByCategories = ingredientsByCategories;
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
    }
}
