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
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
            }

            if (!ModelState.IsValid)
            {
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
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if (!exists)
            {
                // TODO: check application logic
                return BadRequest();
            }

            RecipeEditViewModel model = await this.recipeService.GetByIdAsync(id);
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;
            model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipeEditViewModel model)
        {
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
            }

            if (!ModelState.IsValid)
            {
                ICollection<string> modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                var formattedErrors = string.Join(Environment.NewLine, modelErrors);
                TempData[ErrorMessage] = formattedErrors;
                return View(model);
            }

            try
            {
                await this.recipeService.EditAsync(model);
                TempData[SuccessMessage] = "Your recipe was successfully edited!";
                return RedirectToAction("Details", new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = $"Something went wrong and the ingredient was not edited! {ex.Message}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if(!exists)
            {
                // TODO: check application logic
                return NotFound();
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

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            //string currentUserId = GetUserId();
            //bool isOwner = await this.recipeService.IsOwnerById(currentUserId, id);

            if (exists) //TODO: add (&& isOwner)
            {
                RecipeDeleteViewModel? model = await this.recipeService.GetByIdForDelete(id);
                if (model != null)
                {
                    return View(model);
                }
            }
            TempData[ErrorMessage] = "Recipe unsuccessfully deleted!";
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await this.recipeService.DeleteById(id);
            TempData[SuccessMessage] = "Recipe deleted!";
            return RedirectToAction("All");
        }

        private async Task<bool> IsIngredientValid(string ingredientName)
        {
            if (!string.IsNullOrEmpty(ingredientName))
            {
                return await this.ingredientService.ExistsByNameAsync(ingredientName);
            }
            return false;
        }

    }
}
