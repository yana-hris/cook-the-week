using Microsoft.Extensions.Caching.Memory;

namespace CookTheWeek.Web.Controllers
{
    using System.Globalization;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using Common.HelperMethods;
    using Infrastructure.Extensions;
    using Services.Data.Interfaces;
    using Services.Data.Models.MealPlan;
    using ViewModels.Meal;
    using ViewModels.MealPlan;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.Meal;
    using static Common.EntityValidationConstants.MealPlan;
    using static Common.EntityValidationConstants.Recipe;

    [Authorize]
    public class MealPlanController : Controller
    {
        private readonly IMealPlanService mealPlanService;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;
        private readonly ILogger<MealPlanController> logger;
        private readonly SanitizerHelper sanitizer;
        private readonly IMemoryCache memoryCache;

        public MealPlanController(IMealPlanService mealPlanService,
            IRecipeService recipeService,
            IUserService userService,
            ILogger<MealPlanController> logger,
            IMemoryCache memoryCache)
        {
            this.mealPlanService = mealPlanService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.logger = logger;
            this.sanitizer = new SanitizerHelper();
            this.memoryCache = memoryCache;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateMealPlanModel([FromBody] MealPlanServiceModel serviceModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Invalid model received!");
                return BadRequest();
            }

            string userId = serviceModel.UserId;
            bool userExists = await this.userService.ExistsByIdAsync(userId);

            if (!userExists || userId != User.GetId())
            {
                ModelState.AddModelError(nameof(serviceModel.UserId), "Invalid User Id!");
                logger.LogError($"User with ID {serviceModel.UserId} does not exist.");
                return BadRequest(ModelState);
            }

            ICollection<MealServiceModel> recipes = serviceModel.Meals;
            MealPlanAddFormModel mealPlanModel = new MealPlanAddFormModel()
            {
                Name = "",
                Meals = new List<MealAddFormModel>()
            };

            foreach (var recipe in recipes)
            {
                try
                {
                    bool exists = await this.recipeService.ExistsByIdAsync(recipe.RecipeId);

                    // there might be a case where recipe, previously added to local storage has been deleted, but still exists in local storage array
                    if (exists)
                    {
                        MealAddFormModel mealModel = await this.recipeService.GetForMealByIdAsync(recipe.RecipeId);
                        mealPlanModel.Meals.Add(mealModel);
                    }                    
                   
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while processing recipe ID {recipe.RecipeId}.");
                    return StatusCode(500, StatusCode500InternalServerErrorMessage);
                }
            }

            try
            {
                SaveMealPlanToMemoryCache(mealPlanModel);
                string redirectUrl = Url.Action("Add", "MealPlan")!;
                Response.Headers.Append("X-Redirect", redirectUrl);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("View not loaded!");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {

            string userId = User.GetId();
            string cacheKey = userId + "mealPlan";

            if (memoryCache.TryGetValue(cacheKey, out MealPlanAddFormModel? model))
            {
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    logger.LogError($"Retriveved Meal Plan for user with ID {userId} is null");
                    
                }
            }
            else
            {
                logger.LogError($"Cannot retrieve the Meal Plan for user with id {userId} from Memory Cache");
            }

            return RedirectToAction("All", "Recipe");

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MealPlanAddFormModel model)
        {
            string userId = User.GetId();

            if (string.IsNullOrEmpty(model.Name) || model.Name == DefaultMealPlanName)
            {
                ModelState.AddModelError(nameof(model.Name), NameRequiredErrorMessage);
            }

            if (!model.Meals.Any())
            {
                ModelState.AddModelError(nameof(model.Meals), MealsRequiredErrorMessage);
            }
            else
            {
                foreach (var meal in model.Meals)
                {
                    bool exists = await this.recipeService.ExistsByIdAsync(meal.RecipeId);

                    if (!exists)
                    {
                        ModelState.AddModelError(nameof(meal.RecipeId), RecipeNotFoundErrorMessage);
                    }

                    if(!meal.SelectDates.Contains(meal.Date))
                    {
                        ModelState.AddModelError(meal.Date.ToString(), DateRangeErrorMessage);
                    }

                    if (!DateTime.TryParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                    {
                        ModelState.AddModelError(nameof(meal.Date), DateRangeErrorMessage);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                string formattedMessage = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedMessage;
                logger.LogError($"Mode State is Invalid: {formattedMessage}");

                return View(model);
            }

            model.Name = sanitizer.SanitizeInput(model.Name!);

            try
            {
                string id = await this.mealPlanService.AddAsync(userId, model);
                TempData["SubmissionSuccess"] = true;
                DeleteMealPlanFromMemmoryCache();
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;
                return RedirectToAction("Details", "MealPlan", new { id });

            }
            catch (Exception)
            {
                logger.LogError($"Meal plan with name: \"{model.Name}\" of userId \"{userId}\" unsuccessfully added to the Database!");
                return RedirectToAction("Error500", "Home");
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = User.GetId();

            try
            {
                ICollection<MealPlanAllViewModel> allMine = await this.mealPlanService.MineAsync(userId);
                return View(allMine);
            }
            catch (Exception)
            {
                logger.LogError("Mine Meal Plans unsuccessfully loaded to View Model");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(id);

            if (!exists)
            {
                TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;

                return RedirectToAction("Mine", "MealPlan");
            }

            try
            {
                MealPlanViewModel model = await this.mealPlanService.GetByIdAsync(id);
                model.TotalIngredients = await this.mealPlanService.GetIMealPlanIngredientsCountForDetailsAsync(id);
                model.TotalCookingTimeMinutes = await this.mealPlanService.GetMealPlanTotalMinutesForDetailsAsync(id);

                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("Meal Plan Details unsuccessfully loaded!");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CopyMealPlan(string id)
        {
            string userId = User.GetId();

            bool mealPlanExists = await this.mealPlanService.ExistsByIdAsync(id);
            bool isMealPlanOwner = await this.userService.IsOwnerByMealPlanIdAsync(id, userId);

            if (!mealPlanExists)
            {
                logger.LogError($"Meal Plan with id {id} does not exist!");
                return NotFound();
            }

            if (!isMealPlanOwner)
            {
                logger.LogError($"User with Id {userId} is not the owner of Meal Plan with id {id}");
                return BadRequest();
            }

            MealPlanAddFormModel model = await this.mealPlanService.GetForEditByIdAsync(id);
            model.Id = null;
            model.Name = "";
            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days();
            model.StartDate = DateTime.Today;

            try
            {
                SaveMealPlanToMemoryCache(model);                
                return RedirectToAction("Add", "MealPlan");
            }
            catch (Exception ex)
            {
                logger.LogError("Unsuccessful redirect to Add Meal Plan View!");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(id);

            if(!exists)
            {
                TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;
                logger.LogWarning($"Meal Plan with id {id} does not exist in database!");
                return RedirectToAction("Mine", "MealPlan");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByMealPlanIdAsync(id, userId);

            if (!isOwner)
            {
                TempData[ErrorMessage] = MealPlanOwnerErrorMessage;
                logger.LogWarning("The user id of the meal plan owner and current user do not match!");
                return RedirectToAction("Mine", "MealPlan");
            }

            try
            {
                MealPlanAddFormModel model = await this.mealPlanService.GetForEditByIdAsync(id);
                model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days(model.StartDate); // ensure the plan start date stays the same
                model.Id = id;

                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("Meal Plan model was not successfully loaded for edit!");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MealPlanAddFormModel model)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(model.Id!);

            if (!exists)
            {
                TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;
                logger.LogWarning($"Meal Plan with id {model.Id} does not exist in database!");
                return RedirectToAction("Mine", "MealPlan");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByMealPlanIdAsync(model.Id!, userId);

            if (!isOwner)
            {
                TempData[ErrorMessage] = MealPlanOwnerErrorMessage;
                logger.LogWarning("The mealPlan OwnerId and current userId do not match!");
                return RedirectToAction("Mine", "MealPlan");
            }

            if (model.Name == DefaultMealPlanName)
            {
                ModelState.AddModelError(nameof(model.Name), NameRequiredErrorMessage);
            }

            if (!model.Meals.Any())
            {
                ModelState.AddModelError(nameof(model.Meals), MealsRequiredErrorMessage);
            }
            else
            {
                foreach (var meal in model.Meals)
                {
                    bool recipeExists = await this.recipeService.ExistsByIdAsync(meal.RecipeId);

                    if (!recipeExists)
                    {
                        ModelState.AddModelError(nameof(meal.RecipeId), RecipeNotFoundErrorMessage);
                    }

                    if (!meal.SelectDates.Contains(meal.Date))
                    {
                        ModelState.AddModelError(meal.Date.ToString(), DateRangeErrorMessage);
                    }

                    if (!DateTime.TryParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                    {
                        ModelState.AddModelError(nameof(meal.Date), DateRangeErrorMessage);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                string formattedMessage = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedMessage;
                logger.LogError($"Model State is Invalid: {formattedMessage}");

                return View(model);
            }

            model.Name = sanitizer.SanitizeInput(model.Name!);

            try
            {
                await this.mealPlanService.EditAsync(userId, model);
            }
            catch (Exception)
            {
                logger.LogError($"Meal plan with name: \"{model.Name}\" of userId \"{userId}\" unsuccessfully edited!");
                TempData[ErrorMessage] = StatusCode500InternalServerErrorMessage;
                return RedirectToAction("Error500", "Home");
            }

            TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;
            return RedirectToAction("Mine", "MealPlan");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(id);
            string currentUserId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByMealPlanIdAsync(id, currentUserId);

            if (!exists)
            {
                logger.LogError($"Meal Plan with id {id} does not exist");
                return NotFound();
            }

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = MealPlanOwnerErrorMessage;
                return RedirectToAction("Details", "MealPlan", new { id });
            }

            try
            {
                await this.mealPlanService.DeleteById(id);
                TempData[SuccessMessage] = MealPlanSuccessfulDeleteMessage;
            }
            catch (Exception)
            {
                logger.LogError($"Something went wrong and meal plan with id {id} was not deleted!");
                return BadRequest();
            }

            return RedirectToAction("Mine");
        }

        // Private Helper Methods
        private void SaveMealPlanToMemoryCache(MealPlanAddFormModel mealPlanModel) 
        {
            string cacheKey = User.GetId() + "mealPlan";

            if (memoryCache.TryGetValue(cacheKey, out object? oldMealPlanModel))
            {
                if (oldMealPlanModel != null)
                {
                    memoryCache.Remove(cacheKey);
                }
            }

            memoryCache.Set(cacheKey, mealPlanModel);
        }
        private void DeleteMealPlanFromMemmoryCache()
        {
            string cacheKey = User.GetId() + "mealPlan";

            memoryCache.Remove(cacheKey);
        }
    }
}
