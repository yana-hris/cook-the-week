namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealController : Controller
    {
        private readonly IMealPlanService mealplanService;

        public MealController(IMealPlanService mealplanService)
        {
            this.mealplanService = mealplanService;
        }

        [HttpGet]
        public async Task<IActionResult> Cook(int id)
        {

            //var model = await this.mealService.GetMeal(id);
            //return View(model);
            return View();
        }
    }
}
