namespace CookTheWeek.Web.Controllers
{
    
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.User;
    
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.NotificationMessagesConstants;
    using static CookTheWeek.Common.TempDataConstants;

    [AllowAnonymous]
    public class UserController : BaseController
    {
        
        private readonly IUserService userService;
        private readonly IValidationService validationService;
        private readonly IMemoryCache memoryCache;
        private readonly Guid userId;
        

        public UserController(IUserService userService,
                              IMemoryCache memoryCache,
                              IUserContext userContext,
                              IValidationService validationService,
                              ILogger<UserController> logger) : base(logger)
        {
            this.validationService = validationService;
            this.userService = userService;
            this.userId = userContext.UserId;
            this.memoryCache = memoryCache;
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            
            if (userId != Guid.Empty)
            {
                TempData[ErrorMessage] = "You are already registered and logged in!";
                return RedirectToAction("Index", "Home");
            }
            SetViewData("Register", null, "image-overlay food-background");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            SetViewData("Register", null, "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var validationResult = await validationService.ValidateRegisterUserModelAsync(model);

            if (!validationResult.IsValid)
            {
                AddCustomValidationErrorsToModelState(validationResult.Errors);
                return View(model);
            }

            Guid newUserId = Guid.Empty;

            try
            {
                var result = await userService.TryRegisterUserAsync(model);

                if (result.Succeeded && result.Data.Count == 2)
                {
                    newUserId = Guid.Parse(result.Data["userId"].ToString());
                    string token = result.Data["code"].ToString()!;
                    
                    string? callbackUrl = Url.Action(
                        "ConfirmedEmail", "User",
                        new { userToConfirmId = newUserId, code = token },
                        Request.Scheme);

                    
                    await userService.SendConfirmationEmailAsync(model.Email, callbackUrl);
                    return RedirectToAction("EmailConfirmationInfo", "User", new { email = model.Email });
                }
            }
            catch (InvalidOperationException)
            {
                TempData[ErrorMessage] = "Sending Confirmation Email failed!";
            }
            catch (Exception ex)
            {
                await userService.DeleteUserByIdIfExistsAsync(newUserId);
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(Register));
            }

            await userService.DeleteUserByIdIfExistsAsync(newUserId);
            return RedirectToAction("ConfirmationFailed");
        }
        
        [HttpGet]
        public IActionResult EmailConfirmationInfo(string email)
        {
            ViewBag.Email = email;
            SetViewData("Email Confirmation Info", null, "image-overlay food-background");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmedEmail(string userToConfirmId, string code)
        {
            try
            {
                OperationResult result = await userService.TyrConfirmEmailAsync(userToConfirmId, code);

                if (!result.Succeeded)
                {
                    logger.LogError($"Email confirmation Failed. Errors: {result.Errors}");
                    return RedirectToAction("ConfirmationFailed");
                }

                TempData[JustLoggedIn] = true;
                this.memoryCache.Remove(UsersCacheKey);

                SetViewData("Confirmed Email", null, "image-overlay food-background");
                return View();

            }
            catch(Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(ConfirmedEmail));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            if (userId != Guid.Empty)
            {
                TempData[ErrorMessage] = "You are already logged in";
                return RedirectToAction("Index", "Home");
            }


            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginFormModel model = new LoginFormModel()
            {
                ReturnUrl = returnUrl ?? "/Recipe/All"
            };
            
            SetViewData("Login", model.ReturnUrl, "image-overlay food-background");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            SetViewData("Login", model.ReturnUrl, "image-overlay food-background");

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
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(Login));
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
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(ExternalLoginCallback));
            }

        }

        [HttpGet]
        public IActionResult AccessDeniedPathInfo()
        {
            SetViewData("Access Denied", null, "image-overlay food-background");
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await userService.TryLogOutUserAsync();
               
                if (HttpContext != null)
                {
                    // Delete the IsOnline cookie from memoryCache 
                    this.memoryCache.Remove(UsersCacheKey);

                    // Delete the CookieConsentName cookie from the HttpContext
                    HttpContext.Response.Cookies.Delete(CookieConsentName);
                }
                else
                {
                    logger.LogWarning("HttpContext is null while attempting to log out.");
                }
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(Logout));
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                UserProfileViewModel model = await this.userService.GetUserProfileDetailsAsync();
                SetViewData("Profile", null, "image-overlay food-background");
                return View(model);
            }
            catch (RecordNotFoundException ex)
            {
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code=ex.ErrorCode});
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordFormModel model = new ForgotPasswordFormModel();
            SetViewData("Forgot Password", null, "image-overlay food-background");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            SetViewData("Forgot Password", null, "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Step 1: Call the service to find the user and generate the token
                var result = await userService.TryGetPasswordResetTokenAsync(model.Email);

                if (!result.Succeeded)
                {
                    // If result indicates a failure, log the error
                    logger.LogError($"Password reset token generation failed. Errors: {result.Errors}");
                }
                else
                {
                    if (result.Data.Count > 0 && !string.IsNullOrEmpty(result.Data["token"].ToString())) // send email only if there is a token
                    {
                        // Step 2: Construct the password reset URL using the token from the result
                        var callbackUrl = Url.Action(
                            "ResetPassword",
                            "User",
                            new { email = model.Email, code = result.Data["token"].ToString() },
                            Request.Scheme);

                        // Step 3: Send the password reset email via the service
                        string username = result.Data["username"].ToString() ?? model.Email;
                        await userService.SendPasswordResetEmailAsync(model.Email, username, callbackUrl);
                    }

                    // Redirect to confirmation view in all Success scenarios
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                logger.LogError($"Error during password reset: {ex.Message}. Error StackTrace: {ex.StackTrace}", ex);
            }

            return RedirectToAction("ForgotPasswordConfirmation");
        }


        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            SetViewData("Forgot Password Confirmation", null, "image-overlay food-background");
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword([FromQuery]string code, string email)
        {
            
            SetViewData("Reset Password", null, "image-overlay food-background");

            if (code == null || email == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var model = new ResetPasswordFormModel()
            {
                Token = code,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel model)
        {
            SetViewData("Reset Password", null, "image-overlay food-background");

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
            SetViewData("Reset Password Confirmation", null, "image-overlay food-background");
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            SetViewData("Change Password", null, "image-overlay food-background");
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordFormModel model)
        {
            SetViewData("Change Password", null, "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await userService.TryChangePasswordAsync(model);

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
            SetViewData("Set Password", null, "image-overlay food-background");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetPassword(SetPasswordFormModel model)
        {
            SetViewData("Set Password", null, "image-overlay food-background");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await userService.TrySetPasswordAsync(model);

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
            SetViewData("Confirmation Failed", null, "image-overlay food-background");
            return View();
        }

        [Authorize]
        [HttpGet]
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
                return LogExceptionAndRedirectToInternalServerError(ex, nameof(DeleteAccount));
                
            }

        }

        [HttpGet]
        public IActionResult AccountDeletedConfirmation()
        {
            SetViewData("Account Deleted Confirmation", null, "image-overlay food-background");
            return View();
        }
        
        /// <summary>
        /// Utility method that logs the error message and stacktrace in case of Unhandled Exceptions and redirects to Internal Server Error Page
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private IActionResult LogExceptionAndRedirectToInternalServerError(Exception ex, string actionName)
        {
            logger.LogError($"Error in User Action: {actionName}. Error message: {ex.Message}. Error StackTrace: {ex.StackTrace}", ex);

            // Redirect to a custom error page with a generic error message
            string userFriendlyMessage = "An unexpected error occurred. Please try again later.";
            return RedirectToAction("InternalServerError", "Home", new { message = userFriendlyMessage });

        }

    }
}
