namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Recipe;
    using Services.Interfaces;
    using Services.Data.Models.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    [Authorize]
    public class RecipeController : Controller
    {

        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;

        public RecipeController(IRecipeService recipeService, 
            ICategoryService categoryService, 
            IRecipeIngredientService recipeIngredientService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllRecipesQueryModel queryModel)
        {
            //ICollection<RecipeAllViewModel> model = await this.recipeService.AllUnsortedUnfilteredAsync();
            AllRecipesFilteredAndPagedServiceModel serviceModel = await this.recipeService.AllAsync(queryModel);
            queryModel.Recipes = serviceModel.Recipes;
            queryModel.TotalRecipes = serviceModel.TotalRecipesCount;
            queryModel.Categories = await this.categoryService.AllRecipeCategoryNamesAsync();
            queryModel.RecipeSortings = Enum.GetValues(typeof(RecipeSorting))
                .Cast<RecipeSorting>()
                .ToDictionary(rs => (int)rs, rs => rs.ToString());
            
            return View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {            
            RecipeFormViewModel model = new RecipeFormViewModel();
            model.Ingredient = new RecipeIngredientFormViewModel()
            {
                Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync()
            };
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = this.recipeService.GenerateServingOptions();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeFormViewModel model)
        {          
            
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = this.recipeService.GenerateServingOptions();

            return View(model);
        }
    }
}
