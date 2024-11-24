namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class MealController : BaseController
    {
        
        private readonly IMealViewModelFactory viewModelFactory;
        private readonly IMealService mealService;

        public MealController(IMealViewModelFactory viewModelFactory,
            IMealService mealService,
            ILogger<MealController> logger) : base(logger) 
        {
            this.viewModelFactory = viewModelFactory;
            this.mealService = mealService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, bool isMealPlanFinished, string? returnUrl)
        {
            try
            {
                MealDetailsViewModel model = await this.viewModelFactory.CreateMealDetailsViewModelAsync(id, isMealPlanFinished);
                SetViewData("Meal Details", returnUrl ?? "/MealPlan/Mine", "image-overlay food-background");
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code =  ex.ErrorCode});
            }    
            catch(Exception ex) //in case of all database exceptions
            {
                return HandleException(ex, nameof(Details), id);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Cook(int id)
        {
            try
            {
                if (id != default)
                {
                    await mealService.TryMarkAsCooked(id);
                    return Ok();
                }                
                logger.LogError($"MealId is null.");

            }
            catch (Exception ex)
            {
                logger.LogError($"An error occured: {ex.Message}. Error stacktrace: {ex.StackTrace}");
                
            }

            return BadRequest();
        }

        public async Task<IActionResult> Uncook(int id)
        {
            try
            {
                if (id != default)
                {
                    await mealService.TryMarkUncooked(id);
                    return Ok();
                }
                logger.LogError($"MealId is null");
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occured: {ex.Message}. Error stacktrace: {ex.StackTrace}");
            }

            return BadRequest();
        }

        /// <summary>
        /// Helper method to log error message and return a custom Internal Server Error page
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="actionName"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IActionResult HandleException(Exception ex, string actionName, int mealId)
        {
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}, MealId: {mealId}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

            // Redirect to a custom error page with a generic error message
            string userFriendlyMessage = "An unexpected error occurred. Please try again later.";
            return RedirectToAction("InternalServerError", "Home", new { message = userFriendlyMessage });
        }
    }
}
