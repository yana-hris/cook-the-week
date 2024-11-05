namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize]
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<BaseController> logger;

        public BaseController(ILogger<BaseController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Helper method for adding custom validation errors to the model state
        /// </summary>
        /// <param name="errors">A Dictionary of kvp errors</param>
        protected void AddCustomValidationErrorsToModelState(IDictionary<string,string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        /// <summary>
        /// Helper method for setting up ViewData and ViewBag values
        /// </summary>
        /// <param name="title">The View Title</param>
        /// <param name="returnUrl">The ReturnUrl</param>
        protected void SetViewData(string title, string returnUrl, string? backgroundClass = default, string? pageScrollClass = default)
        {
            ViewData["Title"] = title;
            ViewBag.ReturnUrl = returnUrl;

            if (backgroundClass != default)
            {
                ViewData["BackgroundClass"] = backgroundClass;
            }

            if (pageScrollClass != default)
            {
                ViewData["PageScrollClass"] = pageScrollClass;
            }
        }

    }
}
