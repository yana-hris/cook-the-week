namespace CookTheWeek.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using Interfaces;
    using Web.ViewModels.MealPlan;
    using CookTheWeek.Data.Models;

    using static Common.GeneralApplicationConstants;
    using System.Globalization;

    public class MealplanService : IMealplanService
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealplanService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(string userId, MealPlanAddFormModel model)
        {
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

            await this.dbContext.MealPlans.AddAsync(newMealPlan);
            await this.dbContext.SaveChangesAsync();
           
        }

        public async Task<int> AllActiveCountAsync()
        {
            return await this.dbContext
                .MealPlans
                .Where(m => m.IsFinished == false)
                .CountAsync();
        }
    }
}
