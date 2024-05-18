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
    using Microsoft.Extensions.Logging;

    public class MealPlanService : IMealPlanService
    {
        private readonly CookTheWeekDbContext dbContext;

        public MealPlanService(CookTheWeekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync()
        {
            return await this.dbContext.MealPlans
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
        public async Task<string> AddAsync(string userId, MealPlanAddFormModel model)
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

            return newMealPlan.Id.ToString();
           
        }
        public async Task EditAsync(string userId, MealPlanAddFormModel model)
        {
            MealPlan mealPlanToEdit = await this.dbContext
                .MealPlans
                .Include(mp => mp.Meals)
                .FirstAsync(mp => mp.Id.ToString() == model.Id);

            mealPlanToEdit.Name = model.Name;

            // Add or update existing Meals
            foreach (var modelMeal in model.Meals)
            {
                if (mealPlanToEdit.Meals.Any(m => m.RecipeId.ToString() == modelMeal.RecipeId.ToLower()))
                {
                    Meal currentMeal = await this.dbContext
                        .Meals
                        .FirstAsync(m => m.MealPlanId.ToString() == model.Id && m.RecipeId.ToString() == modelMeal.RecipeId);

                    currentMeal.ServingSize = modelMeal.Servings;
                    currentMeal.CookDate = DateTime.ParseExact(modelMeal.Date, MealDateFormat, CultureInfo.InvariantCulture);
                }
                else
                {
                    Meal newMeal = new Meal()
                    {
                        RecipeId = Guid.Parse(modelMeal.RecipeId),
                        ServingSize = modelMeal.Servings,
                        CookDate = DateTime.ParseExact(modelMeal.Date, MealDateFormat, CultureInfo.InvariantCulture),
                    };
                    mealPlanToEdit.Meals.Add(newMeal);
                }                
            }

            // Delete removed Meals from Meal Plan Database
            foreach (var existingMeal in mealPlanToEdit.Meals)
            {
                string databaseMealRecipeId = existingMeal.RecipeId.ToString();
                bool remains = model.Meals.Any(m => m.RecipeId.ToLower() == databaseMealRecipeId);

                if (!remains)
                {
                    dbContext.Meals.Remove(existingMeal);
                }
            }

            await this.dbContext.SaveChangesAsync();
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
        public async Task<MealPlanViewModel> GetByIdAsync(string id)
        {
            // without IngredientsCount
            MealPlanViewModel model = await this.dbContext
                .MealPlans                
                .Where(mp => mp.Id.ToString() == id)
                .Include(mp => mp.Meals)
                    .ThenInclude(mpm => mpm.Recipe)                        
                        .ThenInclude(recipe => recipe.Category)
                .Select(mp => new MealPlanViewModel()
                {
                    Id = mp.Id.ToString(),
                    Name = mp.Name,
                    IsFinished = mp.IsFinished,
                    Meals = mp.Meals.Select(mpm => new MealViewModel()
                    {
                        Id = mpm.Id.ToString(),
                        RecipeId = mpm.RecipeId.ToString(),
                        Title = mpm.Recipe.Title,
                        Servings = mpm.ServingSize,
                        ImageUrl = mpm.Recipe.ImageUrl,
                        CategoryName = mpm.Recipe.Category.Name,
                        Date = mpm.CookDate.ToString(MealDateFormat),
                    }).ToList(),
                    TotalServings = mp.Meals.Sum(mpm => mpm.ServingSize),                    
                    TotalCookingDays = mp.Meals.Select(mpm => mpm.CookDate.Date).Distinct().Count(),

                }).FirstAsync();

            return model;
        }
        public async Task<MealPlanAddFormModel> GetForEditByIdAsync(string id)
        {
            MealPlanAddFormModel model = await this.dbContext
                .MealPlans
                .Where(mp => mp.Id.ToString() == id)
                .Include(mp => mp.Meals)
                .ThenInclude(mpm => mpm.Recipe)
                .ThenInclude(mpmr => mpmr.Category)
                .Select(mp => new MealPlanAddFormModel()
                {
                    Name = mp.Name,
                    StartDate = mp.StartDate,
                    Meals = mp.Meals.Select(mpm => new MealAddFormModel()
                    {
                        RecipeId = mpm.RecipeId.ToString().ToLower(),
                        Title = mpm.Recipe.Title,
                        Servings = mpm.ServingSize,
                        ImageUrl = mpm.Recipe.ImageUrl,
                        CategoryName = mpm.Recipe.Category.Name,
                        Date = mpm.CookDate.ToString(MealDateFormat),
                        SelectServingOptions = ServingsOptions
                    }).ToList(),
                }).FirstAsync();

            return model;
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.dbContext
                .MealPlans
                .AnyAsync(mp => mp.Id.ToString() == id);
        }
        public async Task<int> AllActiveCountAsync()
        {
            return await this.dbContext
                .MealPlans
                .Where(mp => mp.IsFinished == false)
                .CountAsync();
        }
        public async Task<int> GetIMealPlanIngredientsCountForDetailsAsync(string id)
        {
            var mealPlan = await this.dbContext
                .MealPlans
                .Where(mp => mp.Id.ToString() == id)
                    .Include(mp => mp.Meals)
                        .ThenInclude(m => m.Recipe)  
                            .ThenInclude(r => r.RecipesIngredients)
                .FirstAsync();

            return mealPlan.Meals.Sum(m => m.Recipe.RecipesIngredients.Count());
        }
        public async Task<int> GetMealPlanTotalMinutesForDetailsAsync(string id)
        {
            var mealPlan = await this.dbContext
                .MealPlans
                .Where(mp => mp.Id.ToString() == id)
                .Include(mp => mp.Meals)
                .ThenInclude(m => m.Recipe)
                .FirstAsync();

            return mealPlan.Meals.Sum(m => (int)m.Recipe.TotalTime.TotalMinutes);
        }

       

    }
}
