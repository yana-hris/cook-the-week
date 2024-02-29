using CookTheWeek.Services.Interfaces;
using CookTheWeek.Web.ViewModels.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookTheWeek.Web.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {

        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public RecipeController(IRecipeService recipeService, ICategoryService categoryService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            ICollection<RecipeAllViewModel> model = await this.recipeService.GetAllRecipesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.ServingOptions = this.recipeService.GenerateServingOptions();

            RecipeFormViewModel model = new RecipeFormViewModel();
            model.Categories = await this.categoryService.GetAllRecipeCategoriesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeFormViewModel model)
        {          
            
            model.Categories = await this.categoryService.GetAllRecipeCategoriesAsync();

            return View(model);
        }
    }
}
