﻿namespace CookTheWeek.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Common.HelperMethods;
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
        private readonly IValidationService validationService;
        private readonly ILogger<MealPlanService> logger;
        private readonly string? userId;

        public MealPlanService(IMealService mealService,
            IMealplanRepository mealplanRepository,
            IValidationService validationService,
            IUserContext userContext,
            ILogger<MealPlanService> logger)
        {
            this.mealplanRepository = mealplanRepository;
            this.validationService = validationService;
            this.mealService = mealService;
            this.userId = userContext.UserId ?? String.Empty;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ICollection<MealPlan>> GetAllActiveAsync()
        {
            ICollection<MealPlan> activeMealPlans = await mealplanRepository.GetAllQuery()
                .Where(mp => !mp.IsFinished)
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
                .OrderByDescending(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .ToListAsync();

            return finishedMealPlans;
        }

        /// <inheritdoc/>
        public async Task<OperationResult<string>> TryAddMealPlanAsync(MealPlanAddFormModel model)
        {
            var result = await validationService.ValidateMealPlanMealsAsync(model);

            if (!result.IsValid)
            {
                return OperationResult<string>.Failure(result.Errors);
            }

            
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
            MealPlan mealplan = await TryGetAsync(model.Id); // RecordNotFound, UnauthorizedException

            var result = await validationService.ValidateMealPlanMealsAsync(model);

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
                .Where(mp => GuidHelper.CompareGuidStringWithGuid(userId!, mp.OwnerId))
                .OrderByDescending(mp => mp.StartDate)
                .ToListAsync();

            if (userMealPlans == null || userMealPlans.Any())
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.NoMealplansFoundExceptionMessage, null);
            }

            return userMealPlans;
        }

        /// <inheritdoc/>
        public async Task<int?> MineCountAsync()
        {
            return await mealplanRepository.GetAllQuery()
                .Where(mp => GuidHelper.CompareGuidStringWithGuid(userId, mp.OwnerId))
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
        public async Task TryDeleteByIdAsync(string id)
        {
            MealPlan mealplanToDelete = await TryGetAsync(id); // RecordNotFound, UnauthorizedUser
            await mealplanRepository.RemoveAsync(mealplanToDelete);
        }

        /// <inheritdoc/>
        public async Task<MealPlan> TryGetAsync(string id)
        {
            MealPlan mealplan = await GetByIdAsync(id);

            validationService.ValidateUserIsResourceOwnerAsync(mealplan.OwnerId);

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
            await mealplanRepository.UpdateAsync(mealplan);

            await mealService.HardDeleteAllByMealPlanIdAsync(model.Id);
            await mealService.AddAllAsync(model.Meals);
        }


        /// <summary>
        /// Gets a single Meal Plan or throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        private async Task<MealPlan> GetByIdAsync(string id)
        {
            MealPlan? mealplan = await mealplanRepository.GetByIdAsync(id);

            if (mealplan == null)
            {
                logger.LogError($"Meal plan with id {id} not found.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            return mealplan;
        }
    }
}
