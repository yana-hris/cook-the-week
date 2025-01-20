namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Events.Dispatchers;
    using CookTheWeek.Services.Data.Events;
    using CookTheWeek.Services.Data.Factories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.ActionFilters;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.Recipe;

    using static Common.EntityValidationConstants.RecipeValidation;
    using static Common.NotificationMessagesConstants;
    using CookTheWeek.Data.Models;

    public class RecipeController : BaseController
    {

        private readonly IRecipeViewModelFactory recipeViewModelFactory;
        private readonly IRecipeService recipeService;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public RecipeController(IRecipeService recipeService,
            IRecipeViewModelFactory recipeViewModelFactory,
            IDomainEventDispatcher domainEventDispatcher,
            ILogger<RecipeController> logger) : base(logger)
        {
            this.recipeService = recipeService;
            this.recipeViewModelFactory = recipeViewModelFactory;
            this.domainEventDispatcher = domainEventDispatcher;
        }


        [AllowAnonymous]
        [AdminRedirect("All", "RecipeAdmin")]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllRecipesQueryModel queryModel)
        {
            var model = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                bool justLoggedIn = TempData.Peek(TempDataConstants.JustLoggedIn) as bool? ?? false;
                model = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, justLoggedIn);

                SetViewData("All Recipes", Request.Path + Request.QueryString);
                return View(model);
            }
            catch(Exception ex)
            {
                return HandleException(ex, nameof(All), null);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> QuickDinners([FromQuery] AllRecipesQueryModel queryModel)
        {
            var quickDinnersModel = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                int mealType = 4; // "Main Dish"
                int tag = 9; //"Quick" 
                int maxCookingTime = 30;

                queryModel.MealTypeId = mealType;
                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }

                queryModel.SelectedTagIds.Add(tag);
                queryModel.MaxPreparationTime = maxCookingTime;

                quickDinnersModel = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Quick Dinners", Request.Path + Request.QueryString);
                return View(quickDinnersModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(QuickDinners), null);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> KidsFriendly([FromQuery] AllRecipesQueryModel queryModel)
        {
            var kidsFriendlyModel = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                int tag = 13; //"Kid-friendly" 
                
                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }

                queryModel.SelectedTagIds.Add(tag);

                kidsFriendlyModel = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Kids Friendly", Request.Path + Request.QueryString);
                return View(kidsFriendlyModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(KidsFriendly), null);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Vegetarian([FromQuery] AllRecipesQueryModel queryModel)
        {
            var vegetarianModel = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                int tag = 2; //"Vegetarian" 

                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }

                queryModel.SelectedTagIds.Add(tag);

                vegetarianModel = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Vegetarian", Request.Path + Request.QueryString);
                return View(vegetarianModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Vegetarian), null);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GlutenFree([FromQuery] AllRecipesQueryModel queryModel)
        {
            var glutenFreeModel = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                int tag = 4; //"Gluten-free" 

                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }

                queryModel.SelectedTagIds.Add(tag);

                glutenFreeModel = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Gluten Free", Request.Path + Request.QueryString);
                return View(glutenFreeModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(GlutenFree), null);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DiaryFree([FromQuery] AllRecipesQueryModel queryModel)
        {
            var diaryFreeModel = new AllRecipesFilteredAndPagedViewModel();

            try
            {
                int tag = 5; //"Diary-free" 

                if (queryModel.SelectedTagIds == null)
                {
                    queryModel.SelectedTagIds = new List<int>();
                }

                queryModel.SelectedTagIds.Add(tag);

                diaryFreeModel = await this.recipeViewModelFactory.CreateAllRecipesViewModelAsync(queryModel, false);

                SetViewData("Diary Free", Request.Path + Request.QueryString);
                return View(diaryFreeModel);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(DiaryFree), null);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Add(string? returnUrl = null)
        {
            try
            {
                var model = await this.recipeViewModelFactory.CreateRecipeAddFormModelAsync();

                SetViewData("Add Recipe", AdjustReturnUrl(returnUrl), "image-overlay food-background");
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Loading Recipe Add View failed.");
                return HandleException(ex, nameof(Add), null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeAddFormModel model, string? returnUrl = null)
        {
            returnUrl = AdjustReturnUrl(returnUrl);

            if (model == null)
            {
                return HandleException(new ArgumentNullException(nameof(model)), nameof(Add));
            }

            try
            {
                model = (RecipeAddFormModel) await recipeViewModelFactory.PopulateRecipeFormModelAsync(model);
                
                if (model == null)
                {
                    return HandleException(new InvalidOperationException("Failed to populate RecipeAddFormModel."), nameof(Add));
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Add));
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAddViewWithModelErrors(model!, returnUrl);
            }

            try
            {
                var result = await recipeService.TryAddRecipeAsync(model!);

                if (result.Succeeded && result.Value != default)
                {
                    string recipeId = result.Value;
                    TempData[SuccessMessage] = RecipeSuccessfullyAddedMessage;
                    await DispatchRecipeImageUpdateEvent(Guid.Parse(recipeId), model.ImageUrl);

                    return Redirect(Url.Action("Details", "Recipe", new { id = recipeId, returnUrl = returnUrl }));
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return RedirectToAddViewWithModelErrors(model, returnUrl);
                }
            }
            catch (Exception ex) 
            {
                return HandleException(ex, nameof(Add), null);
            }
            
        }

        

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? returnUrl = null)
        {
            returnUrl = AdjustReturnUrl(returnUrl);

            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    RecipeEditFormModel model = await this.recipeViewModelFactory.CreateRecipeEditFormModelAsync(guidId);
                    SetViewData("Edit Recipe", returnUrl, "image-overlay food-background");
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
                    return HandleException(ex, nameof(Edit), guidId);
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid Recipe Id.", code = "400" });

        }

       
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] RecipeEditFormModel model, [FromQuery] string returnUrl)
        {
            if (model == null)
            {
                logger.LogError("Unsuccessful model binding from ko.JSON to RecipeEditFormModel");
                TempData[ErrorMessage] = "Error: please try again!";
                return BadRequest(new { success = false});
            }

            try
            {
                model = (RecipeEditFormModel)await recipeViewModelFactory.PopulateRecipeFormModelAsync(model);
            }
            catch (Exception ex)
            {
                return HandleException(ex, nameof(Edit), model.Id);
            }

            if (!ModelState.IsValid)
            {
                return ReturnBadRequestWithSerializedModelErrors();
            }
            string recipeDetailsLink = Url.Action("Details", "Recipe", new { id = model.Id, returnUrl = AdjustReturnUrl(returnUrl) })!;

            try
            {
                var result = await recipeService.TryEditRecipeAsync(model);

                if (result.Succeeded)
                {
                    await DispatchRecipeImageUpdateEvent(model.Id, model.ImageUrl);
                    return Ok(new { success = true, redirectUrl = recipeDetailsLink });
                }
                else
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return ReturnBadRequestWithSerializedModelErrors();
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
            returnUrl = AdjustReturnUrl(returnUrl);

            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    RecipeDetailsViewModel model = await this.recipeViewModelFactory.CreateRecipeDetailsViewModelAsync(guidId);
                    
                    SetViewData("Recipe Details", returnUrl, "image-overlay food-background");
                    return View(model);
                }
                catch (RecordNotFoundException ex)
                {
                    return RedirectToAction("NotFound", "Home", new { message = ex.Message, code = ex.ErrorCode });
                }
                catch (Exception ex)
                {
                    return HandleException(ex, nameof(Details));
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid Recipe Id.", code = "400" });

        }

        [HttpGet]
        public IActionResult None(string? returnUrl = null)
        {
            returnUrl = AdjustReturnUrl(returnUrl);
            SetViewData("None", returnUrl, "image-overlay food-background");
            return View();
        }


        [HttpGet]
        [AdminRedirect("All", "RecipeAdmin")]
        public async Task<IActionResult> Mine(string? returnUrl = null)
        {
            returnUrl = AdjustReturnUrl(returnUrl ?? Request.Path + Request.QueryString);
            SetViewData("My Recipes", returnUrl);

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
        public async Task<IActionResult> DeleteConfirmed(string id, string? returnUrl)
        {
            returnUrl = AdjustReturnUrl(returnUrl, "Details");

            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    await this.recipeService.DeleteByIdAsync(guidId);
                    TempData[SuccessMessage] = "Recipe successfully deleted!";

                    // Dispatch the soft delete event
                    var recipeSoftDeletedEvent = new RecipeSoftDeletedEvent(guidId);
                    await domainEventDispatcher.DispatchAsync(recipeSoftDeletedEvent);

                    return Redirect(returnUrl);
                }
                catch (RecordNotFoundException ex)
                {
                    return RedirectToAction("NotFound", "Home", new { message = ex.Message, code = ex.ErrorCode });
                }
                catch(UnauthorizedUserException ex)
                {
                    TempData[ErrorMessage] = ex.Message;
                    return Redirect(returnUrl);
                }
                catch (InvalidOperationException)
                {
                    TempData[ErrorMessage] = "This Recipe cannot be deleted as it is included in active Meal Plans";
                    return Redirect(returnUrl);
                }
                catch (Exception ex)
                {
                    return HandleException(ex, nameof(DeleteConfirmed), guidId);
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid Recipe Id.", code = "400" });

        }

        // PRIVATE HELPER METHODS:

        

        /// <summary>
        /// Helper method for the case when model errors have to be shown to the user in Add view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToAddViewWithModelErrors(RecipeAddFormModel model, string? returnUrl = null)
        {
            StoreServerErrorsInTempData();
            SetViewData("Add Recipe", AdjustReturnUrl(returnUrl), "image-overlay food-background");
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
        /// Transforms the model state errors into an object, expected to be received from the AJAX request callback function in case of Bad Request server response
        /// </summary>
        /// <returns>Returns BadRequest response as Json, containing success = false and errors = Modelstate errors</returns>
        private IActionResult ReturnBadRequestWithSerializedModelErrors()
        {
            var filteredErrors = ModelState
                                    .Where(kvp => kvp.Key != "returnUrl") // Exclude `returnUrl`
                                    .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => new
                                        {
                                            errors = kvp.Value.Errors.Select(e => new { errorMessage = e.ErrorMessage }).ToList()
                                        }
                                    );

            return BadRequest(new { success = false, errors = filteredErrors });
        }

        /// <summary>
        /// Helper method to log error message and return a custom Internal Server Error page
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="actionName"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IActionResult HandleException(Exception ex, string actionName, Guid? recipeId = null)
        {
            // Log the full exception details for internal reference
            var recipeIdInfo = recipeId != default ? $"Recipe ID: {recipeId.ToString()}" : "No Recipe ID";
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}, {recipeIdInfo}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

            // Redirect to a custom error page with a generic error message
            string userFriendlyMessage = "An unexpected error occurred. Please try again later.";
            return RedirectToAction("InternalServerError", "Home", new { message = userFriendlyMessage });
        }

        /// <summary>
        /// Helper method to check if the returnUrl is not a previous Add / Edit action or Details action (before Delete) which may cause form resubmit or a page not found issue
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="fallbackUrl"></param>
        /// <returns>the transformed returnUrl</returns>
        private string AdjustReturnUrl(string? returnUrl, string extraUrl = null)
        {
            // Define restricted actions
            var restrictedActions = new List<string> { "Add", "Edit" };

            // Add extra restricted action if any
            if (extraUrl != null && !string.IsNullOrEmpty(extraUrl))
            {
                restrictedActions.Add(extraUrl);
            }

            // Always the same
            string fallbackUrl = "/Recipe/All";

            // Validate returnUrl
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                // Check if the returnUrl contains any restricted actions
                foreach (var action in restrictedActions)
                {
                    if (returnUrl.Contains(action, StringComparison.OrdinalIgnoreCase))
                    {
                        // Redirect to fallback URL if restricted action is found
                        return fallbackUrl;
                    }
                }

                // If no restricted actions, return the original returnUrl
                return returnUrl;
            }

            // Return fallback URL if returnUrl is invalid
            return fallbackUrl;
        }

        /// <summary>
        /// Private method that dispatches an event to trigger Cloudinary link generation after every recipe modification
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        private async Task DispatchRecipeImageUpdateEvent(Guid id, string imageUrl)
        {
            var recipeCreatedEvent = new RecipeImageUpdateEvent
            {
                RecipeId = id,
                ExternalImageUrl = imageUrl
            };

            await domainEventDispatcher.DispatchAsync<RecipeImageUpdateEvent>(recipeCreatedEvent);
        }
    }
}
