namespace CookTheWeek.Services.Data
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Common.Exceptions;
    using Common.Extensions;
    using Common.HelperMethods;
    using CookTheWeek.Data;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using Interfaces;
    using Web.ViewModels.Admin.MealPlanAdmin;
    using Web.ViewModels.Meal;
    using Web.ViewModels.MealPlan;

    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.Recipe;
    using static Common.ExceptionMessagesConstants;

    public class MealPlanService : IMealPlanService
    {
        private readonly IMealplanRepository mealplanRepository;
        private readonly IMealService mealService;

        public MealPlanService(IMealService mealService,
            IMealplanRepository mealplanRepository)
        {
            this.mealplanRepository = mealplanRepository;
            this.mealService = mealService;
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

        
        /// <inheritdoc/>
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

            string id = await this.mealplanRepository.AddAsync(newMealPlan);

            if (String.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessages.MealplanUnsuccessfullyAddedExceptionMessage);
            }

            return newMealPlan.Id.ToString();
           
        }

        // TODO: use repository
        /// <inheritdoc/>
        public async Task EditAsync(string userId, MealPlanEditFormModel model)
        {
            MealPlan? mealPlan = await this.mealplanRepository.GetByIdAsync(model.Id);

            if (mealPlan == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            mealPlan.Name = model.Name;
            

            await this.mealService.DeleteAllByMealPlanIdAsync(model.Id);
            await this.mealService.AddAllAsync(model.Meals);
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
        public async Task<MealPlanEditFormModel> GetForEditByIdAsync(string id)
        {
            MealPlan? mealplan = await this.mealplanRepository.GetByIdAsync(id);

            if (mealplan == null)
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }

            MealPlanEditFormModel model = new MealPlanEditFormModel()
            {
                Id = mealplan.Id.ToString(),
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

            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate);

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

        
        /// <inheritdoc/>
        public async Task DeleteById(string id)
        {
            MealPlan? mealPlanToDelete = await this.mealplanRepository.GetByIdAsync(id);

            if (mealPlanToDelete != null)
            {
                await this.mealService.DeleteAllByMealPlanIdAsync(id);
                await this.mealplanRepository.DeleteByIdAsync(mealPlanToDelete);
            }
            else
            {
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.MealplanNotFoundExceptionMessage, null);
            }
        }

        public Task<MealPlanDetailsViewModel> GetForDetailsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var userMealplans = await this.mealplanRepository
                .GetAllQuery()
                .Where(mp => mp.OwnerId.ToString().ToLower() == userId.ToLower())
                .ToListAsync();

            if (userMealplans.Any())
            {
                foreach (var mealpan in userMealplans)
                {
                    await this.mealService.DeleteAllByMealPlanIdAsync(mealpan.Id.ToString());
                }
                await this.mealplanRepository.DeleteAllAsync(userMealplans);
            }

        }
    }
}
