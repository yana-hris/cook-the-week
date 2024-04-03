namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealController : Controller
    {
        private readonly IMealplanService mealplanService;

        public MealController(IMealplanService mealplanService)
        {
            this.mealplanService = mealplanService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
