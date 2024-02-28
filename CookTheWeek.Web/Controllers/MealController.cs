namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MealController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
