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
        public IActionResult Index()
        {
            return View();
        }
    }
}
