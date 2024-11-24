namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
   
    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.Interfaces;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static CookTheWeek.Common.EntityValidationConstants.MealPlanValidation;
    using static CookTheWeek.Common.NotificationMessagesConstants;
    using static CookTheWeek.Common.TempDataConstants;

    public class MealPlanController : BaseController
    {
        private readonly IMealPlanViewModelFactory viewModelFactory;
        private readonly IMealPlanService mealPlanService;
        private readonly IMealPlanValidationService mealplanValidator;
        private readonly bool hasActiveMealplan;

        public MealPlanController(IMealPlanService mealPlanService,
            IMealPlanValidationService mealplanValidator,
            IMealPlanViewModelFactory viewModelFactory,
            IUserContext userContext,
            ILogger<MealPlanController> logger) : base(logger)
        {
            this.mealPlanService = mealPlanService;
            this.mealplanValidator = mealplanValidator;
            this.viewModelFactory = viewModelFactory;
            this.hasActiveMealplan = userContext.HasActiveMealplan;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMealPlanModel([FromBody] MealPlanServiceModel model)
        {
            // Fast return in case of invalid data received
            if (!ModelState.IsValid)
            {
                return LogAndReturnBadRequestWithModelState("Meal plan creation failed. Invalid Service model received.");
            }

            // Perform additional validation
            try
            {
                model = await mealplanValidator.CleanseAndValidateServiceModelAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Meal plan creation failed. Error message: {ex.Message}. Stacktrace: {ex.StackTrace}");
                return BadRequest();
            }

            // Check if the user has an active meal plan or not (Add or Edit model)

            
            try
            {
                IMealPlanFormModel resultModel;
                string redirectUrl;

                if (hasActiveMealplan)
                {
                    Guid existingId = await mealPlanService.GetActiveIdAsync();
                    resultModel = (MealPlanEditFormModel) await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanEditFormModel>(existingId, model.Meals);
                    redirectUrl = "/MealPlan/Edit";
                }
                else
                {
                    resultModel = (MealPlanAddFormModel) await viewModelFactory.CreateMealPlanAddFormModelAsync(model);
                    redirectUrl = "/MealPlan/Add";
                }

                TempData[MealPlanStoredModel] = JsonConvert.SerializeObject(resultModel);
                return Json(new { success = true, redirectUrl = redirectUrl });


            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"Meal plan creation failed. Invalid recipeId. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError($"Meal plan creation failed. Error message: {ex.Message}. Stacktrace: {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Add(string? returnUrl = null)
        {
            // Retrieve the model from TempData
            if (TempData[MealPlanStoredModel] != null)
            {
                var mealPlanModel = JsonConvert.DeserializeObject<MealPlanAddFormModel>(TempData[MealPlanStoredModel].ToString());

                SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All", "image-overlay food-background");
                return View(mealPlanModel);
            }
            
            // Fallback if meal plan was not found in TempData
            logger.LogError($"Mealplan retrieval from TempData failed.");
            TempData[ErrorMessage] = "Building meal plan failed!";

            return Redirect(returnUrl ?? "/Recipe/All");
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MealPlanAddFormModel model, string? returnUrl = null)
        {
            SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All", "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                model = viewModelFactory.AddMealCookSelectDates(model);
                return View(model);
            }

            try
            {
                OperationResult<string> result = await mealPlanService.TryAddMealPlanAsync(model);

                if (!result.Succeeded)
                {
                    model = viewModelFactory.AddMealCookSelectDates(model);
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }

                TempData[SubmissionSuccess] = true;
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;

                if (result?.Value == null)
                {
                    // Log error or handle the issue accordingly
                    logger.LogError("Adding meal plan failes. Result value for meal plan ID is null.");
                    return RedirectToAction("Mine", "MealPlan");
                }
                
                return RedirectToAction("Details", "MealPlan", new { id = result.Value, returnUrl = returnUrl });
            }
            catch (RecordNotFoundException)
            {
                TempData[ErrorMessage] = $"Adding Mealplan {model.Name} failed. Invalid recipe selected.";

                return Redirect(returnUrl ?? "/Home/Index");
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Add), null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine(string? returnUrl)
        {
            try
            {
                ICollection<MealPlanAllViewModel> model = await viewModelFactory.CreateMealPlansMineViewModelAsync();
                SetViewData("My Meal Plans", returnUrl ?? "/Recipe/All");
                return View(model);
                         
            }
            catch(RecordNotFoundException)
            {
                return RedirectToAction("None");
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Mine), null);
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
            
            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    MealPlanDetailsViewModel model = await this.viewModelFactory.CreateMealPlanDetailsViewModelAsync(guidId);

                    SetViewData("Meal Plan Details", returnUrl ?? "/MealPlan/Mine", "image-overlay food-background");
                    return View(model);
                }
                catch (RecordNotFoundException)
                {
                    TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;
                    return Redirect(returnUrl ?? "/MealPlan/Mine");

                }
                catch (Exception ex)
                {
                    return HandleException(ex, nameof(Details), guidId);
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan ID.", code = "400" });
        }

        [HttpPost]
        public async Task<IActionResult> CopyMealPlan(string mealPlanId, string? returnUrl = null)
        {
            if (mealPlanId.TryToGuid(out Guid guidId))
            {
                try
                {
                    MealPlanAddFormModel copiedModel = await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanAddFormModel>(guidId);

                    bool hasDeletedRecipes = mealplanValidator.ValidateIfRecipesWereDeleted(copiedModel);
                   
                    if (hasDeletedRecipes)
                    {
                        TempData[MissingRecipesMessage] = true;
                    }
                       
                    // Store the model in TempData for the next request
                    TempData[MealPlanStoredModel] = JsonConvert.SerializeObject(copiedModel);
                    return RedirectToAction("Add", new { returnUrl });
                   
                }
                catch(ArgumentNullException)
                {
                    TempData[ErrorMessage] = "Meal Plan cannot be copied due to unexisting Recipes.";
                    return Redirect(returnUrl ?? "/MealPlan/Mine");
                }
                catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
                {
                    TempData[ErrorMessage] = ex.Message;
                }
                catch (Exception ex)
                {
                    return HandleException(ex, nameof(CopyMealPlan), guidId);
                }

                return Redirect(returnUrl ?? "/MealPlan/Mine");
            }

            TempData[ErrorMessage] = "Copying meal plan failed!";
            return Redirect(returnUrl ?? "/MealPlan/Mine");

        }
        

        [HttpGet]
        public async Task<IActionResult> Edit(string? id = null, string? returnUrl = null)
        {
            if (TempData[MealPlanStoredModel] != null)
            {
                var mealPlanModel = JsonConvert.DeserializeObject<MealPlanEditFormModel>(TempData[MealPlanStoredModel].ToString());

                SetViewData("Edit Meal Plan", returnUrl ?? "/Recipe/All", "image-overlay food-background");
                return View(mealPlanModel);
            }

            if (id != default)
            {
                if (id.TryToGuid(out Guid guidId))
                {
                    try
                    {
                        MealPlanEditFormModel model = await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanEditFormModel>(guidId);
                        SetViewData("Edit Meal Plan", returnUrl ?? "/MealPlan/Mine", "image-overlay food-background");
                        return View(model);
                    }
                    catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
                    {
                        TempData[ErrorMessage] = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        return HandleException(ex, nameof(Edit), guidId);
                    }

                    return Redirect(returnUrl ?? "/MealPlan/Mine");
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan ID.", code = "400" });

        }

        [HttpPost]
        public async Task<IActionResult> Edit(MealPlanEditFormModel model, string? returnUrl)
        {
            SetViewData("Edit Meal Plan", returnUrl ?? "/MealPlan/Mine", "image-overlay food-background");

            if (model == null || model.StartDate == default)
            {
                return HandleException(new ArgumentNullException(nameof(model)), nameof(Edit));
            }
                

            if (!ModelState.IsValid)
            {
                model = viewModelFactory.AddMealCookSelectDates(model);
                return View(model);
            }

            try
            {
                OperationResult result = await mealPlanService.TryEditMealPlanAsync(model);

                if (!result.Succeeded)
                {
                    model = viewModelFactory.AddMealCookSelectDates(model);
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
                
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), model.Id);
            }

            TempData[SuccessMessage] = MealPlanSuccessfulEditMessage;
            TempData[SubmissionSuccess] = true;
            return RedirectToAction("Details", "MealPlan", new { id = model.Id, returnUrl = returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    await this.mealPlanService.TryDeleteByIdAsync(guidId);
                    TempData[SuccessMessage] = MealPlanSuccessfulDeleteMessage;
                }
                catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
                {
                    TempData[ErrorMessage] = ex.Message;
                }
                catch (Exception ex)
                {
                    HandleException(ex, nameof(Delete), guidId);
                }
                return RedirectToAction("Mine");
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan ID.", code = "400" });
        }

        // HELPER METHODS:

        /// <summary>
        /// Helper method to log error message and return a custom Internal Server Error page
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="actionName"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IActionResult HandleException(Exception ex, string actionName, Guid? mealPlanId = null)
        {
            var mealPlanInfo = mealPlanId != default ? $"Mealplan ID: {mealPlanId.ToString()}" : "No mealplan ID";
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}, {mealPlanInfo}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");
                        
            // Redirect to a custom error page with a generic error message
            string userFriendlyMessage = "An unexpected error occurred. Please try again later.";
            return RedirectToAction("InternalServerError", "Home", new { message = userFriendlyMessage });
        }

        // Helper method for logging and returning bad request
        private IActionResult LogAndReturnBadRequestWithModelState(string message)
        {
            logger.LogError(message);
            return BadRequest(ModelState);
        }

    }
}
