namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.MealPlan;
    using CookTheWeek.Common.Extensions;

    using static Common.GeneralApplicationConstants;

    public class MealPlanService : IMealPlanService
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealPlanService(CookTheWeekDbContext dbContext)
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

        public async Task<ICollection<MealPlanAllViewModel>> AllActiveAsync()
        {
            return await this.dbContext.MealPlans
                .Where(mp => mp.IsFinished == false)
                .OrderBy(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .Select(mp => new MealPlanAllViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToListAsync();
        }

        public async Task<ICollection<MealPlanAllViewModel>> AllFinishedAsync()
        {
            return await this.dbContext.MealPlans
                .Where(mp => mp.IsFinished == true)
                .OrderByDescending(mp => mp.StartDate)
                .ThenBy(mp => mp.Name)
                .Select(mp => new MealPlanAllViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name.TrimToChar(30),
                    OwnerUsername = mp.Owner.UserName!,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count
                }).ToListAsync();
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
