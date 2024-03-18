namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Recipe;
    using Services.Interfaces;
    using Services.Data.Models.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;

    using static Common.NotificationMessagesConstants;
    using static Common.EntityValidationConstants.Recipe;
    using CookTheWeek.Data.Models;

    [Authorize]
    public class RecipeController : Controller
    {

        private readonly IRecipeService recipeService;
        private readonly IIngredientService ingredientService;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;

        public RecipeController(IRecipeService recipeService, 
            ICategoryService categoryService, 
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientService = ingredientService;
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
            RecipeFormViewModel model = new RecipeFormViewModel()
            {
                RecipeIngredients = new List<RecipeIngredientFormViewModel>()
                {
                    new RecipeIngredientFormViewModel()
                    {
                        Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                        Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync()
                    }
                },
                Categories = await this.categoryService.AllRecipeCategoriesAsync(),
                ServingsOptions = ServingsOptions,
            };            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeFormViewModel model)
        {
            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
                model.ServingsOptions = ServingsOptions;
                
                model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
                model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();
                       

                ICollection<string> modelErrors =  ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.recipeService.AddAsync(model);
                TempData[SuccessMessage] = "Your recipe was successfully added!";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = $"Something went wrong and the ingredient was not added! {ex.Message}";

                model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
                model.ServingsOptions = ServingsOptions;
                model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
                model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

                return View(model);
            }
        }        
        private async Task<bool> IsIngredientValid(string ingredientName)
        {
            if(!string.IsNullOrEmpty(ingredientName))
            {
                return await this.ingredientService.ExistsByNameAsync(ingredientName);
            }            
            return false;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if(!exists)
            {
                return NotFound();
                // TODO: Implement different logic for such requests
            }

            try
            {
                RecipeDetailsViewModel? model = await this.recipeService.DetailsByIdAsync(id);
                if(model != null)
                {
                    return View(model);
                }                
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = "Recipe not found!";
            }

            return NotFound();
        }
    }
}
