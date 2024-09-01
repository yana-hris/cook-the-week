namespace CookTheWeek.Web.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    

    
    using CookTheWeek.Web.Infrastructure.Extensions;
    using Data.Models;
    using ViewModels.User;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using CookTheWeek.Services.Data.Interfaces;
    using System.Security.Claims;

    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;
        

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IUserService userService,
                              IMemoryCache memoryCache)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.memoryCache = memoryCache;
            
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
            TempData["JustLoggedIn"] = true;
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

            if (this.User.IsAdmin())
            {
                return RedirectToAction("Index", "HomeAdmin", new { area = AdminAreaName });
            }
            TempData["JustLoggedIn"] = true;
            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [HttpGet]
        public IActionResult ExternalLogin(string schemeProvider, string? returnUrl = null)
        {
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User");
            var properties = signInManager.ConfigureExternalAuthenticationProperties(schemeProvider, redirectUrl);
            return new ChallengeResult(schemeProvider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle external provider error
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle error
                return RedirectToAction("Login");
            }
            
            // Extract the username, email, and user ID from the external login info
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // Check if the user already exists
            var user = await userManager.FindByEmailAsync(email);

            if(user == null)
            {
                user = new ApplicationUser // If the user does not exist, create a new one
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    // Handle failure to create user
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Error");
                }

                // Add the external login (Google, etc.) to the user
                result = await userManager.AddLoginAsync(user, info);
                if (!result.Succeeded)
                {
                    // Handle failure to add login
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Error");
                }
            }


            // Sign in the user
            await signInManager.SignInAsync(user, isPersistent: false);

            TempData["JustLoggedIn"] = true;
            this.memoryCache.Remove(UsersCacheKey);
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));            
        }

        [HttpGet]
        public IActionResult AccessDeniedPathInfo()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            // Delete IsOnline cookie from memoryCache 
            this.memoryCache.Remove(UsersCacheKey);

            // Delete the .AspNet.Consent cookie
            HttpContext.Response.Cookies.Delete(CookieConsentName);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string userId = User.GetId();
            UserProfileViewModel? model = await this.userService.GetProfile(userId);

            if (model == null)
            {
                return RedirectToAction("Error404", "Home");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordFormModel model)
        {
            string userId = User.GetId();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await userService.ChangePasswordAsync(userId, model);

            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "Your password has been changed successfully.";
                return RedirectToAction("Profile");
            }
           
            foreach (var error in result.Errors)
            {
                if(error.Description == UserNotFoundErrorMessage)
                {
                    return RedirectToAction("Error404", "Home");
                } 
                else if(error.Description == IncorrectPasswordErrorMessage)
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), IncorrectPasswordErrorMessage);
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return BadRequest(ModelState);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetPassword(SetPasswordFormModel model)
        {
            string userId = User.GetId();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await userService.SetPasswordAsync(userId, model);

            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "Your password has been set successfully.";
                return RedirectToAction("Profile");
            }

            // Check if the error indicates that the user was not found
            var userNotFoundError = result.Errors.FirstOrDefault(e => e.Description == UserNotFoundErrorMessage);
            if (userNotFoundError != null)
            {
                return RedirectToAction("Error404", "Home");
            }

            // Handle other errors, such as password change failure
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Return BadRequest if there are errors in setting the password
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> AccountDeletedConfirmation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found.");
            }

            await this.userService.DeleteUserAsync(userId);
            await this.signInManager.SignOutAsync();

            return RedirectToAction("AccountDeletedConfirmation");
        }

        
    }
}
