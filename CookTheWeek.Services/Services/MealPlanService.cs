namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.Extensions;
    using CookTheWeek.Common.HelperMethods;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.MealPlanAdmin;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.RecipeValidation;
    using static Common.ExceptionMessagesConstants;

    public class MealPlanService : IMealPlanService
    {
        private readonly IMealplanRepository mealplanRepository;

        private readonly IMealService mealService;
        private readonly IValidationService validationService;
        private readonly IUserService userService;
        private readonly ILogger<MealPlanService> logger;

        public MealPlanService(IMealService mealService,
            IMealplanRepository mealplanRepository,
            IValidationService validationService,
            IUserService userService,
            ILogger<MealPlanService> logger)
        {
            this.mealplanRepository = mealplanRepository;
            this.validationService = validationService;
            this.mealService = mealService;
            this.userService = userService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => !mp.IsFinished)
                .OrderBy(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .Select(mp => new MealPlanAllAdminViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => mp.IsFinished == true)
                .OrderByDescending(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .Select(mp => new MealPlanAllAdminViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<OperationResult<string>> TryAddMealPlanAsync(MealPlanAddFormModel model)
        {
            var result = await validationService.ValidateMealPlanFormModelAsync(model);

            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            string userId = userService.GetCurrentUserId()!;

            MealPlan newMealPlan = new MealPlan()
            {
                Name = model.Name,
                OwnerId = Guid.Parse(userId),
                StartDate = DateTime.ParseExact(model
                                                 .Meals
                                                 .First()
                                                 .SelectDates
                                                 .First(), MealDateFormat, CultureInfo.InvariantCulture),
                Meals = model.Meals.Select(m => new Meal()
                {
                    RecipeId = Guid.Parse(m.RecipeId),
                    ServingSize = m.Servings,
                    CookDate = DateTime.ParseExact(m.Date, MealDateFormat, CultureInfo.InvariantCulture),
                }).ToList()
            };

            string id = await mealplanRepository.AddAsync(newMealPlan);

            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.MealplanUnsuccessfullyAddedExceptionMessage);
            }

            return OperationResult<string>.Success(newMealPlan.Id.ToString());

        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryEditMealPlanAsync(MealPlanEditFormModel model)
        {
            var result = await validationService.ValidateMealPlanFormModelAsync(model);

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }

            MealPlan mealplan = await GetByIdAsync(model.Id);

            await UpdateMealPlanAsync(model, mealplan);

            return OperationResult.Success();
        }       

        /// <inheritdoc/>
        public async Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId)
        {
            var userMealPlans = await mealplanRepository.GetAllQuery()
                .Where(mp => mp.OwnerId.ToString() == userId)
                .OrderByDescending(mp => mp.StartDate)
                .ToListAsync();

            if (userMealPlans == null || userMealPlans.Any())
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoMealplansFoundExceptionMessage, null);
            }

            var model = userMealPlans
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
        public async Task<int?> MineCountAsync(string userId)
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => GuidHelper.CompareGuidStringWithGuid(userId, mp.OwnerId))
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id)
        {
            MealPlan mealPlan = await GetByIdAsync(id);

            MealPlanDetailsViewModel model = new MealPlanDetailsViewModel()
            {
                Id = mealPlan.Id.ToString(),
                Name = mealPlan.Name,
                OwnerId = mealPlan.OwnerId.ToString(),
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
        public async Task<MealPlanEditFormModel> GetForEditByIdAsync(string id)
        {
            MealPlan mealplan = await GetByIdAsync(id);

            validationService.ValidateMealPlanUserAuthorizationAsync(mealplan.OwnerId);

            MealPlanEditFormModel model = MapMealPlanToEditModel(mealplan);

            return model;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await mealplanRepository.GetByIdAsync(id) != null;
        }

        /// <inheritdoc/>
        public async Task<MealPlan> GetByIdAsync(string id)
        {
            MealPlan? mealplan = await mealplanRepository.GetByIdAsync(id);

            if (mealplan == null)
            {
                logger.LogError($"Meal plan with id {id} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            return mealplan;
        }

        /// <inheritdoc/>
        public async Task<int?> AllActiveCountAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => mp.IsFinished == false)
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task TryDeleteByIdAsync(string id)
        {
            MealPlan mealplanToDelete = await GetByIdAsync(id); //RecordNotFoundException
            validationService.ValidateMealPlanUserAuthorizationAsync(mealplanToDelete.OwnerId);

            await DeleteByIdAsync(mealplanToDelete);
        }

        
        // TODO: Check if needed as configuration for User is OnDelete.Cascade
        /// <inheritdoc/>
        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var userMealplans = await mealplanRepository
                .GetAllQuery()
                .Where(mp => GuidHelper.CompareGuidStringWithGuid(userId, mp.OwnerId))
                .ToListAsync();

            if (userMealplans.Any())
            {
                foreach (var mealpan in userMealplans)
                {
                    await mealService.DeleteAllByMealPlanIdAsync(mealpan.Id.ToString());
                }
                await mealplanRepository.DeleteAllAsync(userMealplans);
            }

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
                MealAddFormModel currentMeal = await mealService.CreateMealAddFormModelAsync(meal);
                model.Meals.Add(currentMeal);
            }

            return model;
        }

        /// <inheritdoc/>
        public async Task<MealPlanAddFormModel> TryCopyMealPlanByIdAsync(string mealPlanId)
        {
            MealPlan originalMealPlan = await GetByIdAsync(mealPlanId); // RecordNotFoundException
            validationService.ValidateMealPlanUserAuthorizationAsync(originalMealPlan.OwnerId); // UnauthorizedUserException

            MealPlanAddFormModel model = MapMealPlanToAddModel(originalMealPlan);
            var result = await validationService.ValidateMealPlanFormModelAsync(model);

            if (!result.IsValid)
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.MealplanUnsuccessfullyCopiedExceptionMessage);
            }

            return model;
        }

        // HELPER METHODS:

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
                    RecipeId = mpm.RecipeId.ToString(),
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

        /// <summary>
        /// Utility method for mapping the Edit Form model (for edit case)
        /// </summary>
        /// <param name="mealPlan"></param>
        /// <returns></returns>
        private static MealPlanEditFormModel MapMealPlanToEditModel(MealPlan mealPlan)
        {
            var model = MapMealPlanToFormModel<MealPlanEditFormModel>(mealPlan);
            model.Id = mealPlan.Id.ToString();
            return model;
        }

        /// <summary>
        /// Utility method to update the mealplan according to the edit model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mealplan"></param>
        /// <returns></returns>
        private async Task UpdateMealPlanAsync(MealPlanEditFormModel model, MealPlan mealplan)
        {
            mealplan.Name = model.Name;
            await mealService.DeleteAllByMealPlanIdAsync(model.Id);
            await mealService.AddAllAsync(model.Meals);
            await mealplanRepository.UpdateAsync(mealplan);
        }

        /// /// <summary>
        /// Utility metho that deletes a given meal plan, deleting also all its nested meals. If a meal plan does not exists, throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task DeleteByIdAsync(MealPlan mealplan)
        {
            await mealService.DeleteAllByMealPlanIdAsync(mealplan.Id.ToString()); // TODO: Check if might be useless as Configuration is OnDelete.Cascade
            await mealplanRepository.DeleteByIdAsync(mealplan);

        }
    }
}
