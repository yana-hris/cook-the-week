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
                Name = "[Your Meal Plan Name]",
                Meals = new List<MealAddFormModel>()
            };

            foreach (var recipe in recipes)
            {
                try
                {
                    bool exists = await this.recipeService.ExistsByIdAsync(recipe.RecipeId);

                    if (exists)
                    {
                        MealAddFormModel mealModel = await this.recipeService.GetForMealByIdAsync(recipe.RecipeId);
                        mealPlanModel.Meals.Add(mealModel);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(recipe.RecipeId), "Ivalid Recipe Id");
                        logger.LogWarning($"Recipe ID {recipe.RecipeId} does not exist.");
                        return BadRequest(ModelState);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while processing recipe ID {recipe.RecipeId}.");
                    return StatusCode(500, "An unexpected error occurred.");
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
                    logger.LogError($"Retriveved value for Meal Plan for user with ID {userId} is null");
                    return BadRequest();
                }
            }
            else
            {
                logger.LogError($"Cannot retrieve value for the Meal Plan for user with id {userId}");
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MealPlanAddFormModel model)
        {
            string userId = User.GetId();

            if (model.Name == DefaultMealPlanName)
            {
                ModelState.AddModelError(nameof(model.Name), "Give your Meal Plan a fancy Name");
            }

            if (!model.Meals.Any())
            {
                ModelState.AddModelError(nameof(model.Meals), "Meals are required!");
            }
            else
            {
                foreach (var meal in model.Meals)
                {
                    bool exists = await this.recipeService.ExistsByIdAsync(meal.RecipeId);

                    if (!exists)
                    {
                        ModelState.AddModelError(nameof(meal.RecipeId), $"No such recipe found: {meal.RecipeId}");
                    }

                    if (!DateTime.TryParseExact(meal.Date, MealDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                    {
                        ModelState.AddModelError(nameof(meal.Date), $"Incorrect Cooking Date Format for recipe with id {meal.RecipeId}");
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

            model.Name = sanitizer.SanitizeInput(model.Name);

            try
            {
                await this.mealPlanService.AddAsync(userId, model);
                TempData["SubmissionSuccess"] = true;
                DeleteMealPlanFromMemmoryCache();

            }
            catch (Exception ex)
            {
                logger.LogError($"Meal plan with name: \"{model.Name}\" of userId \"{userId}\" unsuccessfully added to the Database!");
                return BadRequest();
            }

            TempData[SuccessMessage] = $"Your Meal Plan \"{model.Name}\" was successfully Saved! You can go to Meal Plans and edit it if needed.";
            return RedirectToAction("All", "Recipe");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = this.User.GetId();

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
        public async Task<IActionResult> CopyMealPlan(string id)
        {
            string userId = User.GetId();

            bool mealPlanExists = await this.mealPlanService.ExistsByIdAsync(id);
            bool isMealPlanOwner = await this.userService.IsOwnerByMealPlanId(userId, id);

            if (!mealPlanExists)
            {
                logger.LogError($"Meal Plan with id {id} does not exist!");
                return NotFound();
            }

            if (!isMealPlanOwner)
            {
                logger.LogError($"User with Id {userId} is not the owner of Meal Plan with id {id}");
                return Unauthorized();
            }

            MealPlanAddFormModel model = await this.mealPlanService.GetForEditByIdAsync(id);
            model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days();

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
