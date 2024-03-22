namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using CookTheWeek.Data.Models;
    using CookTheWeek.Web.ViewModels.User;
    using Microsoft.AspNetCore.Authentication;

    using static Common.NotificationMessagesConstants;

    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
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

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                TempData[ErrorMessage] = 
                    "There was an error while logging you in! Please try again later or contact an administrator.";
                
                return View(model);
            }


            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
    }
}
