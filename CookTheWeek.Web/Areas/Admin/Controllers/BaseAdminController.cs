namespace CookTheWeek.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    using static Common.GeneralApplicationConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public abstract class BaseAdminController : Controller
    {

        protected readonly ILogger<BaseAdminController> logger;

        protected BaseAdminController(ILogger<BaseAdminController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Adding custom validation errors to the modelstate
        /// </summary>
        /// <param name="validationResult"></param>
        protected void AddCustomValidationErrorsToModelState(IDictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        /// <summary>
        /// Helper method to log error message and return a custom Internal Server Error page
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="actionName"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected IActionResult HandleException(Exception ex, string actionName, string entityName, string? id)
        {
            var entityInfo = entityName != null ? $", Entity: {entityName}" : "";
            var idInfo = id != null ? $"$ with id: {id}" : "";
            logger.LogError($"Unexpected error occurred while processing the request. Action: {actionName}{entityInfo}{idInfo}. Error message: {ex.Message}. StackTrace: {ex.StackTrace}");

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });
        }
    }

   
}
