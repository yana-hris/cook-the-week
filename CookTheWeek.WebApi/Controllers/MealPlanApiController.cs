namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using CookTheWeek.Services.Data.Models.MealPlan;
    using CookTheWeek.Web.ViewModels.MealPlan;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Services.Data.Models.Validation;

    [Route("api/mealplan")]
    [ApiController]
    public class MealPlanApiController : ControllerBase
    {
        
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;
        private readonly ILogger<MealPlanApiController> logger;

        public MealPlanApiController(
            IMealPlanService mealPlanService,
            IValidationService validationService,
            ILogger<MealPlanApiController> logger)
        {
           
            this.validationService = validationService;
            this.mealPlanService = mealPlanService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("getMealPlanData")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MealPlanAddFormModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMealPlanDataForAddViewAsync([FromBody] MealPlanServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return LogAndReturnBadRequest("Invalid model received.");
            }

            // Custom validation
            var validationResult = await ValidateMealPlanModelAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<MealServiceModel> recipes = model.Meals;
            MealPlanAddFormModel mealPlanModel = new MealPlanAddFormModel();

            try
            {
                // TODO: create model and throw exceptions
                mealPlanModel = await this.mealPlanService.CreateMealPlanAddFormModelAsync(model);
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return NotFound();
            }
            
            return Ok(mealPlanModel);

        }

        // Helper method for logging and returning bad request
        private IActionResult LogAndReturnBadRequest(string message)
        {
            logger.LogWarning(message);
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
