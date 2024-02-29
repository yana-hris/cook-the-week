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

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            ICollection<RecipeAllViewModel> model = await this.recipeService.GetAllRecipesAsync();

            return View(model);
        }
    }
}
