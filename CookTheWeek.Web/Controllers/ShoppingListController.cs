namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
