namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static Common.GeneralApplicationConstants;
    using static Common.ExceptionMessagesConstants;

    public class MealPlanService : IMealPlanService
    {
        private readonly IMealplanRepository mealplanRepository;

        private readonly IMealService mealService;
        private readonly IMealPlanValidationService mealplanValidator;
        private readonly IUserService userService;
        private readonly ILogger<MealPlanService> logger;
        private readonly Guid userId;

        public MealPlanService(IMealService mealService,
            IMealplanRepository mealplanRepository,
            IMealPlanValidationService mealplanValidator,
            IUserService userService,
            IUserContext userContext,
            ILogger<MealPlanService> logger)
        {
            this.mealplanRepository = mealplanRepository;
            this.mealplanValidator = mealplanValidator;
            this.mealService = mealService;
            this.userService = userService;
            this.userId = userContext.UserId;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlan>> GetAllActiveAsync()
        {
            ICollection<MealPlan> activeMealPlans = await mealplanRepository.GetAllQuery()
                .Where(mp => !mp.IsFinished)
                .Include(mp => mp.Meals)
                .Include(mp => mp.Owner)
                .OrderBy(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .ToListAsync();

            return activeMealPlans;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlan>> GetAllFinishedAsync()
        {
            ICollection<MealPlan> finishedMealPlans = await mealplanRepository.GetAllQuery()
                .Where(mp => mp.IsFinished == true)
                .Include(mp => mp.Meals)
                .Include(mp => mp.Owner)
                .OrderByDescending(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .ToListAsync();

            return finishedMealPlans;
        }

        /// <inheritdoc/>
        public async Task<OperationResult<string>> TryAddMealPlanAsync(MealPlanAddFormModel model)
        {
            var result = await mealplanValidator.ValidateMealPlanFormModelAsync(model);

            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            
            MealPlan newMealPlan = new MealPlan()
            {
                Name = model.Name,
                OwnerId = userId,
                StartDate = DateTime.ParseExact(model
                                                 .Meals
                                                 .First()
                                                 .SelectDates
                                                 .First(), MealDateFormat, CultureInfo.InvariantCulture),
                Meals = model.Meals.Select(m => new Meal()
                {
                    RecipeId = m.RecipeId,
                    ServingSize = m.Servings,
                    CookDate = DateTime.ParseExact(m.Date, MealDateFormat, CultureInfo.InvariantCulture),
                }).ToList()
            };

            string id = await mealplanRepository.AddAsync(newMealPlan);

            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.MealplanUnsuccessfullyAddedExceptionMessage);
            }

            await userService.UpdateMealPlanClaimAsync(userId, true); // Update the user claims with the newly created mealplan

            // Schedule expiration 7 days from the start date using Hangfire
            BackgroundJob.Schedule<IMealPlanService>(
                service => service.ExpireMealPlanAsync(newMealPlan.Id, userId),
                newMealPlan.StartDate.AddDays(7) - DateTime.UtcNow);

            return OperationResult<string>.Success(id);

        }

        
        /// <inheritdoc/>
        public async Task<OperationResult> TryEditMealPlanAsync(MealPlanEditFormModel model)
        {
            MealPlan mealplan = await GetByIdAsync(model.Id); // RecordNotFound, UnauthorizedException

            var result = await mealplanValidator.ValidateMealPlanFormModelAsync(model);

            if (!result.IsValid)
            {
                return OperationResult.Failure(result.Errors);
            }


            await UpdateMealPlanAsync(model, mealplan);

            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlan>> GetAllMineAsync()
        {
            var userMealPlans = await mealplanRepository.GetAllQuery()
                .Include(mp => mp.Meals)
                .Where(mp => mp.OwnerId == userId)
                .OrderByDescending(mp => mp.StartDate)
                .ToListAsync();

            if (userMealPlans == null || userMealPlans.Count == 0)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoMealplansFoundExceptionMessage, null);
            }

            return userMealPlans;
        }

        /// <inheritdoc/>
        public async Task<int?> MineCountAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => mp.OwnerId == userId)
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task<int?> AllActiveCountAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => mp.IsFinished == false)
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task TryDeleteByIdAsync(Guid id)
        {
            MealPlan mealplanToDelete = await GetByIdAsync(id); // RecordNotFound, UnauthorizedUser
            await mealplanRepository.RemoveAsync(mealplanToDelete);
            await userService.UpdateMealPlanClaimAsync(userId, false);
        }
        
        /// <inheritdoc/>
        public async Task UpdateMealPlansStatusAsync(CancellationToken cancellationToken)
        {
            var mealPlans = await mealplanRepository
                .GetAllQuery()
                    .Include(mp => mp.Meals)
                .Where(mp => !mp.IsFinished)
                .ToListAsync(cancellationToken);

            // Iterate through the meal plans and update their status.
            foreach (var mealPlan in mealPlans)
            {
                // Check if the cancellation token has been requested
                cancellationToken.ThrowIfCancellationRequested();

                // Calculate expiration date (7 full days from StartDate)
                var expirationDate = mealPlan.StartDate.AddDays(7).Date;

                // Expire the meal plan if today is the 8th day (UTC)
                if (DateTime.UtcNow.Date >= expirationDate)
                {
                    mealPlan.IsFinished = true;
                }
            }

            await mealplanRepository.SaveAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ExpireMealPlanAsync(Guid id, Guid userId)
        {
            var mealplan = await mealplanRepository
                .GetByIdQuery(id)
                .FirstOrDefaultAsync();

            // Ensure the meal plan exists and has reached its expiration date
            if (mealplan != null && DateTime.UtcNow.Date >= mealplan.StartDate.AddDays(7).Date)
            {
                mealplan.IsFinished = true;
                await mealplanRepository.SaveAsync(CancellationToken.None);

                // Update the user's meal plan claim to reflect the expired status
                await userService.UpdateMealPlanClaimAsync(userId, false);
            }
        }

        /// <inheritdoc/>
        public async Task<MealPlan> GetActiveAsync()
        {
            var id = await GetActiveIdAsync();

            if (id == default)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            MealPlan mealplan = await GetByIdAsync(id);
            return mealplan;
        }

        /// <inheritdoc/>
        public async Task<Guid> GetActiveIdAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => mp.OwnerId == userId && !mp.IsFinished)
                .Select(mp => mp.Id)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<MealPlan> GetByIdAsync(Guid id)
        {
            MealPlan? mealplan = await mealplanRepository.GetByIdQuery(id)
                .Include(mp => mp.Owner)
                .Include(mp => mp.Meals)
                    .ThenInclude(m => m.Recipe)
                        .ThenInclude(r => r.RecipesIngredients)
                            .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Meals)
                    .ThenInclude(m => m.Recipe)
                        .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync();

            if (mealplan == null)
            {
                logger.LogError($"Meal plan with id {id} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            mealplanValidator.ValidateUserIsMealPlanOwner(mealplan.OwnerId);

            return mealplan;
        }

        /// <inheritdoc/>
        public async Task<MealPlan> GetUnfilteredByIdAsync(Guid id)
        {
            MealPlan? mealplan = await mealplanRepository.GetByIdQuery(id)
                .IgnoreQueryFilters()  // Apply to the entire query, ignoring all global filters
                .Include(mp => mp.Meals)
                    .ThenInclude(m => m.Recipe)
                        .ThenInclude(r => r.RecipesIngredients)
                            .ThenInclude(ri => ri.Ingredient)
                .Include(mp => mp.Meals)
                    .ThenInclude(m => m.Recipe)
                        .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync();

            if (mealplan == null)
            {
                logger.LogError($"Meal plan with id {id} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            mealplanValidator.ValidateUserIsMealPlanOwner(mealplan.OwnerId);

            return mealplan;
        }

        // HELPER METHODS:

        /// <summary>
        /// Utility method to update the mealplan according to the edit model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mealplan"></param>
        /// <returns></returns>
        private async Task UpdateMealPlanAsync(MealPlanEditFormModel model, MealPlan mealplan)
        {
            mealplan.Name = model.Name;
            mealplan.Meals = mealService.CreateOrUdateMealsAsync(model.Meals);

            await mealplanRepository.SaveAsync(CancellationToken.None);
        }

        internal static async Task<ICollection<Guid>> GetAcitveRecipeIdsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
