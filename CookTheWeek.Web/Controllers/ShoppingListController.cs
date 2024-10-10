namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Rotativa.AspNetCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    using static CookTheWeek.Common.NotificationMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;

    public class ShoppingListController : BaseController
    {
        private readonly IShoppingListService shoppingListService;        
        private readonly IWebHostEnvironment hostingEnvironment;

        public ShoppingListController(IShoppingListService shoppingListService,
            ILogger<ShoppingListController> logger,
            IWebHostEnvironment hostingEnvironment) : base(logger) 
        {
            this.shoppingListService = shoppingListService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingList(string id, string returnUrl = null)
        {
            try
            {
                ShoppingListViewModel model = await this.shoppingListService
                .TryGetShoppingListDataByMealPlanIdAsync(id);
                ViewBag.ReturnUrl = returnUrl;

                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                return Redirect(returnUrl ?? "/MealPlan/Mine");
            }
            catch(Exception ex)
            {
                logger.LogError($"Shopping list generation failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingListAsPdf(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorMessage = $"Received mealplan ID from ajax request failed in action {nameof(GetShoppingListAsPdf)}. Id is null.";
                logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            try
            {
                ShoppingListViewModel model = await this.shoppingListService.TryGetShoppingListDataByMealPlanIdAsync(id);
                string contentRootPath = hostingEnvironment.ContentRootPath;
                string viewName = nameof(GetShoppingListAsPdf);

                // Render the partial view to HTML
                return new ViewAsPdf(viewName, model)
                {
                    FileName = $"Shopping_list_{model.Title}_{DateTime.Today.ToString("dd-MM-yyyy")}.pdf",
                    CustomSwitches = "--print-media-type",
                    PageWidth = DefaultPdfPageWidth,
                    PageHeight = DefaultPdfPageHeight
                };
            }
            catch (RecordNotFoundException ex)
            {
                TempData[ErrorMessage] = ex.Message;
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Shopping list generation as pdf for mealplan with id: {id} failed. " +
                    $"Error Message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return BadRequest(ex);
            }

        }
    }
}
