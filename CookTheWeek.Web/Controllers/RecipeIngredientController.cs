namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Interfaces;
    using ViewModels.RecipeIngredient;

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
        
    }
}
