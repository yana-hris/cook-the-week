namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MealController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
