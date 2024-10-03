namespace CookTheWeek.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Services.Data.Models.Validation;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.Infrastructure.Extensions;
    using CookTheWeek.Web.ViewModels.User;
    
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

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
                
                // Retrieve whether the user is an admin from the result data
                bool isAdmin = (bool)result.Data[IsAdmin];

                if (isAdmin)
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
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User", new { returnUrl });

            // Call the userService to get the external login properties
            var properties = userService.GetExternalLoginProperties(schemeProvider, redirectUrl);

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

            try
            {
                OperationResult result = await userService.TryLoginOrRegisterUserWithExternalLoginProvider();

                if (!result.Succeeded)
                {
                    return RedirectToAction("Login", new { returnUrl });
                }

                TempData[JustLoggedIn] = true;
                this.memoryCache.Remove(UsersCacheKey);
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            catch(InvalidOperationException)
            {
                return RedirectToAction("Login", new { returnUrl });
            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
            }

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
            try
            {
                await userService.TryLogOutUserAsync();

                // Delete IsOnline cookie from memoryCache 
                this.memoryCache.Remove(UsersCacheKey);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
            }
            
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

            try
            {
                var result = await userService.TrySendPasswordResetEmailAsync(model);
                if (!result.Succeeded)
                {
                    logger.LogError($"Sending email for password reset failed. Errors: {result.Errors}");
                }
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during registration: {ex.Message}. Error StackTrace: {ex.StackTrace}", ex);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
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

            var result = await userService.TryResetPasswordAsync(model);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Value);
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
            
            var result = await userService.TryChangePasswordAsync(userId, model);

            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "Your password has been changed successfully.";
                return RedirectToAction("Profile");
            }
           
            foreach (var error in result.Errors)
            {
                if (error.Key == "UserError")
                {
                    return RedirectToAction("NotFound", "Home", new {area = ""});
                }
                else if (error.Key == nameof(model.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), error.Value);
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Value);
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

            var result = await userService.TrySetPasswordAsync(userId, model);

            if (result.Succeeded)
            {
                TempData[SuccessMessage] = "Your password has been set successfully.";
                return RedirectToAction("Profile");
            }

            // Handle user not found error
            if (result.Errors.Any(e => e.Key == "UserError"))
            {
                return RedirectToAction("NotFound", "Home", new {area = ""});
            }

            // Handle all other errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Value);
            }

            // Return view with errors
            return View(model);
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
            try
            {
                OperationResult result = await userService.TryDeleteUserAccountAsync();

                if (result.Succeeded)
                {
                    return RedirectToAction("AccountDeletedConfirmation");
                }

                // Handle errors
                foreach (var error in result.Errors)
                {
                    if (error.Key == "UserError")
                    {
                        return NotFound(error.Value);
                    }

                    ModelState.AddModelError(string.Empty, error.Value);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex);
                
            }

        }

        [HttpGet]
        public IActionResult AccountDeletedConfirmation()
        {
            return View();
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
