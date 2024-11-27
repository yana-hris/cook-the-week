namespace CookTheWeek.Services.Data.Factories
{
    using System.Globalization;

    using Microsoft.Extensions.Logging;

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
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.HelperMethods.CookingTimeHelper;

    public class MealPlanViewModelFactory : IMealPlanViewModelFactory
    {
        private readonly IMealPlanService mealplanService;
        private readonly IMealViewModelFactory mealFactory;
        private readonly ILogger<RecipeViewModelFactory> logger;

        public MealPlanViewModelFactory(ILogger<RecipeViewModelFactory> logger,
                                        IMealViewModelFactory mealFactory,
                                      IMealPlanService mealplanService)
        {
            this.logger = logger;
            this.mealFactory = mealFactory;
            this.mealplanService = mealplanService;
        }

        /// <inheritdoc/>
        public async Task<MealPlanAddFormModel> CreateMealPlanAddFormModelAsync(MealPlanServiceModel serviceModel)
        {
            MealPlanAddFormModel model = new MealPlanAddFormModel();

            // Ensure all fields are initiated and filled with correct data
            if (model.StartDate == default)
            {
                model.StartDate = DateTime.UtcNow.Date;
            }

            if (model.Meals == null)
            {
                model.Meals = new List<MealFormModel>();
            }

            foreach (var meal in serviceModel.Meals)
            {
                MealFormModel currentMeal = await mealFactory.CreateMealAddFormModelAsync(meal);
                model.Meals.Add(currentMeal);
            }

            model = AddMealCookSelectDates(model);

            return model;
        }

        /// <inheritdoc/>
        public T AddMealCookSelectDates<T>(T model) where T : IMealPlanFormModel
        {
            if (model.Meals.Any())
            {
                model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);
            }

            return model;
        }

        /// <inheritdoc/>
        public async Task<MealPlanDetailsViewModel> CreateMealPlanDetailsViewModelAsync(Guid mealplanId)
        {
            MealPlan mealPlan = await mealplanService.GetUnfilteredByIdAsync(mealplanId);

            MealPlanDetailsViewModel model = new MealPlanDetailsViewModel()
            {
                Id = mealPlan.Id.ToString(),
                Name = mealPlan.Name,
                OwnerId = mealPlan.OwnerId,
                StartDate = mealPlan.StartDate.ToString(MealPlanDateFormat),
                EndDate = mealPlan.StartDate.AddDays(6).ToString(MealPlanDateFormat),
                IsFinished = mealPlan.IsFinished,
                Meals = mealPlan.Meals
                    .OrderBy(mpm => mpm.CookDate)
                    .Select(mpm => new MealViewModel()
                    {
                        Id = mpm.Id.ToString(),
                        RecipeId = mpm.RecipeId.ToString(),
                        Title = mpm.Recipe.Title,
                        Servings = mpm.ServingSize,
                        CookingTime = FormatCookingTime(mpm.Recipe.TotalTime),
                        ImageUrl = mpm.Recipe.ImageUrl,
                        CategoryName = mpm.Recipe.Category.Name,
                        IsCooked = mpm.IsCooked,
                        DayOfTheWeek = mpm.CookDate.ToString("ddd"),
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
        public async Task<ICollection<MealPlanAllViewModel>> CreateMealPlansMineViewModelAsync()
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
        public async Task<TFormModel> CreateMealPlanFormModelAsync<TFormModel>(Guid id, ICollection<MealServiceModel>? newMeals = null)
            where TFormModel : IMealPlanFormModel, new()
        {
            MealPlan mealplan = await mealplanService.GetUnfilteredByIdAsync(id);

            if (mealplan.Meals == null || mealplan.Meals.Count == 0)
            {
                throw new ArgumentNullException(ArgumentNullExceptionMessages.MealsArrayNullExceptionMessage);
            }

            if (typeof(TFormModel) == typeof(MealPlanAddFormModel))
            {
                return (TFormModel)(object)MapMealPlanToAddModel(mealplan);
            }
            else if (typeof(TFormModel) == typeof(MealPlanEditFormModel))
            {
                if (newMeals != null && newMeals.Count > 0)
                {
                    ICollection<MealFormModel> newMealsModel = new List<MealFormModel>();

                    foreach (var newMeal in newMeals)
                    {
                        MealFormModel currentModel = await mealFactory.CreateMealAddFormModelAsync(newMeal);
                        newMealsModel.Add(currentModel);
                    }      

                    return (TFormModel)(object)MapMealPlanToEditModel(mealplan, newMealsModel);
                }

                return (TFormModel)(object)MapMealPlanToEditModel(mealplan);
            }

            throw new InvalidOperationException("Unsupported form model type.");
        }

        public async Task<MealPlanActiveModalViewModel> GetActiveDetails()
        {
            MealPlan mealplan = await mealplanService.GetActiveAsync(); // RecordNotFoundException

            var model = new MealPlanActiveModalViewModel
            {
                Id = mealplan.Id.ToString(),
                Name = mealplan.Name,
                StartDate = mealplan.StartDate.ToString(MealPlanDateFormat, CultureInfo.InvariantCulture),
                EndDate = mealplan.StartDate.AddDays(6.00).ToString(MealPlanDateFormat, CultureInfo.InvariantCulture),
                TotalServings = mealplan.Meals.Sum(mpm => mpm.ServingSize),
                TotalMealsCount = mealplan.Meals.Count,
                TotalMealsCooked = mealplan.Meals.Where(m => m.IsCooked).Count(),
                TotalMealsUncooked = mealplan.Meals.Where(m => !m.IsCooked).Count(),
                TotalIngredients = mealplan.Meals.Sum(m => m.Recipe.RecipesIngredients.Count),
                TotalCookingTimeMinutes = mealplan.Meals.Sum(m => (int)m.Recipe.TotalTime.TotalMinutes),
                DaysRemaining = (mealplan.StartDate.AddDays(7.00) -  DateTime.UtcNow.Date).Days,
            };

            return model;
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
        private static T MapMealPlanToFormModel<T>(MealPlan mealplan, 
            ICollection<MealFormModel>? addedMeals = null) where T : IMealPlanFormModel, new()
        {
            var model = new T()
            {
                Name = mealplan.Name,
                StartDate = mealplan.StartDate,
            };

            if (model is MealPlanEditFormModel editModel)
            {
                editModel.Meals = mealplan.Meals.Select(mpm => new MealFormModel()
                {
                    Id = mpm.Id,
                    RecipeId = mpm.RecipeId,
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    IsCooked = mpm.IsCooked,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                    SelectServingOptions = ServingsOptions
                }).ToList();

                if (addedMeals != null && addedMeals.Count > 0)
                {
                    editModel.Meals = editModel.Meals.Concat(addedMeals).ToList();
                }
                
                editModel.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);
                return (T)(IMealPlanFormModel)editModel;
            }
            else
            {
                model.Meals = mealplan.Meals.Select(mpm => new MealFormModel()
                {
                    RecipeId = mpm.RecipeId,
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                    SelectServingOptions = ServingsOptions
                }).ToList();
            }

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);
            return model;
        }

        /// <summary>
        /// Utility method for mapping the Edit Form model (for edit case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanEditFormModel MapMealPlanToEditModel(MealPlan mealPlan, ICollection<MealFormModel>? newMeals = null)
        {
            var model = MapMealPlanToFormModel<MealPlanEditFormModel>(mealPlan, newMeals);
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
