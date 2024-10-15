namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
   
    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.MealPlan;

    using static CookTheWeek.Common.NotificationMessagesConstants;
    using static CookTheWeek.Common.TempDataConstants;
    using static CookTheWeek.Common.EntityValidationConstants.MealPlanValidation;

    public class MealPlanController : BaseController
    {
        private readonly IMealPlanViewModelFactory viewModelFactory;
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;

        public MealPlanController(IMealPlanService mealPlanService,
            IValidationService validationService,
            IMealPlanViewModelFactory viewModelFactory,
            ILogger<MealPlanController> logger) : base(logger)
        {
            this.mealPlanService = mealPlanService;
            this.validationService = validationService;
            this.viewModelFactory = viewModelFactory;
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateMealPlanModel([FromBody] MealPlanServiceModel model)
        {
            // Fast return in case of invalid data received
            if (!ModelState.IsValid)
            {
                return LogAndReturnBadRequestWithModelState("Invalid service model received.");
            }

            // Perform additional validation
            var validationResult = await ValidateMealPlanModelAsync(model);

            if (!validationResult.IsValid)
            {
                return LogAndReturnBadRequestWithModelState(string.Join(Environment.NewLine, validationResult.Errors));
            }

            // If model is valid, create the view model for Add view
            try
            {
                MealPlanAddFormModel mealPlanModel = await this.viewModelFactory.CreateMealPlanAddFormModelAsync(model);

                // Store the model in TempData for the next request
                TempData[MealPlanStoredModel] = JsonConvert.SerializeObject(mealPlanModel);
                return Ok();  
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"MealPlanAddFormModel creation failed due to missing record. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return NotFound();
            }
            catch (Exception ex) when (ex is DataRetrievalException || ex is Exception)
            {
                logger.LogError(ex, "Error retrieving data.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Add(string? returnUrl = null)
        {
            // Retrieve the model from TempData
            if (TempData[MealPlanStoredModel] != null)
            {
                var mealPlanModel = JsonConvert.DeserializeObject<MealPlanAddFormModel>(TempData[MealPlanStoredModel].ToString());

                SetViewData("Add Meal Plan", returnUrl ?? "/Recipe/All");
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

                TempData[SubmissionSuccess] = true;
                TempData[SuccessMessage] = MealPlanSuccessfulSaveMessage;

                if (result?.Value == null)
                {
                    // Log error or handle the issue accordingly
                    logger.LogError("Result value is null.");
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
            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    MealPlanDetailsViewModel model = await this.viewModelFactory.CreateMealPlanDetailsViewModelAsync(guidId);

                    ViewBag.ReturnUrl = returnUrl;
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

                    try
                    {
                        var result = await validationService.ValidateMealPlanMealsAsync(copiedModel);

                        if (result.IsValid)
                        {

                            // Store the model in TempData for the next request
                            TempData[MealPlanStoredModel] = JsonConvert.SerializeObject(copiedModel);

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
                    return HandleException(ex, nameof(CopyMealPlan), guidId);
                }
                return Redirect(returnUrl ?? "/MealPlan/Mine");
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan ID.", code = "400" });

        }
        

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl)
        {
            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    MealPlanEditFormModel model = await viewModelFactory.CreateMealPlanFormModelAsync<MealPlanEditFormModel>(guidId);
                    SetViewData("Edit Meal Plan", returnUrl ?? "/MealPlan/Mine");
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

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan ID.", code = "400" });

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

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
        }

        // Helper method for logging and returning bad request
        private IActionResult LogAndReturnBadRequestWithModelState(string message)
        {
            logger.LogError(message);
            return BadRequest(ModelState);
        }

        // Helper method for custom validation
        private async Task<ValidationResult> ValidateMealPlanModelAsync(MealPlanServiceModel model)
        {
            var validationResult = await this.validationService.ValidateMealPlanServiceModelAsync(model);

            if (!validationResult.IsValid)
            {
                AddModelErrors(validationResult);
            }

            return validationResult;
        }

        // Helper method for adding model errors from the custom validation to the ModelState
        private void AddModelErrors(ValidationResult validationResult)
        {
            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }
    }
}
