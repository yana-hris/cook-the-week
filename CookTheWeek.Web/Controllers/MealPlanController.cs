using Microsoft.Extensions.Caching.Memory;

namespace CookTheWeek.Web.Controllers
{
    using System.Globalization;


    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
   
    using CookTheWeek.Common.Exceptions;
    using Infrastructure.Extensions;
    using Services.Data.Services.Interfaces;
    using Services.Data.Models.MealPlan;
    using ViewModels.MealPlan;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using static Common.EntityValidationConstants.MealValidation;
    using static Common.EntityValidationConstants.MealPlanValidation;
    using static Common.EntityValidationConstants.RecipeValidation;

    public class MealPlanController : BaseController
    {
        private readonly IMealPlanService mealPlanService;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;
        private readonly ILogger<MealPlanController> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IValidationService validationService;

        public MealPlanController(IMealPlanService mealPlanService,
            IRecipeService recipeService,
            IValidationService validationService,
            IUserService userService,
            ILogger<MealPlanController> logger,
            IMemoryCache memoryCache)
        {
            this.mealPlanService = mealPlanService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.validationService = validationService;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] //TODO: Refacor and move model creation to viewmodel factory
        public async Task<IActionResult> CreateMealPlanModel([FromBody] MealPlanServiceModel serviceModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Invalid model received!");
                return BadRequest();
            }

            //string userId = serviceModel.UserId;
            ////bool userExists = await this.userService.ExistsByIdAsync(userId);

            //if (!userExists || userId != User.GetId())
            //{
            //    ModelState.AddModelError(nameof(serviceModel.UserId), "Invalid User Id!");
            //    logger.LogError($"User with ID {serviceModel.UserId} does not exist.");
            //    return BadRequest(ModelState);
            //}

            //ICollection<MealServiceModel> recipes = serviceModel.Meals;
            //MealPlanAddFormModel mealPlanModel = new MealPlanAddFormModel()
            //{
            //    Name = "",
            //    Meals = new List<MealAddFormModel>()
            //};

            //foreach (var recipe in recipes)
            //{
            //    try
            //    {
            //        //bool exists = await this.recipeService.ExistsByIdAsync(recipe.RecipeId);

            //        // there might be a case where recipe, previously added to local storage has been deleted, but still exists in local storage array
            //        //if (exists)
            //        //{
            //        //    MealAddFormModel mealModel = await this.recipeService.GetForMealByIdAsync(recipe.RecipeId);
            //        //    mealPlanModel.Meals.Add(mealModel);
            //        //}                    
                   
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex, $"An error occurred while processing recipe ID {recipe.RecipeId}.");
            //        return StatusCode(500, StatusCode500InternalServerErrorMessage);
            //    }
            //}

            try
            {
                //SaveMealPlanToMemoryCache(mealPlanModel);
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
        public IActionResult Add(string? returnUrl = null)
        {

            string userId = User.GetId();
            string cacheKey = userId + "mealPlan";

            if (memoryCache.TryGetValue(cacheKey, out MealPlanAddFormModel? model))
            {
                if (model != null)
                {
                    ViewBag.ReturnUrl = returnUrl;
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
        public async Task<IActionResult> Add([FromForm] MealPlanAddFormModel model, string? returnUrl = null)
        {
            string userId = User.GetId();

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
                ViewBag.ReturnUrl = returnUrl;

                return View(model);
            }
            
            try
            {
                string mealPlanId = await this.mealPlanService.AddAsync(userId, model);
                TempData["SubmissionSuccess"] = true;
                DeleteMealPlanFromMemmoryCache();
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;
                return RedirectToAction("Details", "MealPlan", new { mealPlanId, returnUrl });

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Meal plan was not added!");
                return BadRequest();
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = User.GetId();

            try
            {
                ICollection<MealPlanAllViewModel> model = await this.mealPlanService.MineAsync(userId);
                if (model.Count > 0)
                {
                    return View(model);
                }
                else
                {
                    return RedirectToAction("None");
                }                
            }
            catch (Exception)
            {
                logger.LogError("Mine Meal Plans unsuccessfully loaded to View Model");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult None()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string? returnUrl = null)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(id);

            if (!exists)
            {
                TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;

                return RedirectToAction("Mine", "MealPlan");
            }

            try
            {
                MealPlanDetailsViewModel model = await this.mealPlanService.GetForDetailsAsync(id);

                ViewBag.ReturnUrl = returnUrl;
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
            //bool isMealPlanOwner = await this.userService.IsMealplanOwnerByIdAsync(id, userId);

            //if (!mealPlanExists)
            //{
            //    logger.LogError($"Meal Plan with id {id} does not exist!");
            //    return NotFound();
            //}

            //if (!isMealPlanOwner)
            //{
            //    logger.LogError($"User with Id {userId} is not the owner of Meal Plan with id {id}");
            //    return BadRequest();
            //}

            //MealPlanAddFormModel model = await this.mealPlanService.GetForEditByIdAsync(id);
            //model.Id = null;
            //model.Name = "";
            //model.Meals.First().SelectDates = DateGenerator.GenerateNext7Days();
            //model.StartDate = DateTime.Today;

            try
            {
                //SaveMealPlanToMemoryCache(model);                
                return RedirectToAction("Add", "MealPlan");
            }
            catch (Exception ex)
            {
                logger.LogError("Unsuccessful redirect to Add Meal Plan View!");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl)
        {
            string userId = User.GetId();

            try
            {
                var model = await this.mealPlanService.GetForEditByIdAsync(id, userId);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
                return Redirect(returnUrl ?? "/MealPlan/Mine");
            }
            catch (Exception ex)
            {
                logger.LogError($"Getting meal plan edit data failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home", new {message = ex.Message});
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(MealPlanEditFormModel model, string? returnUrl)
        {
            string userId = User.GetId();

            if (returnUrl == null)
            {
                returnUrl = "/MealPlan/Mine";
            }
            ViewBag.ReturnUrl = returnUrl;

            

            var validationResult = await this.validationService.ValidateMealPlanEditFormModelAsync(model);
            AddValidationErrorsToModelState(validationResult);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.mealPlanService.EditAsync(userId, model);
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;                
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                logger.LogError($"Meal plan edit failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
            }

            return RedirectToAction("Mine", "MealPlan");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exists = await this.mealPlanService.ExistsByIdAsync(id);
            string currentUserId = User.GetId();
            //bool isOwner = await this.userService.IsMealplanOwnerByIdAsync(id, currentUserId);

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
