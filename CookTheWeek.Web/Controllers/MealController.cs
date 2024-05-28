namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Meal;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealController : Controller
    {
        private readonly IMealPlanService mealplanService;
        private readonly IMealService mealService;
        private readonly ILogger<MealController> logger;

        public MealController(IMealPlanService mealplanService, 
            IMealService mealService,
            ILogger<MealController> logger)
        {
            this.mealplanService = mealplanService;
            this.mealService = mealService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool exists = await this.mealService.ExistsByIdAsync(id);            

            if (!exists)
            {
                logger.LogError($"Meal with id {id} does not exist in the DB.");
                return BadRequest();
            }

            try
            {
                MealDetailsViewModel model = await this.mealService.DetailsByIdAsync(id);
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("Meal Details unsuccessfully loaded!");
                return BadRequest();
            }

        }


    }
}
