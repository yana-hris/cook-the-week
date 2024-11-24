namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.ActionFilters;
    using CookTheWeek.Web.ViewModels.Home;
    
    using static Common.NotificationMessagesConstants;

    [AllowAnonymous]
    public class HomeController : BaseController
    { 
        private readonly IEmailSender emailSender;
        private readonly Guid userId;

        public HomeController(IEmailSender emailSender,
                              IUserContext userContext,
                              ILogger<HomeController> logger) 
        : base(logger)
        {
            this.emailSender = emailSender;
            this.userId = userContext.UserId;
        }

        [HttpGet]
        [AdminRedirect("Index", "HomeAdmin")]
        public IActionResult Index()
        {
            if (userId != default)
            {
                return RedirectToAction("All", "Recipe");
            }
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
                
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TermsOfUse()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CookiePolicy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HowItWorks()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ContactFormModel model = new ContactFormModel();
            SetViewData("Contact us", null, "image-overlay food-background");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            SetViewData("Contact us", null, "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
           
            var result = await emailSender.SendContactFormEmailAsync(model);

            if (result.Succeeded)
            {
                TempData[SuccessMessage] = SuccessfulEmailSentMessage;

                // TODO: change to Confirmation view when available
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData[ErrorMessage] = UnsuccessfulEmailSentMessage;

                // TODO: change to Confirmation Failed view when available
                return RedirectToAction("Contact", "Home");
            }
           
        }   
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound(string? message, string? code)
        {
            Response.StatusCode = 404;
            ViewBag.ErrorCode = code;
            ViewBag.Message = message;
            SetViewData("Page Not Found", null, "image-overlay error-image");
            return View();
        }
       
        public IActionResult InternalServerError(string? message, string? code)
        {
            Response.StatusCode = 500;
            ViewBag.ErrorCode = code;
            ViewBag.Message = message;
            SetViewData("Internal Server Error", null, "image-overlay error-image");
            return View();
        }
    }
}
