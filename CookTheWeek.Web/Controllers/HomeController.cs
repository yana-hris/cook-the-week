namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SendGrid.Helpers.Mail;

    using CookTheWeek.Services.Data.Interfaces;
    using CookTheWeek.Web.ViewModels.Home;
    using Infrastructure.Extensions;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    [AllowAnonymous]
    public class HomeController : BaseController
    { 
        
        private readonly ILogger<HomeController> logger;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration,
            IEmailSender emailSender,
            ILogger<HomeController> logger) 
        {
            this.configuration = configuration;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if(User.IsAdmin())
            {
                return this.RedirectToAction("Index", "HomeAdmin", new { Area = AdminAreaName });
            }

            string userId = User.GetId();
            if(string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
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
            
            if (ModelState.IsValid)
            {
                var to = new EmailAddress(configuration["EmailSettings:ToEmail"], "My personal Mail");
                var plainTextContent = $"Name: {model.FullName}\nEmail: {model.EmailAddress}\nMessage: {model.Message}";
                var htmlContent = $"<strong>Name:</strong> {model.FullName}<br><strong>Email:</strong> {model.EmailAddress}<br><strong>Message:</strong> {model.Message}";
                                
                var response = await emailSender.SendEmailAsync(model.EmailAddress, model.Subject, plainTextContent, htmlContent);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    // TODO: Create a ContactFormSubmitConfimrationView (success or not)
                    TempData[SuccessMessage] = "Thank you for your message. We will get back to you soon.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    logger.LogError($"Failed to send contact form message from user with e-mail {model.EmailAddress}");
                    // TODO: Create a ContactFormSubmitConfimrationView (success or not)
                    TempData[ErrorMessage] = "Error sending email. Please try again later.";
                    return RedirectToAction("Error");
                }                
            }

            return View(model);            
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
