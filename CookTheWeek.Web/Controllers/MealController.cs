namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Web.ViewModels.Meal;
    using CookTheWeek.Services.Data.Services.Interfaces;

    public class MealController : BaseController
    {
        private readonly IMealService mealService;
        private readonly ILogger<MealController> logger;

        public MealController(IMealService mealService,
        ILogger<MealController> logger)
        {
            
            this.mealService = mealService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                MealDetailsViewModel model = await this.mealService.GetForDetailsAsync(id);
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"Meal with id {id} does not exist in the DB. Exceprion StackTrace: {ex.StackTrace}");
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code =  ex.ErrorCode});
            }    
            catch(Exception ex)
            {
                logger.LogError($"Meal Details unsuccessfully loaded! Exception error message: {ex.Message}; Exception StackTrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
            }

        }
    }
}
