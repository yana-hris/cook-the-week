namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseAdminController
    {
        public IActionResult AddRecipeCategory()
        {
            return View();
        }

        public IActionResult AllRecipeCategories()
        {
            return View();
        }

        public IActionResult AddIngredientCategory()
        {
            return View();
        }

        public IActionResult AllIngredientCategories()
        {
            return View();
        }
    }
}
