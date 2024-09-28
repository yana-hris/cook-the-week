namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Models.Validation;


    [Authorize]
    public abstract class BaseController : Controller
    {
        
        public BaseController()
        {
            
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

    }
}
