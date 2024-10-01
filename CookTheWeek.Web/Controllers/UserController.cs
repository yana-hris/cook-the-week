namespace CookTheWeek.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.User;
    
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;
    using CookTheWeek.Common;

    [AllowAnonymous]
    public class UserController : BaseController
    {
        
        private readonly IUserService userService;
        private readonly IValidationService validationService;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<UserController> logger;
        

        public UserController(IUserService userService,
                              IMemoryCache memoryCache,
                              IValidationService validationService,
                              ILogger<UserController> logger)
        {
            this.validationService = validationService;
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var validationResult = await validationService.ValidateRegisterUserModelAsync(model);

            if (!validationResult.IsValid)
            {
                AddCustomValidationErrorsToModelState(validationResult.Errors);
            }

            try
            {
                var result = await userService.TryRegisterUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("EmailConfirmationInfo", "User", new { email = model.Email });
                }
                
                return RedirectToAction("ConfirmationFailed");
            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
            }
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
            try
            {
                OperationResult result = await userService.TyrConfirmEmailAsync(userId, code);

                if (!result.Succeeded)
                {
                    logger.LogError($"Email confirmation Failed. Errors: {result.Errors}");
                    return RedirectToAction("ConfirmationFailed");
                }

                TempData[JustLoggedIn] = true;
                this.memoryCache.Remove(UsersCacheKey);

                return View();

            }
            catch(Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
            }
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

            try
            {
                var result = await userService.TryLoginUserAsync(model);

                if (!result.Succeeded)
                {
                    AddCustomValidationErrorsToModelState(result.Errors);
                    return View(model);
                }

                this.memoryCache.Remove(UsersCacheKey);

                // This will not work before the reponse is received
                if (this.User.IsAdmin())
                {
                    return RedirectToAction("Index", "HomeAdmin", new { area = AdminAreaName });
                }
                
                TempData[JustLoggedIn] = true;
                return Redirect(model.ReturnUrl ?? "/Recipe/All");

            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
            }

        }

        [HttpGet]
        public IActionResult ExternalLogin(string schemeProvider, string? returnUrl = null)
        {
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User", new {returnUrl});
            var properties = signInManager.ConfigureExternalAuthenticationProperties(schemeProvider, redirectUrl);
            return new ChallengeResult(schemeProvider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle external provider error
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction("Login", new { returnUrl});
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle error
                return RedirectToAction("Login", new {returnUrl});
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

            try
            {
                UserProfileViewModel model = await this.userService.GetDetailsModelByIdAsync(userId);
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"User with id {userId} does not exist in the database. Error message: {ex.Message}, Error StackTrace: {ex.StackTrace}");
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code=ex.ErrorCode});
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordFormModel model = new ForgotPasswordFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser? user = await userRepository.GetByEmailAsync(model.Email);
           
            bool isEmailConfirmed = user != null ? await userRepository.IsUserEmailConfirmedAsync(user) : false;

            // Redirect the non-existent user without generating a token or sending any email
            if(user == null || !isEmailConfirmed)
            {
                return RedirectToAction("ForgotPasswordConfirmation", "User");
            }

            // Generate the token only if the user exists
            var token = await userRepository.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "User", new { token, email = model.Email }, protocol: Request.Scheme);
            string tokenExpirationTime = DateTime.Now.AddHours(24).ToString();

            var result = await emailSender.SendEmailAsync(
                model.Email,
                "Reset Password",
                $"Please reset your password by clicking here. The link will be active until {tokenExpirationTime}",
                $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>. The link will be active until {tokenExpirationTime}");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var model = new ResetPasswordFormModel()
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Password = model.Password;

            var user = await userRepository.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            IdentityResult? result = await userRepository.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // Handle token expiration
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
            {
                ModelState.AddModelError(string.Empty, "The token is invalid or has expired. Please request a new password reset link.");
                return View(model); 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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
        public IActionResult AccountDeletedConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmationFailed()
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

            await this.userService.DeleteUserAndUserDataAsync(userId);
            await this.signInManager.SignOutAsync();

            return RedirectToAction("AccountDeletedConfirmation");
        }

        private void AddModelErrors(ValidationResult validationResult)
        {
            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }

        
        /// <summary>
        /// Utility method that logs the error message and stacktrace in case of Unhandled Exceptions and redirects to Internal Server Error Page
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private IActionResult LogExceptionAndRedirectToInternalServerError(Exception ex)
        {
            logger.LogError($"Error during registration: {ex.Message}. Error StackTrace: {ex.StackTrace}", ex);

            // Redirect to the internal server error page with the exception message
            return RedirectToAction("InternalServerError", "Home", new { message = ex.Message });

        }

    }
}
