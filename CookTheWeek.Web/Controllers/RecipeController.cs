namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Recipe;
    using Services.Interfaces;
    using Services.Data.Models.Recipe;
    using CookTheWeek.Web.ViewModels.Recipe.Enums;
    using CookTheWeek.Web.ViewModels.RecipeIngredient;
    using CookTheWeek.Data.Models;

    using static Common.NotificationMessagesConstants;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using Microsoft.EntityFrameworkCore;

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
                RecipeIngredients = new HashSet<RecipeIngredientFormViewModel>()
                {
                    new RecipeIngredientFormViewModel()
                    {
                        Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                        Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync()
                    }
                },                
                Categories = await this.categoryService.AllRecipeCategoriesAsync(),
                ServingsOptions = this.recipeService.GenerateServingOptions()
            };            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeFormViewModel model)
        {
            
            if(model.RecipeIngredients != null && model.RecipeIngredients.Any())
            {
                ICollection<RecipeIngredientFormViewModel> nestedCollectionModel = model.RecipeIngredients.ToList();

                if (!await IsNestedModelValid(nestedCollectionModel))
                {
                    ModelState.AddModelError(nameof(model.RecipeIngredients), "Invalid recipe ingredient(s)!");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
                model.ServingsOptions = await this.recipeService.GenerateServingOptions();

                if (model.RecipeIngredients == null || (model.RecipeIngredients != null && !model.RecipeIngredients.Any()))
                {
                    model.RecipeIngredients = new HashSet<RecipeIngredientFormViewModel>()
                    {
                        new RecipeIngredientFormViewModel(){ }
                    };
                }
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
                await this.recipeService.AddRecipeAsync(model);
                TempData[SuccessMessage] = "Your recipe was successfully added!";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = $"Something went wrong and the ingredient was not added! {ex.Message}";
            }

            return View(model);
        }

        private async Task<bool> IsNestedModelValid(ICollection<RecipeIngredientFormViewModel> model)
        {
            foreach (var ingredient in model)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    TempData[ErrorMessage] = $"Ingredient with name {ingredient.Name} does not exist! Please first add it from the Add Ingredient menu!";
                    return false;
                }
                if(!ModelState.IsValid)
                {
                    ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                    string errorMessage = string.Format($"Invalid ingredient: {ingredient.Name}! Errors: {string.Join(Environment.NewLine, modelErrors)}");
                    TempData[ErrorMessage] = errorMessage;
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> IsIngredientValid(string ingredientName)
        {
            // Perform validation check for ingredient existence in the database
            return await this.ingredientService.ExistsByNameAsync(ingredientName);
        }
    }
}
