namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Ganss.Xss;

    using Data.Models;
    using ViewModels.User;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using CookTheWeek.Web.Infrastructure.Extensions;

    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache memoryCache;
        private readonly HtmlSanitizer sanitizer;

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IMemoryCache memoryCache)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.memoryCache = memoryCache;
            sanitizer = new HtmlSanitizer();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model); 
            }

            ApplicationUser user = new ApplicationUser();

            // Sanitizing User Input
            model.Username = SanitizeInput(model.Username);
            model.Password = SanitizeInput(model.Password);
            model.ConfirmPassword = SanitizeInput(model.ConfirmPassword);

            await userManager.SetUserNameAsync(user, model.Username);
            await userManager.SetEmailAsync(user, user.Email);

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded) 
            {
                foreach (IdentityError Error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }

                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            this.memoryCache.Remove(UsersCacheKey);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Sanitizing User Input
            model.Username = SanitizeInput(model.Username);
            model.Password = SanitizeInput(model.Password);

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                TempData[ErrorMessage] = 
                    "There was an error while logging you in! Please try again later or contact an administrator.";
                
                return View(model);
            }

            this.memoryCache.Remove(UsersCacheKey);

            if (User.IsAdmin())
            {
                return RedirectToAction("Index", "HomeAdmin", new { area = AdminAreaName });
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            this.memoryCache.Remove(UsersCacheKey);
            return RedirectToAction("Index", "Home");
        }

        private string SanitizeInput(string input)
        {
            return sanitizer.Sanitize(input);
        }

    }
}
