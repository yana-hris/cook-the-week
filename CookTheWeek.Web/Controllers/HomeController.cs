namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    using CookTheWeek.Web.ViewModels.Home;
    using Infrastructure.Extensions;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    [AllowAnonymous]
    public class HomeController : Controller
    {        
        private readonly string apiKey;
        private readonly ILogger<HomeController> logger;
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration,
            ILogger<HomeController> logger) 
        {
            apiKey = configuration["SendGrid:CookTheWeek_API_Key"]!;
            this.configuration = configuration;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            if(this.User.IsAdmin())
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

        public IActionResult About()
        {
            string userId = User.GetId();
            if (string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        public IActionResult HowItWorks()
        {
            string userId = User.GetId();
            if (string.IsNullOrEmpty(userId))
            {
                return View();
            }

            return RedirectToAction("All", "Recipe");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(ContactFormModel model)
        {

            if (ModelState.IsValid)
            {
                var client = new SendGridClient(apiKey);
                
                var from = new EmailAddress(configuration["EmailSettings:FromEmail"], "Cook The Week Support");
                var subject = model.Subject;
                var to = new EmailAddress(configuration["EmailSettings:ToEmail"], "My personal Mail");
                var plainTextContent = $"Name: {model.FullName}\nEmail: {model.EmailAddress}\nMessage: {model.Message}";
                var htmlContent = $"<strong>Name:</strong> {model.FullName}<br><strong>Email:</strong> {model.EmailAddress}<br><strong>Message:</strong> {model.Message}";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    TempData[SuccessMessage] = "Thank you for your message. We will get back to you soon.";
                }
                else
                {
                    logger.LogError($"Failed to send contact form message for user {User.GetId()}");
                    TempData[ErrorMessage] = "Error sending email. Please try again later.";
                }

                return View("About");
            }

            return View("About", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if(statusCode != null) 
            {
                if(statusCode.Value == 404 || statusCode.Value == 401)
                {
                    return View("Error404");
                }
                else
                {
                    return View("Error500");
                }
                
            } 
            
            return View("Error");
        }
    }
}
