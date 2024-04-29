namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Common.Extensions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using Interfaces;
    using Web.ViewModels.Admin.MealPlanAdmin;
    using Web.ViewModels.Meal;
    using Web.ViewModels.MealPlan;

    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.Recipe;
    using CookTheWeek.Common.HelperMethods;

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

        public async Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync()
        {
            return await this.dbContext.MealPlans
                .Where(mp => mp.IsFinished == false)
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

        public async Task<ICollection<MealPlanAllAdminViewModel>> AllFinishedAsync()
        {
            return await this.dbContext.MealPlans
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
        
        public async Task<ICollection<MealPlanAllViewModel>> MineAsync(string userId)
        {
            return await this.dbContext
                .MealPlans
                .Where(mp => mp.OwnerId.ToString() == userId)
                .OrderByDescending(mp => mp.StartDate)
                .Select(mp => new MealPlanAllViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name,
                    StartDate = mp.StartDate.ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    EndDate = mp.StartDate.AddDays(6.00).ToString(MealDateFormat, CultureInfo.InvariantCulture),
                    MealsCount = mp.Meals.Count,
                    IsFinished = mp.IsFinished
                }).ToListAsync();
        }

        public async Task<int> AllActiveCountAsync()
        {
            return await this.dbContext
                .MealPlans
                .Where(mp => mp.IsFinished == false)
                .CountAsync();
        }

        public async Task<MealPlanAddFormModel> GetForEditByIdAsync(string id)
        {
            MealPlanAddFormModel model = await this.dbContext.MealPlans.Where(mp => mp.Id.ToString() == id)
                .Include(mp => mp.Meals)
                .ThenInclude(mpm => mpm.Recipe)
                .ThenInclude(mpmr => mpmr.Category)
                .Select(mp => new MealPlanAddFormModel()
                {
                    Name = mp.Name,
                    Meals = mp.Meals.Select(mpm => new MealAddFormModel()
                    {
                        RecipeId = mpm.RecipeId.ToString(),
                        Title = mpm.Recipe.Title,
                        Servings = mpm.ServingSize,
                        ImageUrl = mpm.Recipe.ImageUrl,
                        CategoryName = mpm.Recipe.Category.Name,
                        Date = mpm.CookDate.ToString(MealDateFormat),
                        SelectServingOptions = ServingsOptions
                    }).ToList()
                }).FirstAsync();

            return model;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext
                .MealPlans
                .AnyAsync(mp => mp.Id.ToString() == id);
        }
    }
}
