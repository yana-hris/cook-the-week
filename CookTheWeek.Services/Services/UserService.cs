namespace CookTheWeek.Services.Data.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
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

    using static Common.GeneralApplicationConstants;
    using static Common.ExceptionMessagesConstants;
    using static CookTheWeek.Common.EntityValidationConstants;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        private readonly IRecipeService recipeService;
        private readonly IMealPlanService mealPlanService;
        private readonly IValidationService validationService;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailSender emailSender;

        private readonly ILogger<UserService> logger;

        public UserService(IUserRepository userRepository,
            IRecipeService recipeService,
            IValidationService validationService,
            IEmailSender emailSender,
            IMealPlanService mealPlanService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserService> logger)
        {
            this.httpContextAccessor = httpContextAccessor;

            this.userRepository = userRepository;

            this.recipeService = recipeService;
            this.mealPlanService = mealPlanService;
            this.validationService = validationService;

            this.emailSender = emailSender;
            this.logger = logger;   
        }
        public async Task<UserProfileViewModel> GetDetailsModelByIdAsync(string userId)
        {
            // This Throws RecordNotFoundException
            var user = await userRepository.GetByIdAsync(userId);

            bool hasPassword = await userRepository.HasPasswordAsync(user);

            return new UserProfileViewModel
            {
                UserName = user.UserName!,
                Email = user.Email!,
                HasPassword = hasPassword
            };
        }

        
        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            // Check if the current password matches
            // userRepository.CheckPasswordAsync
            bool oldPasswordMatches = await userRepository.CheckPasswordAsync(user, model.CurrentPassword);

            if (!oldPasswordMatches)
            {
                return IdentityResult.Failed(new IdentityError { Description = IncorrectPasswordErrorMessage });
            }

            var result = await userRepository.ChangePassAsync(user, model.CurrentPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> SetPasswordAsync(string userId, SetPasswordFormModel model)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                // Returning an error result if the user is not found
                return IdentityResult.Failed(new IdentityError { Description = UserNotFoundErrorMessage });
            }

            var result = await userRepository.AddPasswordAsync(user, model.NewPassword);

            return result;
        }

        /// <summary>
        /// Deletes the user and all user data: meal plans (and related meals), added recipes, favourite recipes.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUserAndUserDataAsync(string userId)
        {
            // Delete related data first
            await mealPlanService.DeleteAllByUserIdAsync(userId);
            await recipeService.DeleteAllByUserIdAsync(userId);

            var user = await userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                await userRepository.DeleteAsync(user);
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
        public async Task<ICollection<UserAllViewModel>> AllAsync()
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
                user.TotalRecipes = await recipeService.GetMineCountAsync(user.Id.ToString());
                user.TotalMealPlans = await mealPlanService.MineCountAsync(user.Id);
            }

            return users;
        }

        

        

        /// <summary>
        /// Gets the currently logged in user id
        /// </summary>
        /// <returns></returns>
        public string? GetCurrentUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            string? userId = userRepository.GetUserId(user);

            return userId;
        }


        /// <summary>
        /// Takes a RegisterFormModel and tries to create an ApplicationUser in the database. If creation is successful, proceeds to generating a Token for Email confirmation. If token generation is successful, sends an email to the user with the confirmation link.
        /// Logs errors and catches exceptions on the way. In case of failure, returns a dictionary with Errors.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The result of the Operation (Success or Failure)</returns>
        public async Task<OperationResult> TryRegisterUserAsync(RegisterFormModel model)
        {
            string userId = null;

            try
            {
                // Step 1: Create the user
                ApplicationUser user = await CreateUserAsync(model);

                // Step 2: Generate token for email confirmation
                var token = GenerateTokenForEmailConfirmationAsync(user);

                // TODO: check!
                // Step 3: Construct the email confirmation callback URL
                userId = user.Id.ToString();
                var request = httpContextAccessor.HttpContext.Request;
                var callbackUrl = $"{request.Scheme}://{request.Host}/User/ConfirmedEmail?userId={userId}&code={token}";

                // Step 4: Send confirmation email
                var emailResult = await emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                if (emailResult.Succeeded)
                {
                    return OperationResult.Success(); // All steps succeeded
                }

                // Delete created user to clean up unsuccessful regitsration in case of failed email sending
                await DeleteUserByUserIdAsync(userId);
                return OperationResult.Failure(emailResult.Errors); // Email sending failed
            }
            catch (InvalidOperationException ex)
            {
                await DeleteUserByUserIdAsync(userId); //delete user in case token generation failed
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "RegisterError", ex.Message }
                });
            }
            catch(ArgumentNullException ex)
            {
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "RegisterError", ex.Message }
                });
            }
        }

        /// <summary>
        /// Finds the user by the userId and if not null, proceeds to email confirmation using the given token. If successful, logs the user in.
        /// If token or user are null, a Failure result is returned.
        /// Does not throw any exceptions, catches RecordNotFound and ArgumentNullExceptions from previous methods.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns>The Operation Result (Success or Failure)</returns>
        public async Task<OperationResult> TyrConfirmEmailAsync(string userId, string code)
        {
            var user = await userRepository.GetByIdAsync(userId);

            try
            {
                var confirmationResult = await userRepository.ConfirmEmailAsync(user, code);
                await userRepository.SignInAsync(user);

                return OperationResult.Success();   
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError($"Token for Email Confirmation is null. UserId: {userId}. Token: {code}");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {"EmailConfirmationError", ex.Message }
                });
            }
            catch(RecordNotFoundException ex)
            {
                logger.LogError($"Email Confirmation failed. User with userId: {userId} not found in the database.");
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    { "EmailConfirmationError", ex.Message }
                });
            }
        }

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

            var signInResult = await userRepository.PasswordSignInAsync(user, model.Password, model.RememberMe);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    logger.LogError($"User Profile Locked. Sign in failed.");
                }
                else
                {
                    logger.LogError($"Sign in failed for user {model.Username}. Invalid credentials.");
                }

                await userRepository.AccessFailedAsync(user);
                return OperationResult.Failure(new Dictionary<string, string>
                {
                    {string.Empty,  ApplicationUserValidation.LoginFailedErrorMessage}
                });
            }

            return OperationResult.Success();
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

            IdentityResult result = await userRepository.AddAsync(user, model.Password);

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

            string? token = await userRepository.GenerateEmailConfirmationTokenAsync(user);

            if (token == null)
            {
                logger.LogError($"Failed to generate token for user: {user.Id.ToString()} in action {nameof(GenerateTokenForEmailConfirmationAsync)}");
                throw new InvalidOperationException(InvalidOperationExceptionMessages.TokenGenerationUnsuccessfullExceptionMessage);
            }

            return token;

        }

        /// <summary>
        /// Utility method that deletes a user upon accepting user id. Does not throw exceptions. If the userId cannot be found, will not throw exceptions.
        /// </summary>
        /// <param name="user">The user ID</param>
        /// <returns></returns>
        private async Task DeleteUserByUserIdAsync(string userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                await userRepository.DeleteAsync(user);
            }
        }

        
    }
}
