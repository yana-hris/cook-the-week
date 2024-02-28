namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
