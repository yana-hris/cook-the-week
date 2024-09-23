namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Common.Extensions;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using Interfaces;
    using Web.ViewModels.Admin.MealPlanAdmin;
    using Web.ViewModels.Meal;
    using Web.ViewModels.MealPlan;

    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.Recipe;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using CookTheWeek.Common;

    public class MealPlanService : IMealPlanService
    {
        private readonly CookTheWeekDbContext dbContext;
        private readonly IMealplanRepository mealplanRepository;

        public MealPlanService(CookTheWeekDbContext dbContext,
            IMealplanRepository mealplanRepository)
        {
            this.dbContext = dbContext;
            this.mealplanRepository = mealplanRepository;
        }

        public async Task<ICollection<MealPlanAllAdminViewModel>> AllActiveAsync()
        {
            return await this.mealplanRepository.GetAllQuery()
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
            return await this.mealplanRepository.GetAllQuery()
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

        // TODO: apply repository usage
        public async Task<string> AddAsync(string userId, MealPlanAddFormModel model)
        {
            MealPlan newMealPlan = new MealPlan()
            {
                Name = model.Name!,
                OwnerId = Guid.Parse(userId),
                StartDate = DateTime.ParseExact(model
                                                 .Meals
                                                 .First()
                                                 .SelectDates
                                                 .First(), MealDateFormat, CultureInfo.InvariantCulture),
                Meals = model.Meals.Select(m => new Meal()
                {
                    RecipeId = Guid.Parse(m.RecipeId),
                    ServingSize = m.Servings!.Value,
                    CookDate = DateTime.ParseExact(m.Date, MealDateFormat, CultureInfo.InvariantCulture),
                }).ToList()
            };

            string id = await this.mealplanRepository.AddAsync(newMealPlan);

            if (String.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.MealplanUnsuccessfullyAddedExceptionMessage);
            }

            return newMealPlan.Id.ToString();
           
        }
        // TODO: use repository
        public async Task EditAsync(string userId, MealPlanAddFormModel model)
        {
            MealPlan mealPlanToEdit = await this.dbContext
                .MealPlans
                .Include(mp => mp.Meals)
                .FirstAsync(mp => mp.Id.ToString() == model.Id);

            mealPlanToEdit.Name = model.Name!;

            // Add or update existing Meals
            foreach (var modelMeal in model.Meals)
            {
                if (mealPlanToEdit.Meals.Any(m => m.RecipeId.ToString() == modelMeal.RecipeId.ToLower()))
                {
                    Meal currentMeal = await this.dbContext
                        .Meals
                        .FirstAsync(m => m.MealPlanId.ToString() == model.Id && m.RecipeId.ToString() == modelMeal.RecipeId);

                    currentMeal.ServingSize = modelMeal.Servings!.Value;
                    currentMeal.CookDate = DateTime.ParseExact(modelMeal.Date, MealDateFormat, CultureInfo.InvariantCulture);
                }
                else
                {
                    Meal newMeal = new Meal()
                    {
                        RecipeId = Guid.Parse(modelMeal.RecipeId),
                        ServingSize = modelMeal.Servings!.Value,
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
            return await this.mealplanRepository.GetAllQuery()
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

        public async Task<int> MineCountAsync(string userId)
        {
            return await this.mealplanRepository.GetAllQuery()
                .Where(mp => mp.OwnerId.ToString().ToLower() == userId.ToLower())
                .CountAsync();
        }

        // TODO: delegate model creation to factory and here only return the mealplan
        // TODO: handle RecordNotFoundException in the controller
        public async Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id, string userId)
        {
            MealPlan? mealPlan = await this.mealplanRepository.GetByIdAsync(id);

            if (mealPlan == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            MealPlanDetailsViewModel model = new MealPlanDetailsViewModel()
                {
                    Id = mealPlan.Id.ToString(),
                    Name = mealPlan.Name,
                    OwnerId = userId,
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

        // TODO: delegate model creation to factory and here only return the mealplan
        // TODO: handle RecordNotFound Exception in the controller
        public async Task<MealPlanAddFormModel> GetForEditByIdAsync(string id)
        {
            MealPlan? mealplan = await this.mealplanRepository.GetByIdAsync(id);

            if (mealplan == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            MealPlanAddFormModel model = new MealPlanAddFormModel()
            {
                Name = mealplan.Name,
                StartDate = mealplan.StartDate,
                Meals = mealplan.Meals.Select(mpm => new MealAddFormModel()
                {
                    RecipeId = mpm.RecipeId.ToString().ToLower(),
                    Title = mpm.Recipe.Title,
                    Servings = mpm.ServingSize,
                    ImageUrl = mpm.Recipe.ImageUrl,
                    CategoryName = mpm.Recipe.Category.Name,
                    Date = mpm.CookDate.ToString(MealDateFormat),
                    SelectServingOptions = ServingsOptions
                }).ToList(),
            };

            return model;
        }
        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await this.mealplanRepository.GetByIdAsync(id) != null;
        }
        public async Task<int> AllActiveCountAsync()
        {
            return await this.mealplanRepository.GetAllQuery()
                .Where(mp => mp.IsFinished == false)
                .CountAsync();
        }

        //// TODO: catch RecordNotFound exception in all controllers, using it
        //public async Task<int> GetIMealPlanIngredientsCountForDetailsAsync(string id)
        //{
        //    var mealPlan = await this.mealplanRepository.GetByIdAsync(id);

        //    if (mealPlan == null)
        //    {
        //        throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
        //    }
        //    return mealPlan.Meals.Sum(m => m.Recipe.RecipesIngredients.Count);
        //}

        //// TODO: catch RecordNotFound exception in all controllers, using it
        //public async Task<int> GetMealPlanTotalMinutesForDetailsAsync(string id)
        //{
        //    MealPlan? mealPlan = await this.mealplanRepository.GetByIdAsync(id);

        //    if (mealPlan == null)
        //    {
        //        throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
        //    }
        //    return mealPlan.Meals.Sum(m => (int)m.Recipe.TotalTime.TotalMinutes);
        //}
        public async Task DeleteById(string id)
        {
            MealPlan? mealPlanToDelete = await this.mealplanRepository.GetByIdAsync(id);

            if (mealPlanToDelete != null)
            {
                this.dbContext.Meals.RemoveRange(mealPlanToDelete.Meals);
            }

            this.dbContext.MealPlans.Remove(mealPlanToDelete!);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
