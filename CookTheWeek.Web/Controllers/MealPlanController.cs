namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MealPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
