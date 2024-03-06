namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    using CookTheWeek.Web.ViewModels.Home;
    public class HomeController : Controller
    {
        
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
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
