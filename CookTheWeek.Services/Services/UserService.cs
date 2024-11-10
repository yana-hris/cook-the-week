namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;
    using System.Security.Claims;
    
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using CookTheWeek.Common;
    using CookTheWeek.Common.Exceptions;
    using CookTheWeek.Data.Models;
    using CookTheWeek.Data.Repositories;
    using CookTheWeek.Services.Data.Services.Interfaces;
    using CookTheWeek.Web.ViewModels.Admin.UserAdmin;
    using CookTheWeek.Web.ViewModels.User;

    using static CookTheWeek.Common.EntityValidationConstants;
    using static CookTheWeek.Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.GeneralApplicationConstants;
    using static CookTheWeek.Common.TempDataConstants;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository; 
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IRecipeService recipeService;
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;        

        private readonly IEmailSender emailSender;
        private readonly ILogger<UserService> logger;
        private readonly Guid userId;

        public UserService(IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserContext userContext,
            IRecipeService recipeService,
            IValidationService validationService,
            IEmailSender emailSender,
            IMealPlanService mealPlanService,
            ILogger<UserService> logger)
        {
            
            this.userRepository = userRepository;

            this.recipeService = recipeService;
            this.mealPlanService = mealPlanService;
            this.validationService = validationService;
            this.signInManager = signInManager;
            this.userManager = userManager;

            this.emailSender = emailSender;
            this.userId = userContext.UserId;
            this.logger = logger;   
        }
        public async Task<UserProfileViewModel> GetUserProfileDetailsAsync()
        {

            var user = await userRepository
                .GetAllQuery()
                .Include(u => u.Recipes)
                .Include(u => u.FavoriteRecipes)
                .Include(u => u.MealPlans)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                logger.LogError($"User with id {userId} does not exist in the database.");
                throw new RecordNotFoundException(RecordNotFoundExceptionMessages.UserNotFoundExceptionMessage, null);
            }

            bool hasPassword = await userManager.HasPasswordAsync(user);

            int recipes = user.Recipes.Count + user.FavoriteRecipes.Count;
            int mealplans = user.MealPlans.Count;

            return new UserProfileViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!,
                HasPassword = hasPassword,
                RecipesCount = recipes,
                MealplansCount = mealplans
            };
        }

        
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }
            
            bool oldPasswordMatches = await userManager.CheckPasswordAsync(user, model.CurrentPassword);

            if (!oldPasswordMatches)
            {
                return IdentityResult.Failed(new IdentityError { Description = IncorrectPasswordErrorMessage });
            }

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> SetPasswordAsync(SetPasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            var result = await userManager.AddPasswordAsync(user, model.NewPassword);

            return result;
        }

        /// <summary>
        /// A helper method that deletes the user and all user data: meal plans (and related meals), added recipes, favourite recipes.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<OperationResult> DeleteUserAndUserDataAsync()
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "UserError", "User not found." }
                });
            }
            
            try
            {
                await userManager.DeleteAsync(user); // Will cascade and delete all user data
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting user {userId}: {ex.Message}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "UserDeletionError", "An error occurred while deleting the user account." }
                });
            }
        }

        /// <summary>
        /// Returns the sum of all users, registered in the database
        /// </summary>
        /// <returns>int or 0</returns>
        public async Task<int?> AllCountAsync()
        {
            return await userRepository.AllCountAsync();
        }

        /// <summary>
        /// Returns a collection of UserAllViewModel which contains all users, registered in the database
        /// </summary>
        /// <returns>A collection of UserAllViewModel</returns>
        public async Task<ICollection<UserAllViewModel>> GetAllAsync()
        {
            ICollection<UserAllViewModel> users = await userRepository
                .GetAllQuery()
                .Select(u => new UserAllViewModel()
                {
                    Id = u.Id.ToString(),
                    Username = u.UserName,
                    Email = u.Email

                }).ToListAsync();

            foreach (var user in users)
            {
                user.TotalRecipes = await recipeService.GetMineCountAsync();
                user.TotalMealPlans = await mealPlanService.MineCountAsync();
            }

            return users;
        }
        
        public AuthenticationProperties? GetExternalLoginProperties(string provider, string? redirectUrl)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryRegisterUserAsync(RegisterFormModel model)
        {
            string newUserId = string.Empty;
            try
            {
                ApplicationUser user = await CreateUserAsync(model);
                
                var token = await GenerateTokenForEmailConfirmationAsync(user);
                
                newUserId = user.Id.ToString();
                var data = new Dictionary<string, object>()
                {
                    { "userId", newUserId},
                    { "code", token }
                };

                // Return the result with the user ID and token to the controller
                return OperationResult.Success(data);

            }
            catch (InvalidOperationException ex)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "RegisterError", ex.Message }
                });
            }
            catch(Exception ex)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "RegisterError", ex.Message }
                });
            }
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TyrConfirmEmailAsync(string userConfirmedId, string code)
        {
            // Ensure userId is not null or empty
            if (string.IsNullOrEmpty(userConfirmedId))
            {
                logger.LogError("UserId is null or empty during email confirmation.");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {"EmailConfirmationError", "UserId is missing." }
                });
            }

            var user = await userRepository.GetByIdAsync(Guid.Parse(userConfirmedId));

            if (user == null)
            {
                logger.LogError($"Email Confirmation failed. User with userId: {userId} not found in the database."); 
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailConfirmationError", RecordNotFoundExceptionMessages.UserNotFoundExceptionMessage }
                });
            }

            if (string.IsNullOrEmpty(code))
            {
                logger.LogError($"Token for Email Confirmation is null. UserId: {userId}. Token: {code}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {"EmailConfirmationError", ArgumentNullExceptionMessages.TokenNullExceptionMessage }
                });
                
            }

            try
            {
                var confirmationResult = await userManager.ConfirmEmailAsync(user, code);

                if (confirmationResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return OperationResult.Success();
                }

                logger.LogError($"Email confirmation failed. The email confirmation token does not match the received token.");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {"EmailConfirmationError", "The email confirmation token does not match." }
                });

            }
            catch (Exception ex)
            {
                logger.LogError($"Email confirmation failed unexpectedly. Error message: {ex.Message}. Error Stacktrace: {ex.StackTrace}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {"EmailConfirmationError", ex.Message }
                });
            }
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryLoginUserAsync(LoginFormModel model)
        {
            var user = await userRepository.GetByUsernameAsync(model.Username);

            if (user == null)
            {
                logger.LogError($"User Login failed. User with username {model.Username} not found in the database.");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {$"{nameof(model.Username)}",  ApplicationUserValidation.UsernameNotFoundErrorMessage}
                });
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    logger.LogError($"User account is locked out. Username: {model.Username}");
                }
                else
                {
                    logger.LogError($"Invalid credentials for user {model.Username}.");
                }

                await userManager.AccessFailedAsync(user);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {string.Empty,  ApplicationUserValidation.LoginFailedErrorMessage}
                });
            }

            bool isAdmin = await userManager.IsInRoleAsync(user, AdminRoleName);

            // Return OperationResult with isAdmin data
            return OperationResult.Success(new Dictionary<string, object>
            {
                { IsAdmin, isAdmin }
            });
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryLoginOrRegisterUserWithExternalLoginProvider()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                logger.LogError($"Failed to retrieve external login info for user in action {nameof(TryLoginOrRegisterUserWithExternalLoginProvider)}");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.ExternalLoginInfoUnsuccessfulExceptionMessage);
            }

            // Extract the email from the external login info
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // Check if the user already exists
            var user = await userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser // If the user does not exist, create a new one
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult userResult = await userManager.CreateAsync(user);

                if (!userResult.Succeeded)
                {
                    logger.LogError($"User creation from external login info failed. Errors: {userResult.Errors}");
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.UserUnsuccessfullyCreatedExceptionMessage);
                }

                // Add the external login (Google, etc.) to the user
                var registerExternalLoginResult = await userManager.AddLoginAsync(user, info);

                if (!registerExternalLoginResult.Succeeded)
                {
                    await DeleteUserByIdIfExistsAsync(user.Id);
                    logger.LogError($"External login info adding to user failed. Errors: {registerExternalLoginResult.Errors}");
                    throw new InvalidOperationException(InvalidOperationExceptionMessages.UserExternalLoginInfoUnsuccessfullyAddedExceptionMessage);
                }

            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return OperationResult.Success();
        }

        /// <inheritdoc/>
        public async Task TryLogOutUserAsync()
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "An invalid operation occurred during sign-out.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred during sign-out.");
                throw;
            }
        }


        /// <inheritdoc/>
        public async Task<OperationResult> TryGetPasswordResetTokenAsync(string email)
        {
            ApplicationUser? user = await userRepository.GetByEmailAsync(email);

            bool isEmailConfirmed = user != null ? await userManager.IsEmailConfirmedAsync(user) : false;

            // Redirect the non-existent user without generating a token or sending any email

            if (user == null)
            {
                logger.LogWarning($"User with email {email} does not exist.");
                return OperationResult.Success(); // No error; simply return success to avoid leaking user existence info
            }
            else if (!isEmailConfirmed)
            {
                logger.LogWarning($"User with email {email} has not confirmed their email.");
                return OperationResult.Success(); // No error; simply return success to avoid leaking user existence info
            }
           
            try
            {
                // Generate the password reset token
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                if (string.IsNullOrEmpty(token))
                {
                    logger.LogError("Password reset token generation failed.");
                    return OperationResult.Failure(new Dictionary<string, string>
                    {
                        { "ForgotPasswordError", InvalidOperationExceptionMessages.TokenGenerationUnsuccessfullExceptionMessage }
                    });
                }

                
                var data = new Dictionary<string, object>
                    {
                        { "token", token }
                    };

                return OperationResult.Success(data);
            }
            catch (Exception ex)
            {
                logger.LogError($"Token generation error: {ex.Message}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "ForgotPasswordError", "Failed to generate password reset token." }
                });
            }
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryResetPasswordAsync(ResetPasswordFormModel model)
        {
            var user = await userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                return OperationResult.Success();
            }

            try
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    return OperationResult.Success();
                }

                // Handle token expiration or invalid token
                if (result.Errors.Any(e => e.Code == "InvalidToken"))
                {
                    return OperationResult.Failure(new Dictionary<string, string>
                    {
                        { "TokenError", "The token is invalid or has expired. Please request a new password reset link." }
                    });
                }

                // Return failure with all errors
                return OperationResult.Failure(result.Errors.ToDictionary(e => e.Code, e => e.Description));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error resetting password for user {model.Email}: {ex.Message}. StackTrace: {ex.StackTrace}", ex);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "ResetPasswordError", "An error occurred while resetting the password. Please try again." }
                });
            }
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TryChangePasswordAsync(ChangePasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "UserError", UserNotFoundErrorMessage }
                });
            }

            // Attempt to change the password
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            // Handle specific error scenarios
            if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { nameof(model.CurrentPassword), IncorrectPasswordErrorMessage }
                });
            }

            // Return failure with all other errors
            return OperationResult.Failure(result.Errors.ToDictionary(e => e.Code, e => e.Description));
        }

        /// <inheritdoc/>
        public async Task<OperationResult> TrySetPasswordAsync(SetPasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "UserError", UserNotFoundErrorMessage }
                });
            }

            // Attempt to set the password
            var hasPassword = await userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return OperationResult.Failure(new Dictionary<string, string> 
                {
                   { "PasswordAlreadySet",
                     "The user already has a password set." }
                });
            }

            // Add the new password
            var result = await userManager.AddPasswordAsync(user, model.NewPassword);

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }

            // Return failure with all errors encountered during setting the password
            return OperationResult.Failure(result.Errors.ToDictionary(e => e.Code, e => e.Description));
        }
        /// <inheritdoc/>
        public async Task<OperationResult> TryDeleteUserAccountAsync()
        {
            
            if (userId == default)
            {
                logger.LogError($"No user is logged in. Cannot delete account");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "UserError", "No user found." }
                });
            }

            // Delete the user and related data
            var deletionResult = await DeleteUserAndUserDataAsync();

            if (!deletionResult.Succeeded)
            {
                return deletionResult; // Return the failure from the helper method
            }

            // Sign out the user after successful deletion
            try
            {
                await signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error signing out user {userId} after account deletion: {ex.Message}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "SignOutError", "An error occurred while signing out after deleting the account." }
                });
            }

            return OperationResult.Success();
        }
        /// <inheritdoc/>
        public async Task SendConfirmationEmailAsync(string email, string? callbackUrl)
        {
            try
            {
                var emailResult = await emailSender.SendEmailConfirmationAsync(email, callbackUrl);

                if (!emailResult.Succeeded)
                {
                    // Handle email sending failure (if necessary)
                    logger.LogError($"Sending email for confirmation failed. Errors: {emailResult.Errors}");

                    throw new InvalidOperationException("Failed to send confirmation email.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors related to email sending
                logger.LogError(ex, "Error occurred while sending confirmation email.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SendPasswordResetEmailAsync(string email, string? callbackUrl)
        {
            try
            {
                string tokenExpirationTime = DateTime.Now.AddHours(TokenExpirationDefaultHoursTime).ToString();

                var emailResult = await emailSender.SendPasswordResetEmailAsync(email, callbackUrl, tokenExpirationTime);

                if (!emailResult.Succeeded)
                {
                    // Handle email sending failure
                    logger.LogError($"Sending email for password reset failed. Errors: {emailResult.Errors}");
                    throw new InvalidOperationException("Failed to send password reset email.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors related to email sending
                logger.LogError(ex, "Error occurred while sending password reset email.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteUserByIdIfExistsAsync(Guid userId)
        {
            ApplicationUser? user = await userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }

        // HELPER METHODS:
       
        /// <summary>
        /// Utility method that accepts a RegisterFormModel and tries to create an ApplicationUser in the database. If it fails, logs an error and throws an exception
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ApplicationUser</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<ApplicationUser> CreateUserAsync(RegisterFormModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                logger.LogError($"User creation failed. Errors: {result.Errors}");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.UserUnsuccessfullyCreatedExceptionMessage);
            }

            return user;
        }

        /// <summary>
        /// Utility method that generates a token for a given user for email confirmation. If user is null or the token is null, logs errors and throws exceptions.
        /// </summary>
        /// <param name="user">The Application User</param>
        /// <returns>string, the token</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<string> GenerateTokenForEmailConfirmationAsync(ApplicationUser user)
        {
            if (user == null)
            {
                logger.LogError($"The user is null. Cannot proceed with action: {nameof(GenerateTokenForEmailConfirmationAsync)}");
                throw new ArgumentNullException(ArgumentNullExceptionMessages.UserNullExceptionMessage);
            }

            string? token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token == null)
            {
                logger.LogError($"Failed to generate token for user: {user.Id.ToString()} in action {nameof(GenerateTokenForEmailConfirmationAsync)}");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.TokenGenerationUnsuccessfullExceptionMessage);
            }

            return token;

        }

        
       
    }
}
