namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;

    using static Common.GeneralApplicationConstants;

    [AllowAnonymous]
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {
            if(this.User.IsAdmin())
            {
                return this.RedirectToAction("Index", "HomeAdmin", new { Area = AdminAreaName });
            }

            string userId = User.GetId();
            if(string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        public IActionResult About()
        {
            string userId = User.GetId();
            if (string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        public IActionResult HowItWorks()
        {
            string userId = User.GetId();
            if (string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if(statusCode != null) 
            {
                if(statusCode.Value == 404 || statusCode.Value == 401)
                {
                    return View("Error404");
                }
                else
                {
                    return View("Error500");
                }
                
            } 
            
            return View("Error");
        }
    }
}
