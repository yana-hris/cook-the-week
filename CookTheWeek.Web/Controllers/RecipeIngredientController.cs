namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RecipeIngredientController : Controller
    {
        //private readonly 

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(RecipeIngredientFormViewModel model)
        {
            
            return View();
        }
    }
}
