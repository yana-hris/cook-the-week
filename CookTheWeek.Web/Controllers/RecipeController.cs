namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using ViewModels.Recipe;
    using ViewModels.Recipe.Enums;
    using ViewModels.RecipeIngredient;
    using Services.Interfaces;
    using Services.Data.Models.Recipe;

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
            RecipeAddFormModel model = new RecipeAddFormModel()
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RecipeAddFormModel model)
        {
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            bool categoryExists =
                await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.RecipeCategoryId), "Selected category does not exist!");
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
                if(!await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), $"Invalid ingredient measure for ingrediet {ingredient.Name}");
                }
                if(ingredient.SpecificationId != null)
                {
                    if (!await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                    {
                        ModelState.AddModelError(nameof(ingredient.SpecificationId), $"Invalid ingredient specification for ingrediet {ingredient.Name}");
                    }
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
                string ownerId = User.GetId();
                await this.recipeService.AddAsync(model, ownerId);
                TempData[SuccessMessage] = "Your recipe was successfully added!";
                return RedirectToAction("All");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if (!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";
                return RedirectToAction("All", "Recipe");
            }

            string ownerId = User.GetId();
            bool isOwner = await this.recipeService.IsOwner(id, ownerId);

            if(!isOwner) 
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to edit recipe info!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            try
            {
                RecipeEditFormModel model = await this.recipeService.GetForEditByIdAsync(id);
                model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
                model.ServingsOptions = ServingsOptions;
                model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
                model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

                return View(model);
            }
            catch (Exception)
            {
                return BadRequest(); // TODO: check app logic!
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecipeEditFormModel model)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(model.Id);

            if (!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";
                return RedirectToAction("All", "Recipe");
            }

            string ownerId = User.GetId();
            bool isOwner = await this.recipeService.IsOwner(model.Id, ownerId);

            if (!isOwner)
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to edit recipe info!";
                return RedirectToAction("Details", "Recipe", new { id = model.Id });
            }

            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            model.RecipeIngredients!.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients!.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            bool categoryExists =
               await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.RecipeCategoryId), "Selected category does not exist!");
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
                if (!await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), $"Invalid measure for ingrediet {ingredient.Name}");
                }
                if (ingredient.SpecificationId != null)
                {
                    if (!await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                    {
                        ModelState.AddModelError(nameof(ingredient.SpecificationId), $"Invalid specification for ingrediet {ingredient.Name}");
                    }
                }
            }

            if (!ModelState.IsValid)
            {                
                return View(model);
            }

            try
            {
                await this.recipeService.EditAsync(model);
                TempData[SuccessMessage] = "Your recipe was successfully edited!";
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to update the house. Please try again later or contact administrator!");
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if(!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";

                return RedirectToAction("All", "Recipe");
            }

            try
            {
                RecipeDetailsViewModel model = await this.recipeService.DetailsByIdAsync(id);
                
                return View(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }        

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            string currentUserId = User.GetId();
            bool isOwner = await this.recipeService.IsOwner(id, currentUserId);

            if(!exists)
            {
                return NotFound();
            }

            if(!isOwner)
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to delete it!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            try
            {
                RecipeDeleteViewModel model = await this.recipeService.GetForDeleteByIdAsync(id);

                return View(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }           
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            string currentUserId = User.GetId();
            bool isOwner = await this.recipeService.IsOwner(id, currentUserId);

            if (!exists)
            {
                return NotFound();
            }

            if (!isOwner)
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to delete it!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            try
            {
                await this.recipeService.DeleteById(id);
                TempData[SuccessMessage] = "Recipe successfully deleted!";
            }
            catch (Exception)
            {
                return BadRequest();
            }

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
