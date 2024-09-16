namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    
    using Infrastructure.Extensions;
    using Services.Data.Factories.Interfaces;
    using Services.Data.Interfaces;
    using ViewModels.Recipe;
    using ViewModels.RecipeIngredient;
    using ViewModels.Step;

    using static Common.EntityValidationConstants.Recipe;
    using static Common.EntityValidationConstants.RecipeIngredient;
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    public class RecipeController : BaseController
    {

        private readonly IRecipeViewModelFactory recipeViewModelFactory;
        private readonly ILogger<RecipeController> logger;

        // TODO: use factories instead
        private readonly IRecipeService recipeService;
        private readonly IIngredientService ingredientService;
        private readonly ICategoryService categoryService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IUserService userService;
        private readonly IFavouriteRecipeService favouriteRecipeService;

        public RecipeController(IRecipeService recipeService,
            ICategoryService categoryService,
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService,
            IUserService userService,
            IFavouriteRecipeService favouriteRecipeService,
            IRecipeViewModelFactory recipeViewModelFactory,
            ILogger<RecipeController> logger)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientService = ingredientService;
            this.userService = userService;
            this.favouriteRecipeService = favouriteRecipeService;
            this.recipeViewModelFactory = recipeViewModelFactory;
            this.logger = logger;            
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllRecipesQueryModel queryModel)
        {

            string userId = User.GetId();
            bool isAdmin = User.IsAdmin();

            if (isAdmin)
            {
                return RedirectToAction("Site", "RecipeAdmin", new { area = "Admin" });
            }

            try
            {
                var model = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, userId);

                SetViewData("All Recipes", Request.Path + Request.QueryString);

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the All Recipes query");
                return NotFound();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add(string returnUrl)
        {
            RecipeAddFormModel model = new RecipeAddFormModel();
            await PopulateModelDataAsync(model);

            if (returnUrl == null)
            {
                if (User.IsAdmin())
                {
                    returnUrl = "/Admin/HomeAdmin/Index";
                }
                else
                {
                    returnUrl = "/Recipe/All";
                }
            }

            ViewBag.ReturnUrl = returnUrl; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeAddFormModel model, string returnUrl = null)
        {
            await PopulateModelDataAsync(model);
            await ValidateCategoryAsync(model);
            await ValidateIngredientsAsync(model);

            if (!ModelState.IsValid)
            {
                StoreServerErrorsInTempData();
                ViewBag.ReturnUrl = returnUrl;
                // For the back btn to work
                return View(model);
            }
            

            try
            {
                string ownerId = User.GetId();
                bool isAdmin = User.IsAdmin();
                string recipeId = await this.recipeService.AddAsync(model, ownerId, isAdmin);
                TempData[SuccessMessage] = RecipeSuccessfullySavedMessage;
                return RedirectToAction("Details", "Recipe", new { id = recipeId, returnUrl });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Reicpe was not added!");
                return BadRequest();
            }
        }

        

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl = null)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            if (!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";
                logger.LogWarning($"Recipe with id {id} does not exist in database!");
                return Redirect(returnUrl ?? "/Recipe/All");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsRecipeOwnerByIdAsync(id, userId);

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to edit recipe info!";
                logger.LogWarning("The user id of the recipe owner and current user do not match!");
                return RedirectToAction("Details", "Recipe", new { id, returnUrl });
            }

            try
            {
                RecipeEditFormModel model = await this.recipeService.GetForEditByIdAsync(id);
                model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
                model.ServingsOptions = ServingsOptions;
                model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
                model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();
                ViewBag.ReturnUrl = returnUrl; 
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
        public async Task<IActionResult> Edit([FromBody] RecipeEditFormModel model, [FromQuery] string returnUrl)
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
                TempData[ErrorMessage] = RecipeNotFoundErrorMessage;
                return Redirect(returnUrl ?? "/Recipe/All");
            }

            string userId = User.GetId();
            bool isOwner = await this.userService.IsRecipeOwnerByIdAsync(model.Id, userId);
            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = RecipeOwnerErrorMessage;
                return RedirectToAction("Details", "Recipe", new { id = model.Id, returnUrl });
            }

            if (model.RecipeCategoryId.HasValue && model.RecipeCategoryId.Value != default)
            {
                bool categoryExists = await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId.Value);
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(model.RecipeCategoryId), RecipeCategoryIdInvalidErrorMessage);
                }
            }

            foreach (var ingredient in model.RecipeIngredients)
            {
                if (!await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), RecipeIngredientInvalidErrorMessage);
                }
                if (ingredient.MeasureId.HasValue && !await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), MeasureRangeErrorMessage);
                }
                if (ingredient.SpecificationId.HasValue && !await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.SpecificationId), SpecificationRangeErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }
            

            try
            {
                await this.recipeService.EditAsync(model);
                string recipeDetailsLink = Url.Action("Details", "Recipe", new { id = model.Id, returnUrl })!;

                // Return JSON response with redirect URL
                return Ok(new { success = true, redirectUrl = recipeDetailsLink });

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, StatusCode500InternalServerErrorMessage);
                logger.LogError($"Recipe with Id {model.Id} unsuccessfully edited!");

                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id, string returnUrl = null)
        {
            string userId = User.GetId();

            if (userId == string.Empty)
            {
                TempData[ErrorMessage] = "You need to be logged in to view Details";
                returnUrl = Url.Action("Details", "Recipe", new { id = id });
                return RedirectToAction("Login", "User", new {returnUrl});
            }

            bool exists = await this.recipeService.ExistsByIdAsync(id);
            ViewData["Title"] = "Recipe Details";           

            if (!exists)
            {
                TempData[ErrorMessage] = "Recipe with the provided id does not exist!";
                return Redirect(returnUrl ?? "/Recipe/All");
            }

            try
            {
                RecipeDetailsViewModel model = await this.recipeService.DetailsByIdAsync(id, userId);
                ViewBag.ReturnUrl = returnUrl;
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

                if(!model.OwnedRecipes.Any() && !model.FavouriteRecipes.Any())
                {
                    return RedirectToAction("None");
                }
                ViewBag.ReturnUrl = Request.Path + Request.QueryString;
                return View(model);
            }
            catch (Exception)
            {
                logger.LogError("My Recipes unsuccessfully loaded to View Model!");
                return BadRequest();
            }

        }

        [HttpGet]
        public IActionResult None()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(string id, string? returnUrl)
        {
            bool exists = await this.recipeService.ExistsByIdAsync(id);

            string currentUserId = User.GetId();
            bool isOwner = await this.userService.IsRecipeOwnerByIdAsync(id, currentUserId);

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

            // Business Logic => if the Recipe is included in existing Meal Plans, a notification message should be shown before delete
            if (await this.recipeService.IsIncludedInMealPlans(id))
            {
                TempData[WarningMessage] = "Please note this recipe is included in existing Meal Plans!";
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

            return Redirect(returnUrl ?? "/Recipe/Mine");
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

        private async Task PopulateModelDataAsync(RecipeAddFormModel model)
        {
            model.Categories = await categoryService.AllRecipeCategoriesAsync();
            model.ServingsOptions = ServingsOptions;

            await EnsureIngredientsAndStepsExist(model);

            var firstIngredient = model.RecipeIngredients.First();
            firstIngredient.Measures = await recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            firstIngredient.Specifications = await recipeIngredientService.GetRecipeIngredientSpecificationsAsync();
                       
        }
        private async Task EnsureIngredientsAndStepsExist(RecipeAddFormModel model)
        {
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
        }

        private async Task ValidateCategoryAsync(RecipeAddFormModel model)
        {
            if (model.RecipeCategoryId.HasValue && model.RecipeCategoryId != default)
            {
                bool categoryExists = await this.categoryService.RecipeCategoryExistsByIdAsync(model.RecipeCategoryId.Value);
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(model.RecipeCategoryId), RecipeCategoryIdInvalidErrorMessage);
                }
            }
        }

        private async Task ValidateIngredientsAsync(RecipeAddFormModel model)
        {
            foreach (var ingredient in model.RecipeIngredients)
            {
                if (ingredient.Name != default && !await IsIngredientValid(ingredient.Name))
                {
                    ModelState.AddModelError(nameof(ingredient.Name), "Invalid ingridient!");
                }
                if (ingredient.MeasureId.HasValue && !await this.recipeIngredientService.IngredientMeasureExistsAsync(ingredient.MeasureId!.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.MeasureId), MeasureRangeErrorMessage);
                }
                if (ingredient.SpecificationId.HasValue && !await this.recipeIngredientService.IngredientSpecificationExistsAsync(ingredient.SpecificationId.Value))
                {
                    ModelState.AddModelError(nameof(ingredient.SpecificationId), SpecificationRangeErrorMessage);
                }
            }
        }

        private void StoreServerErrorsInTempData()
        {
            // Collect server-side validation errors
            var serverErrors = ModelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            // Serialize the errors to JSON and store them in TempData
            TempData["ServerErrors"] = JsonConvert.SerializeObject(serverErrors);
        }

        // Helper method for setting up ViewData/ViewBag
        private void SetViewData(string title, string returnUrl)
        {
            ViewData["Title"] = title;
            ViewBag.ReturnUrl = returnUrl;
        }
    }
}
