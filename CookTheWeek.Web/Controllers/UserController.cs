﻿namespace CookTheWeek.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Services.Data.Interfaces;
    using Data.Models;
    using ViewModels.User;

    using static Common.NotificationMessagesConstants;
    using static Common.GeneralApplicationConstants;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;
        private readonly IEmailSender emailSender;
        

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IUserService userService,
                              IMemoryCache memoryCache,
                              IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.emailSender = emailSender;
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

            model.Email = SanitizeInput(model.Email);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = SanitizeInput(model.Username),
                Email = model.Email
            };

            IdentityResult identityResult = await userManager.CreateAsync(user, SanitizeInput(model.Password));

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError Error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, Error.Description);
                }

                return View(model);
            }

            // Generate the email confirmation token
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ConfirmedEmail), "User", new { userId = user.Id, code = token }, Request.Scheme);

            // Send email
            var responseResult = await emailSender.SendEmailAsync(
                model.Email, 
                "Confirm your email", 
                $"Please confirm your account by clicking this link: {callbackUrl}",
                $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");

            if (responseResult != null && !responseResult.IsSuccessStatusCode)
            {
                // Optionally delete the user if email failed to send
                await userManager.DeleteAsync(user);
                return RedirectToAction("Error", "Home"); // Return an error view if the email sending failed
            }           
            
            return RedirectToAction("EmailConfirmationInfo", "User", new {email = model.Email});
        }

        [HttpGet]
        public IActionResult EmailConfirmationInfo(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmedEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                // if confirmation goes well, log in the user
                await signInManager.SignInAsync(user, isPersistent: false);
                TempData[JustLoggedIn] = true;
                this.memoryCache.Remove(UsersCacheKey);

                return View();
            }

            return RedirectToAction("Error", "Home");
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
            TempData[JustLoggedIn] = true;
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

            TempData[JustLoggedIn] = true;
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
