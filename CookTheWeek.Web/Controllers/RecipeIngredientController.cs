namespace CookTheWeek.Web.Controllers
{
    using CookTheWeek.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    [Authorize]
    public class RecipeIngredientController : Controller
    {
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IIngredientService ingredientService;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService)
        {
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientService = ingredientService;
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

        public async Task<IActionResult> GetSuggestions(string input)
        {
            string[] wordSuggestions = await this.ingredientService.GetIngredientSuggestions(input.ToLower());

            return Json(wordSuggestions);
        }
        
    }
}
