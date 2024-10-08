using Microsoft.Extensions.Caching.Memory;

namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
   
    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static Common.NotificationMessagesConstants;
    using static Common.EntityValidationConstants.MealPlanValidation;

    public class MealPlanController : BaseController
    {
        private readonly IViewModelFactory viewModelFactory;
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;

        private readonly ILogger<MealPlanController> logger;

        public MealPlanController(IMealPlanService mealPlanService,
            IValidationService validationService,
            IViewModelFactory viewModelFactory,
            ILogger<MealPlanController> logger)
        {
            this.mealPlanService = mealPlanService;
            this.logger = logger;
            this.validationService = validationService;
            this.viewModelFactory = viewModelFactory;
        }

        [HttpPost]
        public IActionResult StoreMealPlanInSession([FromBody] MealPlanAddFormModel model)
        {
            // Store the model received from the API controller in the session
            try
            {
                HttpContext.Session.SetObjectAsJson("MealPlanAddFormModel", model);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Mealplan storing in session failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Add(string? returnUrl = null)
        {
            var mealPlanModel = HttpContext.Session.GetObjectFromJson<MealPlanAddFormModel>("MealPlanAddFormModel");

            if (mealPlanModel == null)
            {
                logger.LogError($"Mealplan retrieval from session failed.");
                TempData[ErrorMessage] = "Building mealplan failed!";

                return Redirect(returnUrl ?? "/Home/Index"); // If session is empty
            }

            SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All");
            return View(mealPlanModel);  
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MealPlanAddFormModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All");
                return View(model);
            }

            try
            {
                OperationResult<string> result = await mealPlanService.TryAddMealPlanAsync(model);

                if (!result.Succeeded)
                {
                    SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All");
                    AddCustomValidationErrorsToModelState(result.Errors);

                    return View(model);
                }

                TempData["SubmissionSuccess"] = true;
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;

                HttpContext.Session.Remove("MealPlanAddFormModel");

                return RedirectToAction("Details", "MealPlan", new { result.Value, returnUrl });
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
        public async Task<IActionResult> Mine()
        {
            try
            {
                ICollection<MealPlanAllViewModel> model = await viewModelFactory.CreateMyMealPlansViewModelAsync();
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
            try
            {
                MealPlanDetailsViewModel model = await this.viewModelFactory.CreateMealPlanDetailsViewModelAsync(id);

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch(RecordNotFoundException)
            {
                TempData[ErrorMessage] = MealPlanNotFoundErrorMessage;
                return Redirect(returnUrl ?? "/MealPlan/Mine");

            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Details), id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CopyMealPlan(string mealPlanId, string? returnUrl = null)
        {
            try
            {
                MealPlanAddFormModel copiedModel = await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanAddFormModel>(mealPlanId);

                try
                {
                    var result = await validationService.ValidateMealPlanFormModelAsync(copiedModel);

                    if (result.IsValid)
                    {
                        HttpContext.Session.SetObjectAsJson("MealPlanAddFormModel", copiedModel); // store in session

                        return RedirectToAction("Add", new { returnUrl });
                    }

                    TempData[ErrorMessage] = "Copying meal plan failed!";
                }
                catch (RecordNotFoundException)
                {
                    // TODO: think about how to exclude them and suggest the user a way to proceed
                    TempData[ErrorMessage] = "Meal Plan cannot be copied due to unexisting Recipes.";
                    return Redirect(returnUrl ?? "/MealPlan/Mine");
                }
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(CopyMealPlan), mealPlanId);
            }

            return Redirect(returnUrl ?? "/MealPlan/Mine");
        }
        

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl)
        {
            try
            {
                MealPlanEditFormModel model = await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanEditFormModel>(id);
                SetViewData("Edit Meal Plan", returnUrl ?? "/MealPlan/Mine");
                return View(model);
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), id);
            }

            return Redirect(returnUrl ?? "/MealPlan/Mine");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MealPlanEditFormModel model, string? returnUrl)
        {
            SetViewData("Edit Meal Plan", returnUrl ?? "/MealPlan/Mine");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                OperationResult result = await mealPlanService.TryEditMealPlanAsync(model);

                if (!result.Succeeded)
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }

                TempData[SuccessMessage] = MealPlanSuccessfulEditMessage;
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
                
            }
            catch (Exception ex)
            {
                HandleException(ex, nameof(Edit), model.Id);
            }

            return Redirect(returnUrl ?? "/MealPlan/Mine");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await this.mealPlanService.TryDeleteByIdAsync(id);
                TempData[SuccessMessage] = MealPlanSuccessfulDeleteMessage;
            }
            catch (Exception ex) when (ex is RecordNotFoundException || ex is UnauthorizedUserException)
            {
                TempData[ErrorMessage] = ex.Message;
            }
            catch (Exception ex)
            {
                HandleException(ex, nameof(Delete), id);
            }

            return RedirectToAction("Mine");
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
        private IActionResult HandleException(Exception ex, string actionName, string? mealPlanId = null)
        {
            var mealPlanInfo = mealPlanId != null ? $"Mealplan ID: {mealPlanId}" : "No mealplan ID";
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}, {mealPlanInfo}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
        }
    }
}
