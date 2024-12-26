namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Rotativa.AspNetCore;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.ShoppingList;

    using static CookTheWeek.Common.NotificationMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using System.ClientModel.Primitives;

    public class ShoppingListController : BaseController
    {
        private readonly IShoppingListService shoppingListService;        
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IEmailSender emailSender;

        public ShoppingListController(IShoppingListService shoppingListService,
            ILogger<ShoppingListController> logger,
            IEmailSender emailSender,
            IWebHostEnvironment hostingEnvironment) : base(logger) 
        {
            this.shoppingListService = shoppingListService;
            this.hostingEnvironment = hostingEnvironment;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetShoppingList(string id, string? returnUrl = null)
        {
            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    ShoppingListViewModel model = await this.shoppingListService
                                    .TryGetShoppingListDataByMealPlanIdAsync(guidId);
                    SetViewData("Shopping List", returnUrl ?? "/MealPlan/Mine");
                    return View(model);
                }
                catch (RecordNotFoundException ex)
                {
                    TempData[ErrorMessage] = ex.Message;
                    return Redirect(returnUrl ?? "/MealPlan/Mine");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Shopping list generation failed. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                    return RedirectToAction("InternalServerError", "Home");
                }
            }

            return RedirectToAction("NotFound", "Home", new { message = "Invalid mealplan Id.", code = "400" });
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

            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    ShoppingListViewModel model = await this.shoppingListService.TryGetShoppingListDataByMealPlanIdAsync(guidId);
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

            return NotFound(new {message = "Invalid mealplan Id.", code = "400" });

        }

        [HttpPost]
        public async Task<IActionResult> EmailShoppingList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var errorMessage = $"Received mealplan ID from ajax request failed in action {nameof(GetShoppingListAsPdf)}. Id is null.";
                logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            if (id.TryToGuid(out Guid guidId))
            {
                try
                {
                    ShoppingListViewModel model = await this.shoppingListService.TryGetShoppingListDataByMealPlanIdAsync(guidId);

                    var result = await emailSender.SendShoppingListEmailAsync(model);

                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Shopping list email sent successfully for mealplan with ID: {id}.");
                        return Ok(new { success = true, message = "Shopping list emailed successfully!" });
                    }
                    else
                    {
                        var errorDetails = string.Join("; ", result.Errors.Select(e => $"{e.Key}: {e.Value}"));
                        logger.LogError($"Failed to send shopping list email for mealplan with ID: {id}. Errors: {errorDetails}");
                        return BadRequest(new { success = false, message = "Failed to send the email.", errors = result.Errors });
                    }
                }
                catch (RecordNotFoundException ex)
                {
                    TempData[ErrorMessage] = ex.Message;
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Shopping list email sending for mealplan with id: {id} failed. " +
                        $"Error Message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                    return BadRequest(ex);
                }
            }
            logger.LogWarning($"Invalid mealplan ID received: {id}");
            return NotFound(new { success = false, message = "Invalid mealplan ID.", code = "400" });

        }
    }
}
