namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
