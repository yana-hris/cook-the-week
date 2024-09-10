namespace CookTheWeek.Web.Controllers
{    
    using Ganss.Xss;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;


    [Authorize]
    public abstract class BaseController : Controller
    {
        private readonly HtmlSanitizer sanitizer;
        public BaseController()
        {
            sanitizer = new HtmlSanitizer();
        }    
        protected string SanitizeInput(string input)
        {
            return sanitizer.Sanitize(input);
        }
    }
}
