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

        protected RedirectToActionResult RedirectToReturnUrl(object? model)
        {
            string returnUrl = TempData[ReturnUrl]!.ToString()!;
            string action = "";
            string controller = "";

            // Parse the returnUrl
            var uri = new Uri(returnUrl, UriKind.RelativeOrAbsolute);
            var segments = uri.Segments.Select(s => s.Trim('/')).ToArray();

            if (segments.Length > 0)
            {
                action = segments[segments.Length - 1];

                if (segments.Length > 1)
                {
                    controller = segments[segments.Length - 2];
                }

            }

            if (model != null)
            {
                TempData[Model] = JsonConvert.SerializeObject(model);
            }

            return RedirectToAction(action, controller);
        }

        protected string SanitizeInput(string input)
        {
            return sanitizer.Sanitize(input);
        }
    }
}
