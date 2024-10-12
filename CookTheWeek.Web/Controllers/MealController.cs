namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Web.ViewModels.Meal;

    public class MealController : BaseController
    {
        
        private readonly IViewModelFactory viewModelFactory;

        public MealController(IViewModelFactory viewModelFactory,
            ILogger<MealController> logger) : base(logger) 
        {
            this.viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                MealDetailsViewModel model = await this.viewModelFactory.CreateMealDetailsViewModelAsync(id);
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

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
        }
    }
}
