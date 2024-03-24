namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            string userId = User.GetId();
            if(string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if(statusCode == 400 || statusCode == 404)
            {
                return this.View("Error404");
            }
            
            return View();
        }
    }
}
