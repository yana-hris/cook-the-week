namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.ActionFilters;
    using CookTheWeek.Web.ViewModels.Recipe;

    using static Common.EntityValidationConstants.RecipeValidation;
    using static Common.NotificationMessagesConstants;
    using CookTheWeek.Services.Data.Factories;

    public class RecipeController : BaseController
    {

        private readonly IRecipeViewModelFactory recipeViewModelFactory;
        private readonly IRecipeService recipeService;

        private readonly ILogger<RecipeController> logger;        

        public RecipeController(IRecipeService recipeService,
            IRecipeViewModelFactory recipeViewModelFactory,
            ILogger<RecipeController> logger)
        {
            this.recipeService = recipeService;
            this.recipeViewModelFactory = recipeViewModelFactory;

            this.logger = logger;            
        }


        [HttpGet]
        [AllowAnonymous]
        [AdminRedirect("Site", "RecipeAdmin")]
        public async Task<IActionResult> All([FromQuery] AllRecipesQueryModel queryModel)
        {
            try
            {
                var model = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel);

                SetViewData("All Recipes", Request.Path + Request.QueryString);
                return View(model);
            }
            catch (RecordNotFoundException)
            {
                return RedirectToAction(nameof(None));
            }
            catch(Exception ex)
            {
                return HandleException(ex, nameof(All), null);
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> Add(string returnUrl)
        {
            var model = await this.recipeViewModelFactory.CreateRecipeAddFormModelAsync();            

            SetViewData("Add Recipe", returnUrl ?? "/Recipe/All");
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
                var result = await recipeService.TryAddRecipeAsync(model!);

                if (result.Succeeded)
                {
                    string recipeId = result.Value;
                    TempData[SuccessMessage] = RecipeSuccessfullyAddedMessage;

                    return RedirectToAction(Url.Action("Details", "Recipe", new { id = recipeId, returnUrl }));
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return RedirectToAddViewWithModelErrors(model, returnUrl);
                }
            }
            catch (Exception ex) // Handle ArgumentNull, InvalidCast and all other exceptions but deliver message
            {
                return HandleException(ex, nameof(Add), null);
            }
            
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? "/Recipe/All";

            try
            {
                RecipeEditFormModel model = await this.recipeViewModelFactory.CreateRecipeEditFormModelAsync(id);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;

                return Redirect(returnUrl);
            }
            catch (UnauthorizedUserException ex)
            {
                TempData[ErrorMessage] = ex.Message;

                return RedirectToAction("Details", "Recipe", new { id, returnUrl });
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), id);
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
            string recipeDetailsLink = Url.Action("Details", "Recipe", new { id = model.Id, returnUrl = returnUrl ?? "/" })!;

            try
            {
                var result = await recipeService.TryEditRecipeAsync(model);

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
            try
            {
                RecipeDetailsViewModel model = await this.recipeViewModelFactory.CreateRecipeDetailsViewModelAsync(id);                
                SetViewData("Recipe Details", returnUrl ?? "/Recipe/All");
                return View(model);
            }
            catch (RecordNotFoundException ex) 
            {
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code = ex.ErrorCode});
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Details));
            }
        }


        [HttpGet]
        [AdminRedirect("Site", "RecipeAdmin")]
        public async Task<IActionResult> Mine()
        {
            SetViewData("My Recipes", Request.Path + Request.QueryString);

            try
            {
                var recipes = await recipeViewModelFactory.CreateRecipeMineViewModelAsync();
                return View(recipes);
            }
            catch (RecordNotFoundException)
            {
                return RedirectToAction(nameof(None));
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Mine), null);
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
            returnUrl = returnUrl ?? "/Recipe/Mine";

            try
            {
                await this.recipeService.DeleteByIdAsync(id);
                TempData[SuccessMessage] = "Recipe successfully deleted!";
                return Redirect(returnUrl);
            }
            catch (RecordNotFoundException ex)
            {
                return RedirectToAction("NotFound", "Home", new { message =  ex.Message, code = ex.ErrorCode});
            }
            catch (Exception ex) when (ex is UnauthorizedUserException || ex is InvalidOperationException)
            {
                TempData[WarningMessage] = ex.Message;
                return RedirectToAction("Details", "Recipe", new { id });
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(DeleteConfirmed), id);
            }            
        }

        // PRIVATE HELPER METHODS:

        

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

        /// <summary>
        /// Helper method to log error message and return a custom Internal Server Error page
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="actionName"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IActionResult HandleException(Exception ex, string actionName, string? recipeId = null)
        {
            var recipeIdInfo = recipeId != null ? $"Recipe ID: {recipeId}" : "No Recipe ID";
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}, {recipeIdInfo}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
        }
    }
}
