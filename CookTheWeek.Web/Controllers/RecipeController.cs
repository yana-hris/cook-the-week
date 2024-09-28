namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Factories.Interfaces;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.Recipe;

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
        private readonly IValidationService validationService;

        public RecipeController(IRecipeService recipeService,
            ICategoryService categoryService,
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService,
            IUserService userService,
            IRecipeViewModelFactory recipeViewModelFactory,
            IValidationService validationService,
            ILogger<RecipeController> logger)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.recipeIngredientService = recipeIngredientService;
            this.ingredientService = ingredientService;
            this.userService = userService;
            this.recipeViewModelFactory = recipeViewModelFactory;
            this.validationService = validationService;
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
                return Redirect("None"); // In case of no recipes found redirect to "None" view
            }
            catch(Exception ex)
            {
                logger.LogError($"An unexpected error occured. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
            }

        }

        
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
        public async Task<IActionResult> Add(RecipeAddFormModel model, string? returnUrl)
        {
            model = await this.recipeViewModelFactory.PreloadRecipeSelectOptionsToFormModel(model) as RecipeAddFormModel;
            string redirectUrl;

            if (!ModelState.IsValid)
            {
                return RedirectToAddViewWithModelErrors(model!, returnUrl);
            }

            try
            {
                string ownerId = User.GetId();
                bool isAdmin = User.IsAdmin();
                var result = await this.recipeService.TryAddRecipeAsync(model!, ownerId, isAdmin);

                if (result.Succeeded)
                {
                    string recipeId = result.Value;
                    TempData[SuccessMessage] = RecipeSuccessfullyAddedMessage;

                    return RedirectToAction(Url.Action("Details", "Recipe", new { id = recipeId, returnUrl = returnUrl }));
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return RedirectToAddViewWithModelErrors(model, returnUrl);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occured while adding recipe. Error message: {ex.Message}, error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
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

                return Redirect(returnUrl ?? "/Recipe/All");
            }
            catch (UnauthorizedUserException ex)
            {
                TempData[ErrorMessage] = ex.Message;

                return RedirectToAction("Details", "Recipe", new { id, returnUrl });
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = "An unexpected error occurred while processing your request.";
                logger.LogError($"Unexpected error occurred while processing the request. Action: {nameof(Edit)}, Recipe ID: {id}, User ID: {userId}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

                return RedirectToAction("InternalServerError", "Home");
            }
        }

        // TODO: Check how to add CSFR token in hidden input in view
        // TODO: Check if it works as Json result not testes yet!
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Edit([FromBody] RecipeEditFormModel model, [FromQuery] string returnUrl)
        {
            if (model == null)
            {
                logger.LogError("Unsuccessful model binding from ko.JSON to RecipeEditFormModel");
                TempData[ErrorMessage] = "Error: please try again!";
                return BadRequest(new { success = false});
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }
            string recipeDetailsLink = Url.Action("Details", "Recipe", new { id = model.Id, returnUrl })!;

            try
            {
                var result = await this.recipeService.TryEditRecipeAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new { success = true, redirectUrl = recipeDetailsLink });
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return BadRequest(new { success = false, errors = ModelState });
                }
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                string myRecipesLink = Url.Action("Mine", "Recipe", new {area = ""})!;
                return BadRequest(new { success = false, redirectUrl = myRecipesLink });
            }
            catch (UnauthorizedUserException ex)
            {
                TempData[ErrorMessage] = ex.Message;                
                return BadRequest(new { success = false, redirectUrl = recipeDetailsLink });
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occured while adding recipe. Error message: {ex.Message}, error Stacktrace: {ex.StackTrace}");
                return StatusCode(500, new { success = false, message = "An internal server error occurred." });
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
            bool isAdmin = User.IsAdmin();

            try
            {
                await this.recipeService.DeleteByIdAsync(id, userId, isAdmin);
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
            catch(InvalidOperationException ex)
            {
                TempData[WarningMessage] = ex.Message; 
                return RedirectToAction("Details", "Recipe", new { id });
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong and the recipe with id {id} was not deleted!");
                return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
            }
        }

        // private method for ingredient input validation
        // TODO: check if it is needed
        private async Task<bool> IsIngredientValid(string ingredientName)
        {
            if (!string.IsNullOrEmpty(ingredientName))
            {
                return await this.ingredientService.ExistsByNameAsync(ingredientName);
            }
            return false;
        }
        
       

        /// <summary>
        /// Helper method for setting up ViewData and ViewBag values
        /// </summary>
        /// <param name="title">The View Title</param>
        /// <param name="returnUrl">The ReturnUrl</param>
        private void SetViewData(string title, string returnUrl)
        {
            ViewData["Title"] = title;
            ViewBag.ReturnUrl = returnUrl;
        }

        /// <summary>
        /// Helper method for the case when model errors have to be shown to the user in Add view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToAddViewWithModelErrors(RecipeAddFormModel model, string? returnUrl)
        {
            StoreServerErrorsInTempData();
            SetViewData("Add Recipe", returnUrl ?? "Home/Index");
            return View(model);
        }

        /// <summary>
        /// Helper method for setting server errors for Recipe Add View
        /// </summary>
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
    }
}
