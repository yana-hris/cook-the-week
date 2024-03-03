namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RecipeIngredientController : Controller
    {
        private readonly IRecipeIngredientService recipeIngredientService;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService)
        {
            this.recipeIngredientService = recipeIngredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            RecipeIngredientFormViewModel model = new RecipeIngredientFormViewModel();
            model.Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeIngredientFormViewModel model)
        {
            
            return View();
        }
    }
}
