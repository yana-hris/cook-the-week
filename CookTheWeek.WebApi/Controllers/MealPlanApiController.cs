namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Factories;

    [Route("api/mealplan")]
    [ApiController]
    public class MealPlanApiController : ControllerBase
    {
        
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;
        private readonly IMealPlanViewModelFactory viewModelFactory;
        private readonly ILogger<MealPlanApiController> logger;

        public MealPlanApiController(
            IMealPlanService mealPlanService,
            IValidationService validationService,
            IMealPlanViewModelFactory viewModelFactory,
            ILogger<MealPlanApiController> logger)
        {
           
            this.validationService = validationService;
            this.mealPlanService = mealPlanService;
            this.viewModelFactory = viewModelFactory;
            this.logger = logger;
        }

        [HttpPost]
        [Route("CreateMealPlanModel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MealPlanAddFormModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            return Ok();

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
