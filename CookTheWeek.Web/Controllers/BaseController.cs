namespace CookTheWeek.Web.Controllers
{    
    using Ganss.Xss;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Newtonsoft.Json;

    using static CookTheWeek.Common.GeneralApplicationConstants;

    [Authorize]
    public abstract class BaseController : Controller
    {
        private readonly HtmlSanitizer sanitizer;
        public BaseController()
        {
            sanitizer = new HtmlSanitizer();
        }        

        // For redirecting to a previously saved in TempData[ReturnUrl] link
        protected RedirectToActionResult RedirectToReturnUrl(object? model)
        {
            string returnUrl = "";
            string action = "";
            string controller = "";

            if (TempData.Peek(ReturnUrl) != null)
            {
                returnUrl = TempData[ReturnUrl] as string;
            }

            if(string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Error");
            }

            // Parse the returnUrl            
            var segments = returnUrl!.Split("/", StringSplitOptions.RemoveEmptyEntries).ToArray();

            if (segments.Length > 0)
            {
                action = segments[segments.Length - 1];

                if (segments.Length > 1)
                {
                    controller = segments[segments.Length - 2];
                }

            }

            // Pass the model in the TempData if any
            if (model != null)
            {
                TempData[ContactFormModelWithErrors] = JsonConvert.SerializeObject(model);
            }

            return RedirectToAction(action, controller);
        }

        protected string SanitizeInput(string input)
        {
            return sanitizer.Sanitize(input);
        }
    }
}
