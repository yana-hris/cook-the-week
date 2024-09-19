namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    
    using Common.Exceptions;
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
            catch (RecordNotFoundException)
            {
                return Redirect("None");
            }
            catch(DataRetrievalException ex)
            {
                return RedirectToAction("InternalServerError", "Home", new {message = ex.Message, code = ex.ErrorCode});
            }

        }


        // TODO: add exception-handling
        [HttpGet]
        public async Task<IActionResult> Add(string returnUrl)
        {
            var model = await this.recipeViewModelFactory.CreateRecipeAddFormModelAsync();

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

            SetViewData("Add Recipe", returnUrl);
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
                SetViewData("Add Recipe", returnUrl);
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
            string userId = User.GetId();
            bool isAdmin = User.IsAdmin();

            try
            {
                RecipeEditFormModel model = await this.recipeViewModelFactory.CreateRecipeEditFormModelAsync(id, userId, isAdmin);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                logger.LogWarning($"Recipe with id {id} does not exist in database!");
                return Redirect(returnUrl ?? "/Recipe/All");
            }
            catch (UnauthorizedUserException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                logger.LogWarning($"Unauthorized user with id: {userId} tried to edit a recipe with id: {id}");
                return RedirectToAction("Details", "Recipe", new { id, returnUrl });
            }
            catch (DataRetrievalException ex)
            {
                logger.LogError($"Internal Server error while retrieving RecipeEditFormModel for recipe with id: {id}");
                return RedirectToAction("InternalServerError", "Home", new { message = ex.Message, code = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                logger.LogError($"Internal Server error while retrieving RecipeEditFormModel for recipe with id: {id}. Error StackTrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Edit([FromBody] RecipeEditFormModel model, [FromQuery] string returnUrl)
        {
            if (model == null)
            {
                logger.LogError("Unsuccessful model binding from ko.JSON to RecipeEditFormModel");
                TempData[ErrorMessage] = "Error: please try again!";
                return RedirectToAction("Edit");
            }

            // TODO: check if this is really needed
            //if (!model.RecipeIngredients.Any())
            //{
            //    ModelState.AddModelError(nameof(model.RecipeIngredients), IngredientsRequiredErrorMessage);
            //    TempData[ErrorMessage] = "At least one ingredient is required";
            //    return BadRequest(new { success = false, errors = ModelState });
            //}

            //if (!model.Steps.Any())
            //{
            //    ModelState.AddModelError(nameof(model.Steps), StepsRequiredErrorMessage);
            //    TempData[ErrorMessage] = "At least one cooking step is required";
            //    return BadRequest(new { success = false, errors = ModelState }); 
            //}

            //model.Id = model.Id.ToLower();
            //model.Categories = await this.categoryService.AllRecipeCategoriesAsync();
            //model.ServingsOptions = ServingsOptions;
            //model.RecipeIngredients.First().Measures = await this.recipeIngredientService.GetRecipeIngredientMeasuresAsync();
            //model.RecipeIngredients.First().Specifications = await this.recipeIngredientService.GetRecipeIngredientSpecificationsAsync();

            if (!ModelState.IsValid)
            {
                logger.LogWarning("Invalid model received!");

                // Return the view with the model and validation errors
                return BadRequest(new { success = false, errors = ModelState });
            }

            try
            {
                await this.recipeService.EditAsync(model);
            }
            catch (RecordNotFoundException)
            {
                TempData[ErrorMessage] = RecipeNotFoundErrorMessage;
                return Redirect(returnUrl ?? "/Recipe/All");
            }
            catch (UnauthorizedUserException)
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
            catch(RecordNotFoundException ex)
            {
                return NotFound(ex);
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
        public async Task<IActionResult> Details(string id, string returnUrl = null)
        {
            string userId = User.GetId();

            try
            {
                RecipeDetailsViewModel model = await this.recipeViewModelFactory.CreateRecipeDetailsViewModelAsync(id, userId);                
                SetViewData("Recipe Details", returnUrl);
                return View(model);
            }
            catch (RecordNotFoundException ex) 
            {
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code = ex.ErrorCode});
            }
            catch (DataRetrievalException ex)
            {
                return RedirectToAction("InternalServerError", "Home", new {message = ex.Message, code = ex.ErrorCode});
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            if (User.IsAdmin())
            {
                Redirect("/Admin/RecipeAdmin/Site");
            }

            string userId = this.User.GetId();
            SetViewData("My Recipes", Request.Path + Request.QueryString);

            try
            {
                var recipes = await recipeViewModelFactory.CreateRecipeMineViewModelAsync(userId);
                return View(recipes);
            }
            catch (RecordNotFoundException)
            {
                return RedirectToAction("None");
            }
            catch (DataRetrievalException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.GetFormattedMessage());
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
            string userId = User.GetId();

            try
            {
                await this.recipeService.DeleteByIdAsync(id, userId);
                TempData[SuccessMessage] = "Recipe successfully deleted!";
                return Redirect(returnUrl ?? "/Recipe/Mine");
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"Recipe with id {id} does not exist");
                return RedirectToAction("NotFound", "Home", new { message =  ex.Message, code = ex.ErrorCode});
            }
            catch(UnauthorizedUserException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                return RedirectToAction("Details", "Recipe", new { id });
            }
            catch(InvalidOperationException ex) // TODO: in case is included in meal plans
            {
                TempData[WarningMessage] = ex.Message; // and procede!
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong and the recipe with id {id} was not deleted!");
                return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
            }


            // TODO: check for admin in service!

            if (!isOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the owner of the recipe to delete it!";
                return RedirectToAction("Details", "Recipe", new { id });
            }

            
            
            

            
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
