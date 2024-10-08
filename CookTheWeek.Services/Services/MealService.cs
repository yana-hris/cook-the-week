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
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    
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
        public async Task AddAllAsync(ICollection<MealAddFormModel> meals)
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

            await mealRepository.AddRangeAsync(newMeals);
        }


        /// <inheritdoc/>
        public async Task<Meal> GetByIdAsync(int mealId)
        {
            Meal? meal = await mealRepository.GetByIdAsync(mealId);

            if (meal == null)
            {
                logger.LogError($"Meal with id {mealId} not found in the database. Error occured in method {nameof(GetByIdAsync)} in MealService.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            return meal;
        }


        /// <inheritdoc/>
        public async Task<int?> GetAllMealsCountByRecipeIdAsync(string recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .CountAsync();
        }


        /// <inheritdoc/>
        public async Task HardDeleteAllByMealPlanIdAsync(string mealplanId)
        {
            var mealsToDelete = await mealRepository.GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(mealplanId, m.MealPlanId))
                .ToListAsync();

            await mealRepository.RemoveRangeAsync(mealsToDelete);

        }
       

        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<Meal> mealsToDelete = await GetAllByRecipeIdAsync(recipeId);

            if (mealsToDelete.Any())
            {
                foreach (var meal in mealsToDelete)
                {
                    meal.IsDeleted = true;
                }

                await mealRepository.UpdateRangeAsync(mealsToDelete);
            }
        }


        /// <inheritdoc/> // TODO: Check if will be used at all..
        public async Task HardDeleteAllByRecipeIdAsync(string recipeId)
        {
            ICollection<Meal> mealsToDelete = await GetAllByRecipeIdAsync(recipeId);

            if (mealsToDelete.Any())
            {
                await mealRepository.RemoveRangeAsync(mealsToDelete);
            }
        }


        // HELPER METHODS:

        /// <summary>
        /// Helper method that gets all meals made with a given recipe ID.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A collection of Meals</returns>
        private async Task<ICollection<Meal>> GetAllByRecipeIdAsync(string recipeId)
        {
            return await mealRepository
                .GetAllQuery()
                .Where(m => GuidHelper.CompareGuidStringWithGuid(recipeId, m.RecipeId))
                .ToListAsync();
        }
    }
}
