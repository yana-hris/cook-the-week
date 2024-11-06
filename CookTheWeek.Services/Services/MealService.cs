namespace CookTheWeek.Services.Data.Services
{
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Common.Exceptions;
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
        public ICollection<Meal> CreateMealsAsync(ICollection<MealFormModel> model)
        {
            ICollection<Meal> meals = new List<Meal>();

            foreach (var modelMeal in model)
            {
                Meal meal = new Meal()
                {
                    RecipeId = modelMeal.RecipeId,
                    ServingSize = modelMeal.Servings,
                    CookDate = DateTime.ParseExact(modelMeal.Date, MealDateFormat, CultureInfo.InvariantCulture),
                };

                meals.Add(meal);
            }

            return meals;
        }


        /// <inheritdoc/>
        public async Task<Meal> GetByIdAsync(int id)
        {
            Meal? meal = await mealRepository.GetByIdAsync(id);

            if (meal == null)
            {
                logger.LogError($"Meal with id {id} not found in the database. Error occured in method {nameof(GetByIdAsync)} in MealService.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            return meal;
        }


        /// <inheritdoc/>
        public async Task<int?> GetAllMealsCountByRecipeIdAsync(Guid recipeId)
        {
            return await mealRepository.GetAllQuery()
                .Where(m => m.RecipeId == recipeId)
                .CountAsync();
        }

        /// <inheritdoc/>
        public async Task TryMarkAsCooked(int id)
        {
            Meal? meal = await mealRepository.GetByIdAsync(id);

            if (meal == null)
            {
                logger.LogError($"Marking meal as cooked failed. Meal with id {id} was not found in the database.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            if (meal.IsCooked)
            {
                logger.LogError($"Meal meal as cooked failed. Meal with id {id} is already cooked.");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidMealCookExceptionMessage);
            }

            meal.IsCooked = true;
            await mealRepository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task TryMarkUncooked(int id)
        {
            Meal? meal = await mealRepository.GetByIdAsync(id);

            if (meal == null)
            {
                logger.LogError($"Marking meal uncooked failed. Meal with id {id} was not found in the database.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealNotFoundExceptionMessage, null);
            }

            if (!meal.IsCooked)
            {
                logger.LogError($"Marking meal uncooked failed. Meal with id {id} is not cooked yet.");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.InvalidMealCookExceptionMessage);
            }

            meal.IsCooked= false;
            await mealRepository.SaveChangesAsync();
        }


        /// <inheritdoc/>
        public async Task SoftDeleteAllByRecipeIdAsync(Guid recipeId)
        {
            ICollection<Meal> mealsToDelete = await GetAllByRecipeIdAsync(recipeId);

            if (mealsToDelete.Any())
            {
                foreach (var meal in mealsToDelete)
                {
                    meal.RecipeId = DeletedRecipeId;
                }

                await mealRepository.UpdateRangeAsync(mealsToDelete);
            }
        }
        
        // HELPER METHODS:

        /// <summary>
        /// Helper method that gets all meals made with a given recipe ID.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns>A collection of Meals</returns>
        private async Task<ICollection<Meal>> GetAllByRecipeIdAsync(Guid recipeId)
        {
            return await mealRepository
                .GetAllQuery()
                .Where(m => m.RecipeId == recipeId)
                .ToListAsync();
        }

       
    }
}
