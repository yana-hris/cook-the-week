namespace CookTheWeek.Web.Controllers
{
    using System.Net.Mail;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    
    using Common.Exceptions;
    using Data.Models;
    using Data.Repositories;
    using Infrastructure.Extensions;
    using Services.Data.Interfaces;
    using Services.Data.Vlidation;
    using ViewModels.User;


    using static Common.EntityValidationConstants.ApplicationUser;
    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;
        private readonly IEmailSender emailSender;
        private readonly IValidationService validationService;
        private readonly ILogger<UserController> logger;
        

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IUserService userService,
                              IMemoryCache memoryCache,
                              IEmailSender emailSender,
                              IValidationService validationService,
                              ILogger<UserController> logger)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.emailSender = emailSender;
            this.validationService = validationService;
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
                AddModelErrors(validationResult);
                return View(model); 
            }

            ApplicationUser user = null;

            try
            {
                user = await this.userService.CreateUserAsync(model);

                // Try to generate the email confirmation token
                var token = await userService.GenerateTokenForEmailConfirmationAsync(user);

                // Generate callback URL and send the confirmation email
                var callbackUrl = Url.Action(nameof(ConfirmedEmail), "User", new { userId = user.Id, code = token }, Request.Scheme);
                await this.emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                // If everything is successful, redirect to the confirmation info page
                return RedirectToAction("EmailConfirmationInfo", "User", new { email = model.Email });
            }
            catch (InvalidOperationException ex)
            {
                // Handle token generation failure
                logger.LogError($"Token generation failed. Error message: {ex.Message}. Error stacktrace: {ex.StackTrace}");
            }
            catch (SmtpException ex)
            {
                // Handle email sending failure
                logger.LogError($"Email sending failed. Error message: {ex.Message}. Error stacktrace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                logger.LogError($"Unexpected error occurred, error message: {ex.Message}, error stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
            }
            finally
            {
                // Always try to delete the user if something failed
                if (user != null)
                {
                    try
                    {
                        await userService.DeleteUserAsync(user);
                    }
                    catch (ArgumentNullException deleteEx)
                    {
                        logger.LogError($"User deletion failed because user was NULL. Error message: {deleteEx.Message}, Error StackTrace: {deleteEx.StackTrace}");
                    }
                    catch (InvalidOperationException deleteEx)
                    {
                        logger.LogError($"User deletion failed. Error message: {deleteEx.Message}, Error StackTrace: {deleteEx.StackTrace}");
                    }
                }

                // Always redirect to ConfirmationFailed if there was any failure
                return RedirectToAction("ConfirmationFailed");
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
                var user = await userRepository.GetUserByIdAsync(userId);
                var result = await userRepository.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    // if confirmation goes well, log in the user
                    await this.userRepository.SignInUserAsync(user);

                    TempData[JustLoggedIn] = true;
                    this.memoryCache.Remove(UsersCacheKey);

                    return View();
                }

                logger.LogError("No errors were thrown, but email confirmation failed.");
                return RedirectToAction("AccountDeletedConfirmation");
            }
            catch (RecordNotFoundException ex)
            {
                logger.LogError($"User with id {userId} not found in the database. Error message: {ex.Message}; ErrorStacktrace: {ex.StackTrace}");
                return RedirectToAction("NotFound", "Home", new {message = ex.Message, code = ex.ErrorCode});
            }
            catch(ArgumentNullException ex)
            {
                logger.LogError($"User token was null, email not confirmed successfully. Error message: {ex.Message}; Error Stacktrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
            }
            catch(Exception ex)
            {
                logger.LogError($"Unidentified error upon user email confirmation. Error message: {ex.Message}; ErrorStactrace: {ex.StackTrace}");
                return RedirectToAction("InternalServerError", "Home");
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

            ApplicationUser? user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, LoginFailedErrorMessage);
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            
            if(result.IsLockedOut)
            {
                await userManager.AccessFailedAsync(user);
                ModelState.AddModelError(string.Empty, AccountLockedErrorMessage);

                return View(model);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, LoginFailedErrorMessage);

                return View(model);
            }


            this.memoryCache.Remove(UsersCacheKey);

            // Redirect Admin User to Admin Area
            if (this.User.IsAdmin())
            {
                return RedirectToAction("Index", "HomeAdmin", new { area = AdminAreaName });
            }

            // Redirect User to return url or Home
            TempData[JustLoggedIn] = true;
            return Redirect(model.ReturnUrl ?? "/Recipe/All");
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
                UserProfileViewModel model = await this.userService.DetailsByIdAsync(userId);
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

            ApplicationUser? user = await userRepository.FindByEmailAsync(model.Email);
           
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


    }
}
