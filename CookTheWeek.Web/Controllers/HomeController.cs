namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Home;
    using CookTheWeek.Web.Infrastructure.ActionFilters;
    using CookTheWeek.Web.Infrastructure.Extensions;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    [AllowAnonymous]
    public class HomeController : BaseController
    { 
        
        private readonly ILogger<HomeController> logger;
        private readonly IEmailSender emailSender;

        public HomeController(IEmailSender emailSender,
            ILogger<HomeController> logger) 
        {
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [HttpGet]
        [AdminRedirect("Index", "HomeAdmin")]
        public IActionResult Index()
        {
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
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
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

        [Route("Home/NotFound")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound(string? message, string? code)
        {
            Response.StatusCode = 404;
            ViewBag.ErrorCode = code;
            ViewBag.Message = message;  
            return View();
        }

        [Route("Home/InternalServerError")]
        public IActionResult InternalServerError(string? message, string? code)
        {
            Response.StatusCode = 500;
            ViewBag.ErrorCode = code;
            ViewBag.Message = message;
            return View();
        }
    }
}
