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
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;

    using static CookTheWeek.Common.EntityValidationConstants.RecipeValidation;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;

        private readonly IRecipeService recipeService;
       
        private readonly ILogger<MealService> logger;

        public MealService(IMealRepository mealRepository,
            IRecipeService recipeService,
            ILogger<MealService> logger)
        {
            this.mealRepository = mealRepository;

            this.recipeService = recipeService;         
            this.logger = logger;   
        }

        /// <inheritdoc/>
        public async Task<Meal> GetMealByIdAsync(int mealId)
        {
            Meal? meal = await mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                logger.LogError($"Meal with id {mealId} not found in the database. Error occured in method {nameof(GetMealByIdAsync)} in MealService.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            return meal;
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
        public async Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .CountAsync();
        }
        
    }
}
