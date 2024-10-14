namespace CookTheWeek.Services.Data.Factories
{
    using System.Globalization;

    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.Extensions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;


    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;

    public class MealPlanViewModelFactory : IMealPlanViewModelFactory
    {
        private readonly IMealService mealService;
        private readonly IMealPlanService mealplanService;
        private readonly ILogger<RecipeViewModelFactory> logger;
        private readonly IRecipeService recipeService;

        public MealPlanViewModelFactory(IRecipeService recipeService,
                                      ILogger<RecipeViewModelFactory> logger,
                                      IMealPlanService mealplanService,
                                      IMealService mealService)
        {
            this.logger = logger;
            this.mealplanService = mealplanService;
            this.mealService = mealService;
            this.recipeService = recipeService;
        }

        /// <inheritdoc/>
        public async Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel serviceModel)
        {
            MealPlanAddFormModel model = new MealPlanAddFormModel();

            // Ensure all fields are initiated and filled with correct data
            if (model.StartDate == default)
            {
                model.StartDate = DateTime.Now;
            }

            if (model.Meals == null)
            {
                model.Meals = new List<MealAddFormModel>();
            }

            foreach (var meal in serviceModel.Meals)
            {
                MealAddFormModel currentMeal = await CreateMealAddFormModelAsync(meal);
                model.Meals.Add(currentMeal);
            }

            return model;
        }

        /// <inheritdoc/>
        public async Task<MealPlanDetailsViewModel> CreateMealPlanDetailsViewModelAsync(Guid mealplanId)
        {
            MealPlan mealPlan = await mealplanService.TryGetAsync(mealplanId);

            MealPlanDetailsViewModel model = new MealPlanDetailsViewModel()
            {
                Id = mealPlan.Id.ToString(),
                Name = mealPlan.Name,
                OwnerId = mealPlan.OwnerId,
                IsFinished = mealPlan.IsFinished,
                Meals = mealPlan.Meals.Select(mpm => new MealViewModel()
                {
                    Id = mpm.Id.ToString(),
                    RecipeId = mpm.RecipeId.ToString(),
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                }).ToList(),
                TotalServings = mealPlan.Meals.Sum(mpm => mpm.ServingSize),
                TotalCookingDays = mealPlan.Meals.Select(mpm => mpm.CookDate.Date).Distinct().Count(),
                TotalIngredients = mealPlan.Meals.Sum(m => m.Recipe.RecipesIngredients.Count),
                TotalCookingTimeMinutes = mealPlan.Meals.Sum(m => (int)m.Recipe.TotalTime.TotalMinutes),
            };

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllViewModel>> CreateMyMealPlansViewModelAsync()
        {
            var userMealplans = await mealplanService.GetAllMineAsync();

            var model = userMealplans
               .Select(mp => new MealPlanAllViewModel()
               {
                   Id = mp.Id.ToString(),
                   Name = mp.Name,
                   StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                   EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                   MealsCount = mp.Meals.Count,
                   IsFinished = mp.IsFinished
               }).ToList();

            return model;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> CreateAllActiveMealPlansAdminViewModelAsync()
        {
            ICollection<MealPlan> allActiveUserMealplans = await mealplanService.GetAllActiveAsync();
            return MapMealPlansToViewModels(allActiveUserMealplans);
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> CreateAllFinishedMealPlansAdminViewModelAsync()
        {
            ICollection<MealPlan> allFinishedUserMealplans = await mealplanService.GetAllFinishedAsync();
            return MapMealPlansToViewModels(allFinishedUserMealplans);
        }

        /// <inheritdoc/>
        public async Task<TFormModel> CreateMealPlanFormModelAsync<TFormModel>(Guid id)
            where TFormModel : IMealPlanFormModel, new()
        {
            MealPlan mealplan = await mealplanService.TryGetAsync(id);

            if (typeof(TFormModel) == typeof(MealPlanAddFormModel))
            {
                return (TFormModel)(object)MapMealPlanToAddModel(mealplan);
            }
            else if (typeof(TFormModel) == typeof(MealPlanEditFormModel))
            {
                return (TFormModel)(object)MapMealPlanToEditModel(mealplan);
            }

            throw new InvalidOperationException("Unsupported form model type.");
        }

        /// <inheritdoc/>
        public async Task<MealAddFormModel> CreateMealAddFormModelAsync(MealServiceModel meal)
        {
            // Retrieve the recipe from database
            if (Guid.TryParse(meal.RecipeId, out Guid guidMealId))
            {
                Recipe recipe = await recipeService.GetForMealByIdAsync(guidMealId);

                MealAddFormModel model = new MealAddFormModel()
                {
                    RecipeId = recipe.Id,
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

            throw new RecordNotFoundException(RecordNotFoundExceptionMessages.RecipeNotFoundExceptionMessage, null);

        }


        // HELPER METHODS:

        /// <summary>
        /// Maps a collection of MealPlan entities to a collection of MealPlanAllAdminViewModel objects.
        /// </summary>
        /// <param name="mealPlans">The collection of MealPlan entities to be mapped.</param>
        /// <returns>A collection of MealPlanAllAdminViewModel objects representing the meal plans.</returns>
        private static ICollection<MealPlanAllAdminViewModel> MapMealPlansToViewModels(ICollection<MealPlan> mealPlans)
        {
            return mealPlans
                .Select(mp => new MealPlanAllAdminViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToList();
        }

        /// <summary>
        /// Utility generic common mapping method
        /// </summary>
        /// <param name="mealplan"></param>
        /// <returns>T => An implementation of IMealPlanFormModel</returns>
        private static T MapMealPlanToFormModel<T>(MealPlan mealplan) where T : IMealPlanFormModel, new()
        {
            var model = new T()
            {
                Name = mealplan.Name,
                StartDate = mealplan.StartDate,
                Meals = mealplan.Meals.Select(mpm => new MealAddFormModel()
                {
                    RecipeId = mpm.RecipeId,
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                    SelectServingOptions = ServingsOptions
                }).ToList(),
            };

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);
            return model;
        }

        /// <summary>
        /// Utility method for mapping the Edit Form model (for edit case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanEditFormModel MapMealPlanToEditModel(MealPlan mealPlan)
        {
            var model = MapMealPlanToFormModel<MealPlanEditFormModel>(mealPlan);
            model.Id = mealPlan.Id;
            return model;
        }


        /// <summary>
        /// Utility method for mapping the Add Form model (for copy case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanAddFormModel MapMealPlanToAddModel(MealPlan mealPlan)
        {
            var model = MapMealPlanToFormModel<MealPlanAddFormModel>(mealPlan);

            model.StartDate = DateTime.Today;
            model.Name = $"{mealPlan.Name} (Copy)";

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days();

            foreach (var meal in model.Meals)
            {
                meal.Date = model.StartDate.ToString(MealDateFormat);
            }

            return model;
        }
    }
}
