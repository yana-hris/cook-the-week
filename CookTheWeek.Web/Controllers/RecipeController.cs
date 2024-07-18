namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common.HelperMethods;
    using Infrastructure.Extensions;
    using ViewModels.Recipe;
    using ViewModels.Recipe.Enums;
    using ViewModels.RecipeIngredient;
    using ViewModels.Step;
    using Services.Data.Interfaces;
    using Services.Data.Models.Recipe;

    using static Common.NotificationMessagesConstants;
    using static Common.EntityValidationConstants.Recipe;
    using Newtonsoft.Json;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    [Authorize]
    public class RecipeController : Controller
    {

        private readonly IRecipeService recipeService;
        private readonly IIngredientService ingredientService;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IUserService userService;
        private readonly IFavouriteRecipeService favouriteRecipeService;
        private readonly ILogger<RecipeController> logger;
        private readonly SanitizerHelper sanitizer;

        public RecipeController(IRecipeService recipeService,
            ICategoryService categoryService,
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService,
            IUserService userService,
            IFavouriteRecipeService favouriteRecipeService,
            ILogger<RecipeController> logger)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientService = ingredientService;
            this.userService = userService;
            this.favouriteRecipeService = favouriteRecipeService;
            this.logger = logger;
            this.sanitizer = new SanitizerHelper();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllRecipesQueryModel queryModel)
        {
            try
            {
                AllRecipesFilteredAndPagedServiceModel serviceModel = await this.recipeService.AllAsync(queryModel);

                queryModel.SearchString = sanitizer.SanitizeInput(queryModel.SearchString);
                queryModel.Recipes = serviceModel.Recipes;
                queryModel.TotalRecipes = serviceModel.TotalRecipesCount;
                queryModel.Categories = await this.categoryService.AllRecipeCategoryNamesAsync();
                queryModel.RecipeSortings = Enum.GetValues(typeof(RecipeSorting))
                    .Cast<RecipeSorting>()
                    .ToDictionary(rs => (int)rs, rs => rs.ToString());

                ViewData["Title"] = "All Recipes";

                return View(queryModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the All recipes action");
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            RecipeAddFormModel model = new RecipeAddFormModel()
            {
                RecipeIngredients = new List<RecipeIngredientFormModel>()
                {
                    new RecipeIngredientFormModel()
                    {
                        Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync(),
                        Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync()
                    }
                },
                Steps = new List<StepFormModel>()
                {
                    new StepFormModel()
                },
                Categories = await this.categoryService.AllRecipeCategoriesAsync(),
                ServingsOptions = ServingsOptions,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeAddFormModel model)
        {
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            if (!model.RecipeIngredients.Any())
            {
                model.RecipeIngredients = new List<RecipeIngredientFormModel>()
                {
                    new RecipeIngredientFormModel()
                };
            }

            if (!model.Steps.Any())
            {
                model.Steps = new List<StepFormModel>()
                {
                    new StepFormModel()
                };
            }

            model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            if (model.RecipeCategoryId.HasValue && model.RecipeCategoryId != default)
            {
                bool categoryExists = await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId.Value);
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(model.RecipeCategoryId), "Selected category does not exist!");
                }
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (ingredient.Name != default && !await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
                if (ingredient.MeasureId.HasValue && !await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), $"Invalid ingredient measure for ingrediet {ingredient.Name}");
                }
                if (ingredient.SpecificationId.HasValue && !await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.SpecificationId), $"Invalid ingredient specification for ingrediet {ingredient.Name}");
                }
            }

            if (!ModelState.IsValid)
            {
                // Collect server-side validation errors
                var serverErrors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                // Serialize the errors to JSON and store them in TempData
                TempData["ServerErrors"] = JsonConvert.SerializeObject(serverErrors);

                return View(model);
            }
            // Sanitize all string input
            model.Title = sanitizer.SanitizeInput(model.Title);
            if (model.Description != null)
            {
                model.Description = sanitizer.SanitizeInput(model.Description);
            }
            if (model.Steps.Any())
            {
                foreach (var step in model.Steps)
                {
                    step.Description = sanitizer.SanitizeInput(step.Description);
                }
            }

            try
            {
                string ownerId = User.GetId();
                string recipeId = await this.recipeService.AddAsync(model, ownerId);
                TempData[SuccessMessage] = "Your recipe was successfully added!";
                return RedirectToAction("Details", new { id = recipeId });
            }
            catch (Exception)
            {
                logger.LogError("Reicpe was not added!");
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
                logger.LogWarning($"Recipe with id {id} does not exist in database!");
                return RedirectToAction("All", "Recipe");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByRecipeIdAsync(id, userId);

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to edit recipe info!";
                logger.LogWarning("The user id of the recipe owner and current user do not match!");
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
                logger.LogError("Recipe model was not successfully loaded for edit!");
                return BadRequest();
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Edit([FromBody] RecipeEditFormModel model)
        {
            if (model == null)
            {
                logger.LogError("Unsuccessful model binding from ko.JSON to RecipeEditFormModel");
                return BadRequest(new { success = false, message = "Invalid data received." });
            }

            if (!model.RecipeIngredients.Any())
            {
                ModelState.AddModelError(nameof(model.RecipeIngredients), IngredientsRequiredErrorMessage);
                TempData[ErrorMessage] = "At least one ingredient is required";
                return BadRequest(new { success = false, errors = ModelState });
            }

            if (!model.Steps.Any())
            {
                ModelState.AddModelError(nameof(model.Steps), StepsRequiredErrorMessage);
                TempData[ErrorMessage] = "At least one cooking step is required";
                return BadRequest(new { success = false, errors = ModelState }); 
            }

            model.Id = model.Id.ToLower();
            model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;
            model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid model received!");

                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }

            bool exists = await this.recipeService.ExistsByIdAsync(model.Id);
            if (!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";
                return RedirectToAction("All", "Recipe");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByRecipeIdAsync(model.Id, userId);
            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to edit recipe info!";
                return RedirectToAction("Details", "Recipe", new { id = model.Id });
            }

            if (model.RecipeCategoryId != default(int))
            {
                bool categoryExists = await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId);
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(model.RecipeCategoryId), "Selected category does not exist!");
                }
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingredient!");
                }
                if (ingredient.MeasureId.HasValue && !await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), $"Invalid measure for ingredient {ingredient.Name}");
                }
                if (ingredient.SpecificationId.HasValue && !await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.SpecificationId), $"Invalid specification for ingredient {ingredient.Name}");
                }
            }

            if (!ModelState.IsValid)
            {
                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }

            // Sanitize all string input
            model.Title = sanitizer.SanitizeInput(model.Title);
            if (model.Description != null)
            {
                model.Description = sanitizer.SanitizeInput(model.Description);
            }

            if (model.Steps.Any())
            {
                foreach (var step in model.Steps)
                {
                    step.Description = sanitizer.SanitizeInput(step.Description);
                }
            }


            try
            {
                await this.recipeService.EditAsync(model);
                // Construct the redirect URL
                string recipeDetailsLink = Url.Action("Details", "Recipe", new { id = model.Id })!;

                // Return JSON response with redirect URL
                return Ok(new { success = true, redirectUrl = recipeDetailsLink });

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to update the house. Please try again later or contact administrator!");
                logger.LogError($"Recipe with Id {model.Id} unsuccessfully edited!");

                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (this.User.GetId() == string.Empty)
            {
                TempData[ErrorMessage] = "You need to be logged in to view Details";
                return RedirectToAction("Login", "User");
            }


            bool exists = await this.recipeService.ExistsByIdAsync(id);
            ViewData["Title"] = "Recipe Details";

            if (!exists)
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
                logger.LogError("Recipe Details unsuccessfully loaded!");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            string userId = this.User.GetId();
            ViewData["Title"] = "My Recipes";

            if (User.IsAdmin())
            {
                Redirect("/Admin/RecipeAdmin/Site");
            }

            try
            {
                RecipeMineViewModel model = new RecipeMineViewModel();
                model.FavouriteRecipes = await this.favouriteRecipeService.AllByUserIdAsync(userId);
                model.OwnedRecipes = await this.recipeService.AllAddedByUserAsync(userId);
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("My Recipes unsuccessfully loaded to View Model!");
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            string userId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByRecipeIdAsync(id, userId);

            if (!exists)
            {
                return NotFound();
            }

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to delete it!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            // Business Logic => if the Recipe is included in existing Meal Plans, a notification message should be shown before delete
            if (await this.recipeService.IsIncludedInMealPlans(id))
            {
                TempData[WarningMessage] = "Please note this recipe is included in existing Meal Plans!";
            }

            try
            {
                RecipeDeleteViewModel model = await this.recipeService.GetForDeleteByIdAsync(id);

                return View(model);
            }
            catch (Exception)
            {
                logger.LogError($"Delete of Recipe with id {id} unsuccessful");
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            string currentUserId = User.GetId();
            bool isOwner = await this.userService.IsOwnerByRecipeIdAsync(id, currentUserId);

            if (!exists)
            {
                logger.LogError($"Recipe with id {id} does not exist");
                return NotFound();
            }

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to delete it!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            try
            {
                await this.recipeService.DeleteByIdAsync(id);
                TempData[SuccessMessage] = "Recipe successfully deleted!";
            }
            catch (Exception)
            {
                logger.LogError($"Something went wrong and the recipe with id {id} was not deleted!");
                return BadRequest();
            }

            return RedirectToAction("All");
        }

        // private method for ingredient input validation
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
